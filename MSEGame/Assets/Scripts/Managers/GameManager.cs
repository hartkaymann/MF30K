using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : Manager<GameManager>
{
    // State management
    private GameStage stage;
    private Combat currentCombat;
    public GameStage Stage { get { return stage; } }

    public static event Action<GameStage> OnGameStateChange;
    public static event Action<DoorCard> OnChangeClass;

    [SerializeField] private Slider timeSlider;
    private Coroutine stageTimerCoroutine;

    [SerializeField] private CombatWheelController combatWheel;

    void Start()
    {
        FetchPlayerInformation();
        UpdateGameStage(GameStage.InventoryManagement);
    }

    public void UpdateGameStage(GameStage newStage)
    {
        stage = newStage;
        Debug.Log("New Stage: " + stage.ToString());

        switch (newStage)
        {
            case GameStage.InventoryManagement:
                RoomManager.Instance.CurrentRoom.OpenDoor();
                break;
            case GameStage.DrawCard:
                PlayerController playerController = PlayerManager.Instance.CurrentPlayer;
                Vector3 doorPosition = RoomManager.Instance.CurrentRoom.transform.Find("Door").gameObject.transform.position;

                UIManager.Instance.ToggleBlackScreen();
                StartCoroutine(UIManager.Instance.FadeToBlack(1f));
                StartCoroutine(AnimationManager.Instance.MoveFromTo(playerController.transform, playerController.transform.position, doorPosition, .9f));

                Invoke(nameof(DrawDoorCard), 1f);
                break;
            case GameStage.CombatPreparations:
                break;
            case GameStage.Selection:
                ChangeClass();
                break;
            case GameStage.Combat:
                Combat();
                break;
            case GameStage.Victory:
                Victory();
                break;
            case GameStage.Defeat:
                Defeat();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newStage), newStage, null);
        }

        RestartStageTimer();

        OnGameStateChange?.Invoke(newStage);
        StartCoroutine(NetworkManager.Instance.PutStage(stage));
    }

    async void FetchPlayerInformation()
    {
        string playerName = SessionData.Username;
        Player player;

        if (playerName.Length == 0)
        {
            Debug.Log("No player, getting Dummy.");
            player = Player.GetDummy();
        }
        else
        {
            player = await NetworkManager.Instance.GetPlayer(playerName);
        }

        PlayerManager.Instance.InstantiatePlayer(player);
    }

    async void DrawDoorCard()
    {
        if (await NetworkManager.Instance.GetCard(CardCategory.Door) is not DoorCard card)
            return;

        RoomManager.Instance.InstantiateRoom(card);

        UIManager.Instance.ToggleBlackScreen();

    }

    async void DrawTreasureCard()
    {
        Card card = await NetworkManager.Instance.GetCard(CardCategory.Treasure);
        if (card == null)
            return;

        CardManager.instance.DrawCardFromStack(card);
    }

    async void Combat()
    {
        RoomController rc = RoomManager.Instance.CurrentRoom;
        if (rc.Card is not MonsterCard monsterCard)
        {
            Debug.LogWarning($"Trying to do combat while not in monster room. Current room: {rc.Card.type}");
            return;
        }

        int playerLvl = PlayerManager.Instance.CurrentPlayer.Player.CombatLevel;
        int enemyLvl = monsterCard.level;

        currentCombat = new Combat()
        {
            PlayerLevel = playerLvl,
            MonsterLevel = enemyLvl
        };

        // Prepare combat wheel and wait until it finishes
        combatWheel.Reset();
        Debug.Log($"Setting ratio {playerLvl} : {enemyLvl}");
        combatWheel.SetRatio(playerLvl / (float)(playerLvl + enemyLvl));
        while (!combatWheel.IsFinished)
        {
            await Task.Delay(500);
        }
        currentCombat.Victory = combatWheel.GetResult();

        PlayerController currentPlayer = PlayerManager.Instance.CurrentPlayer;
        Transform playerTransform = currentPlayer.gameObject.transform;
        Transform npcTransform = RoomManager.Instance.CurrentRoom.gameObject.transform.Find("NPC");

        //TODO: Move there, if kill, destroy monster, if defeat, lie on ground or sth
        currentPlayer.RunForDuration(2f);
        StartCoroutine(AnimationManager.Instance.MoveAndBack(playerTransform, npcTransform.position, 2f));

        //TODO: stop invoking everything
        if (currentCombat.Victory)
            Invoke(nameof(HideMonster), 1f);

        //TODO: Very hardcoded, not a fan. Meh!
        Invoke(nameof(NextStage), 2f);
    }

    void HideMonster()
    {
        RoomManager.Instance.CurrentRoom.Renderer.ToggleNpc();
    }

    private void Victory()
    {
        currentCombat.Consequence = 0;
        StartCoroutine(NetworkManager.Instance.PostCombat(PlayerManager.Instance.CurrentPlayer.Player, currentCombat));

        PlayerManager.Instance.CurrentPlayer.Player.Level += 1;
        RoomManager.Instance.CurrentRoom.Renderer.OpenTreasure(false);
        DrawTreasureCard();
    }

    private void Defeat()
    {
        RoomManager.Instance.CurrentRoom.Renderer.OpenTreasure(true);
    }

    public void ConsequenceChosen(int consequence)
    {
        currentCombat.Consequence = consequence;
        StartCoroutine(NetworkManager.Instance.PostCombat(PlayerManager.Instance.CurrentPlayer.Player, currentCombat));

        // Apply consequence effect
        switch (consequence)
        {
            case 0:
                // Nothing happened
                break;
            case 1:
                {
                    // Lose Eqipment Card
                    PlayerController pc = PlayerManager.Instance.CurrentPlayer;

                    Array values = Enum.GetValues(typeof(EquipmentSlot));


                    // Invoke("PrayThisWorks")
                    bool slotNotEmpty = false;
                    int tries = 0;
                    while (!slotNotEmpty && tries < values.Length)
                    {
                        tries++;

                        // Chose random slot
                        EquipmentSlot randSlot = (EquipmentSlot)values.GetValue(Random.Range(0, values.Length));
                        Transform cardGo = GameObject.Find($"Equipment/Slots/{randSlot}/Slot").transform.GetChild(0);
                        if (cardGo != null)
                        {
                            Debug.Log($"Found card in slot {randSlot}");
                            slotNotEmpty = true;
                            if (cardGo.TryGetComponent<CardController>(out var ctrl))
                            {
                                Debug.Log("Discarding...");
                                ctrl.Discard();
                            }
                        }
                    }
                    break;
                }
            case 2:
                {
                    GameObject hand = GameObject.Find($"Hand/Grid");
                    int idx = Random.Range(0, hand.transform.childCount);

                    if (idx > 0 && hand.transform.GetChild(idx).TryGetComponent<CardController>(out var ctrl))
                    {
                        ctrl.Discard();
                        Debug.Log($"Discarding {idx}th hand card...");
                    }
                    break;
                }
            case 3:
                {
                    // Lose Level
                    PlayerManager.Instance.CurrentPlayer.Player.Level -= 1;
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

        StartCoroutine(NetworkManager.Instance.PostEndRun(player));

        Invoke(nameof(Exit), 3);
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }


    private void RestartStageTimer()
    {
        if (stageTimerCoroutine != null)
        {
            StopCoroutine(stageTimerCoroutine);
        }

        stageTimerCoroutine = StartCoroutine(StageTimer());
    }

    private IEnumerator StageTimer()
    {
        yield return Countdown(30);
        NextStage();
    }
    private IEnumerator Countdown(int timeInSeconds)
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
                    UpdateGameStage(GameStage.CombatPreparations);
                else
                    UpdateGameStage(GameStage.Selection);
                break;
            case GameStage.Selection:
                UpdateGameStage(GameStage.InventoryManagement);
                break;
            case GameStage.CombatPreparations:
                UpdateGameStage(GameStage.Combat);
                break;
            case GameStage.Combat:
                UpdateGameStage(currentCombat.Victory ? GameStage.Victory : GameStage.Defeat);
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
    CombatPreparations,
    Combat,
    Victory,
    Defeat
}

/// <summary>
/// Utility class to pass information between scenes.
/// </summary>
public static class SessionData
{
    public static string Username { get; set; } = "";
}

public static class GameColor
{
    public static Color Red { get; private set; } = new Color(0.754717f, 0.2897656f, 0.2520469f);
    public static Color Green { get; private set; } = new Color(0.01568628f, 0.6431373f, 0.2431373f);
}