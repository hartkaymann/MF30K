using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    // State management
    private GameStage stage;
    private bool combatWasVictory = false;
    public GameStage Stage { get { return stage; } }

    public static event Action<GameStage> OnGameStateChange;
    public static event Action<DoorCard> OnChangeClass;

    [SerializeField] private Slider timeSlider;
    private Coroutine stageTimerCoroutine;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        FetchPlayerInformation();
    }

    void Start()
    {

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

                UIManager.instance.ToggleBlackScreen();
                StartCoroutine(UIManager.instance.FadeToBlack(1f));
                StartCoroutine(AnimationManager.Instance.MoveFromTo(playerController.transform, playerController.transform.position, doorPosition, .9f));

                Invoke(nameof(DrawDoorCard), 1f);
                break;
            case GameStage.CombatPreparations:
                break;
            case GameStage.ChangeClass:
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
        NetworkManager.Instance.PutStage(stage);
    }

    async void FetchPlayerInformation()
    {
        string playerName = LoadSceneInformation.PlayerName;
        Player playerInfo;

        if (playerName.Length == 0)
        {
            playerInfo = Player.GetDummy();
        }
        else
        {
            playerInfo = await NetworkManager.Instance.GetPlayer(playerName);
        }

        PlayerManager.Instance.InstantiatePlayer(playerInfo);
    }

    async void DrawDoorCard()
    {
        if (await NetworkManager.Instance.GetCard(CardCategory.Door) is not DoorCard card)
            return;

        RoomManager.Instance.InstantiateRoom(card);

        UIManager.instance.ToggleBlackScreen();

    }

    async void DrawTreasureCard()
    {
        Card card = await NetworkManager.Instance.GetCard(CardCategory.Treasure);
        if (card == null)
            return;

        CardManager.instance.DrawCardFromStack(card);
    }

    void Combat()
    {
        RoomController rc = RoomManager.Instance.CurrentRoom;
        if (rc.Card is not MonsterCard monsterCard)
        {
            Debug.LogWarning($"Trying to do combat while not in monster room. Current room: {rc.Card.type}");
            return;
        }

        combatWasVictory = PlayerManager.Instance.CurrentPlayer.Player.CombatLevel > monsterCard.level;

        PlayerController currentPlayer = PlayerManager.Instance.CurrentPlayer;
        Transform playerTransform = currentPlayer.gameObject.transform;
        Transform npcTransform = RoomManager.Instance.CurrentRoom.gameObject.transform.Find("NPC");

        //TODO: Move there, if kill, destroy monster, if defeat, lie on ground or sth
        currentPlayer.RunForDuration(2f);
        StartCoroutine(AnimationManager.Instance.MoveAndBack(playerTransform, npcTransform.position, 2f));

        //TODO: stop invoking everything ffs
        if (combatWasVictory)
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
        PlayerManager.Instance.CurrentPlayer.Player.Level += 1;
        RoomManager.Instance.CurrentRoom.Renderer.OpenTreasure(false);
        DrawTreasureCard();
    }

    private void Defeat()
    {
        // Run aways

        RoomManager.Instance.CurrentRoom.Renderer.OpenTreasure(true);
    }

    private void ChangeClass()
    {
        // Get doorcard and notify UI
        DoorCard card = RoomManager.Instance.CurrentRoom.Card;
        if (card.type is CardType.Monster)
            return;

        OnChangeClass?.Invoke(card);
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
                    UpdateGameStage(GameStage.ChangeClass);
                break;
            case GameStage.ChangeClass:
                UpdateGameStage(GameStage.InventoryManagement);
                break;
            case GameStage.CombatPreparations:
                UpdateGameStage(GameStage.Combat);
                break;
            case GameStage.Combat:
                UpdateGameStage(combatWasVictory ? GameStage.Victory : GameStage.Defeat);
                //UpdateGameStage(GameStage.Victory);
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
    ChangeClass,
    CombatPreparations,
    Combat,
    Victory,
    Defeat
}

/// <summary>
/// Utility class to pass information between scenes.
/// </summary>
public static class LoadSceneInformation
{
    public static string PlayerName { get; set; } = "";
}