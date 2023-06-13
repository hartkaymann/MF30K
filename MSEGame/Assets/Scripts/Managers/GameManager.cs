using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class GameManager : Manager<GameManager>
{
    // State management
    private GameStage stage;
    private Combat currentCombat;
    public GameStage Stage { get { return stage; } }

    public static event Action<GameStage> OnGameStageChange;
    public static event Action OnNewCycle;
    public static event Action<DoorCard> OnChangeClass;

    [SerializeField] private Slider timeSlider;
    private Coroutine stageTimerCoroutine;

    [SerializeField] private CombatWheelController combatWheel;

    void Start()
    {
        FetchPlayerInformation();
    }

    public void UpdateGameStage(GameStage newStage)
    {
        stage = newStage;
        Debug.Log("New Stage: " + stage.ToString());

        switch (newStage)
        {
            case GameStage.InventoryManagement:
                RestartStageTimer(20);
                RoomManager.Instance.CurrentRoom.OpenDoor();
                OnNewCycle?.Invoke();
                break;
            case GameStage.DrawCard:
                RestartStageTimer(10);
                PlayerController playerController = PlayerManager.Instance.PlayerController;
                Vector3 doorPosition = RoomManager.Instance.CurrentRoom.transform.Find("Door").gameObject.transform.position;

                UIManager.Instance.ToggleBlackScreen();
                StartCoroutine(UIManager.Instance.FadeToBlack(1f));
                PlayerManager.Instance.PlayerController.RunForDuration(.9f);
                StartCoroutine(AnimationManager.Instance.MoveFromTo(playerController.transform, playerController.transform.position, doorPosition, .9f));

                Invoke(nameof(DrawDoorCard), 1f);
                break;
            case GameStage.CombatPreparation:
                RestartStageTimer(20);
                break;
            case GameStage.Selection:
                RestartStageTimer(20);
                ChangeClass();
                break;
            case GameStage.Combat:
                RestartStageTimer(120);
                Combat();
                break;
            case GameStage.Victory:
                RestartStageTimer(120);
                Victory();
                break;
            case GameStage.Defeat:
                RestartStageTimer(120);
                Defeat();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newStage), newStage, null);
        }

        OnGameStageChange?.Invoke(newStage);
        StartCoroutine(NetworkManager.Instance.PutStage(stage));
    }

    async void FetchPlayerInformation()
    {
        Player player;

        if (SessionData.Username.Length == 0)
        {
            Debug.Log("No player, getting Dummy.");
            player = Player.GetDummy();
        }
        else
        {
            player = await NetworkManager.Instance.GetPlayer(SessionData.Username);
        }

        PlayerManager.Instance.InstantiatePlayer(player, true);
        StartCoroutine(NetworkManager.Instance.PostRun(PlayerManager.Instance.PlayerController.Player));

        UpdateGameStage(GameStage.InventoryManagement);
    }

    async void DrawDoorCard()
    {
        if (await NetworkManager.Instance.GetCard(CardCategory.Door) is not DoorCard card)
            return;

        RoomManager.Instance.InstantiateRoom(card);

        UIManager.Instance.ToggleBlackScreen();
        NextStage();
    }

    async void DrawTreasureCard()
    {
        Card card = await NetworkManager.Instance.GetCard(CardCategory.Treasure);
        if (card == null)
            return;

        CardManager.Instance.DrawCardFromStack(card);
    }

    async void Combat()
    {
        RoomController rc = RoomManager.Instance.CurrentRoom;
        if (rc.Card is not MonsterCard monsterCard)
        {
            Debug.LogWarning($"Trying to do combat while not in monster room. Current room: {rc.Card.type}");
            return;
        }

        int playerLvl = PlayerManager.Instance.PlayerController.Player.CombatLevel;
        int enemyLvl = monsterCard.Level;

        currentCombat = new()
        {
            CombatLvlPlayer = playerLvl,
            CombatLvlMonster = enemyLvl
        };

        currentCombat.Win = await TurnCombatWheel();

        // If knight ability is acive, you can go again
        if (PlayerManager.Instance.PlayerController.TryGetComponent<KnightController>(out var knightCtrl))
        {
            if (!currentCombat.Win && knightCtrl.Active)
                currentCombat.Win = await TurnCombatWheel();
            knightCtrl.Active = false;
        }

        if(currentCombat.Win)
            StartCoroutine(SequenceCombat());
        else
            Invoke(nameof(NextStage), 1f);
    }

    private IEnumerator SequenceCombat()
    {
        PlayerController currentPlayer = PlayerManager.Instance.PlayerController;
        Transform playerTransform = currentPlayer.gameObject.transform;
        Transform npcTransform = RoomManager.Instance.CurrentRoom.gameObject.transform.Find("NPC");

        Vector3 startPos = playerTransform.position;
        float playerWidth = currentPlayer.GetComponent<BoxCollider2D>().size.x;

        currentPlayer.StartRunning();
        yield return StartCoroutine(AnimationManager.Instance.MoveFromTo(playerTransform, startPos, npcTransform.position - new Vector3(playerWidth, 0, 0), 1f));

        currentPlayer.Attack();
        yield return new WaitForSeconds(0.8f);

        if (currentCombat.Win)
        {
            RoomManager.Instance.CurrentRoom.NPC.Die();
        }
        else
        {
            if (currentPlayer.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.AddForce(startPos - playerTransform.position + Vector3.up, ForceMode.Impulse);
                yield return new WaitForSeconds(1);
            }
        }
        yield return new WaitForSeconds(.5f);

        playerTransform.Rotate(Vector2.up, 180f);
        yield return StartCoroutine(AnimationManager.Instance.MoveFromTo(playerTransform, playerTransform.position, startPos, 1f));
        currentPlayer.StopRunning();
        playerTransform.Rotate(Vector2.up, 180f);


        Invoke(nameof(NextStage), 1f);
    }

    private async Task<bool> TurnCombatWheel()
    {
        combatWheel.Reset();
        UIManager.Instance.ToggleCombatPanel();
        combatWheel.SetRatio(currentCombat.CombatLvlPlayer / (float)(currentCombat.CombatLvlPlayer + currentCombat.CombatLvlMonster));
        while (!combatWheel.IsFinished)
        {
            await Task.Delay(500);
        }
        UIManager.Instance.ToggleCombatPanel();

        return combatWheel.GetResult();
    }

    private void Victory()
    {
        currentCombat.Consequence = 0;
        StartCoroutine(NetworkManager.Instance.PostCombat(PlayerManager.Instance.PlayerController.Player, currentCombat));

        PlayerManager.Instance.PlayerController.Player.Level += 1;
        RoomManager.Instance.CurrentRoom.Renderer.OpenTreasure(false);
        DrawTreasureCard();

        if (PlayerManager.Instance.PlayerController.TryGetComponent<RogueController>(out var rogueCtrl) && rogueCtrl.IsActive)
        {
            DrawTreasureCard();
        }
    }

    private void Defeat()
    {
        RoomManager.Instance.CurrentRoom.Renderer.OpenTreasure(true);
    }

    public void ConsequenceChosen(int consequence)
    {
        currentCombat.Consequence = consequence;
        StartCoroutine(NetworkManager.Instance.PostCombat(PlayerManager.Instance.PlayerController.Player, currentCombat));

        // Apply consequence effect
        switch (consequence)
        {
            case 0:
                // Nothing happened
                break;
            case 1:
                {
                    // Lose Eqipment Card
                    PlayerController pc = PlayerManager.Instance.PlayerController;

                    Array values = Enum.GetValues(typeof(EquipmentSlot));

                    bool slotNotEmpty = false;
                    int tries = 0;
                    while (!slotNotEmpty && tries < values.Length)
                    {
                        tries++;

                        // Chose random slot
                        EquipmentSlot randSlot = (EquipmentSlot)values.GetValue(Random.Range(0, values.Length));
                        Debug.Log("Searching for spot, current: " + randSlot);
                        Transform cardGo = GameObject.Find($"Equipment/Slots/{randSlot}/Slot").transform.GetChild(0);
                        if (cardGo != null)
                        {
                            Debug.Log("Slot found!");
                            slotNotEmpty = true;
                            if (cardGo.TryGetComponent<CardController>(out var ctrl))
                            {
                                ctrl.Discard();
                            }
                        }
                    }
                    break;
                }
            case 2:
                {
                    GameObject hand = GameObject.Find($"Hand/Grid");
                    if (hand.transform.childCount == 0)
                        break;

                    int idx = Random.Range(0, hand.transform.childCount);
                    if (hand.transform.GetChild(idx).TryGetComponent<CardController>(out var ctrl))
                    {
                        ctrl.Discard();
                    }
                    break;
                }
            case 3:
                {
                    // Lose Level
                    PlayerManager.Instance.PlayerController.Player.Level -= 1;
                    break;
                }
            default:
                Debug.LogAssertion("Consequence index out of Range: " + consequence);
                break;
        }

        Invoke(nameof(NextStage), 2);
    }

    private void ChangeClass()
    {
        // Get doorcard and notify UI
        DoorCard card = RoomManager.Instance.CurrentRoom.Card;
        if (card.type is CardType.Monster)
            return;

        OnChangeClass?.Invoke(card);
    }

    public void EndOfGame(Player player)
    {
        // Show victory screen?
        Debug.Log($"Victory");

        StartCoroutine(NetworkManager.Instance.PutRun(player));

        Invoke(nameof(Exit), 3);
    }

    public void Exit()
    {
        SceneManager.LoadScene("Outro");
    }


    private void RestartStageTimer(float time = 30f)
    {
        if (stageTimerCoroutine != null)
        {
            StopCoroutine(stageTimerCoroutine);
        }

        stageTimerCoroutine = StartCoroutine(StageTimer(time));
    }

    private IEnumerator StageTimer(float time)
    {
        yield return Countdown(time);
        NextStage();
    }
    private IEnumerator Countdown(float timeInSeconds)
    {
        for (float remaining = timeInSeconds; remaining >= 0; remaining -= Time.deltaTime)
        {
            timeSlider.value = remaining;
            yield return null;
        }
    }

    public void NextStage()
    {
        switch (stage)
        {
            case GameStage.InventoryManagement:
                UpdateGameStage(GameStage.DrawCard);
                break;
            case GameStage.DrawCard:
                if (RoomManager.Instance.CurrentRoom.Card.type == CardType.Monster)
                    UpdateGameStage(GameStage.CombatPreparation);
                else
                    UpdateGameStage(GameStage.Selection);
                break;
            case GameStage.Selection:
                UpdateGameStage(GameStage.InventoryManagement);
                break;
            case GameStage.CombatPreparation:
                UpdateGameStage(GameStage.Combat);
                break;
            case GameStage.Combat:
                UpdateGameStage(currentCombat.Win ? GameStage.Victory : GameStage.Defeat);
                break;
            case GameStage.Victory:
                UpdateGameStage(GameStage.InventoryManagement);
                break;
            case GameStage.Defeat:
                UpdateGameStage(GameStage.InventoryManagement);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(stage), stage, null);
        }
    }
}

public enum GameStage
{
    InventoryManagement,
    DrawCard,
    Selection,
    CombatPreparation,
    Combat,
    Victory,
    Defeat
}

public static class SessionData
{
    public static string Username { get; set; } = "";
}

public static class GameColor
{
    public static Color Red { get; private set; } = new Color(0.754717f, 0.2897656f, 0.2520469f);
    public static Color Green { get; private set; } = new Color(0.01568628f, 0.6431373f, 0.2431373f);
    public static Color White { get; private set; } = new Color(0.9529412f, 0.9333334f, 0.8901961f);
}