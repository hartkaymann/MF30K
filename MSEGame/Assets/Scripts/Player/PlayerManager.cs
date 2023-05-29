using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [SerializeField] private PlayerController currentPlayer;
    public PlayerController CurrentPlayer { get { return currentPlayer; } }

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerInfoPrefab;
    [SerializeField] private TextMeshProUGUI playerGold;

    private GameObject currentPlayerInfo;

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

        GameManager.OnGameStateChange += GameManagerOnGameStageChanged;
    }

    public void InstantiatePlayer(Player player)
    {
        if (currentPlayer != null)
        {
            Destroy(currentPlayer.gameObject);
        }

        // Instantiate player at specified position
        GameObject obj = Instantiate(playerPrefab, new Vector3(-2.5f, -0.72f, 0f), Quaternion.identity);
        if (obj.TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.Player = player;
            currentPlayer = playerController;

            playerController.EquipStarterGear();
        }

        // Follow new player
        if (currentPlayerInfo != null)
        {
            Destroy(currentPlayerInfo);
        }

        currentPlayerInfo = Instantiate(playerInfoPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("UI").transform);
        if (currentPlayerInfo.TryGetComponent<ObjectFollow>(out var follow))
        {
            follow.Follow = obj.transform.Find("Info").transform;
        }

        UpdatePlayer(player);
    }

    public void UpdatePlayer(Player player)
    {
        StartCoroutine(NetworkManager.Instance.PutPlayer(player));

        if (currentPlayerInfo == null)
            return;

        if (currentPlayerInfo.transform.Find("Name").TryGetComponent<TextMeshProUGUI>(out var infoName))
        {
            infoName.text = player.Name;
        }

        if (currentPlayerInfo.transform.Find("Level").TryGetComponent<TextMeshProUGUI>(out var infoLevel))
        {
            infoLevel.text = player.Level.ToString();
        }

        if (currentPlayerInfo.transform.Find("CombatLevel").TryGetComponent<TextMeshProUGUI>(out var infoCombatLevel))
        {
            infoCombatLevel.text = player.CombatLevel.ToString();
        }

        if (playerGold != null)
        {
            playerGold.text = $"{player.Gold}/10";
        }

        // Player level maxed, end of game
        if( player.Level == 10)
        {
            GameManager.Instance.EndOfGame(player);
        }
    }

    //TODO: Hand this to current player controller
    public void ChangeCurrentPlayerClass()
    {
        // Get doorcard and notify UI
        DoorCard card = RoomManager.Instance.CurrentRoom.Card;

        if (card is ProfessionCard professionCard)
        {
            CurrentPlayer.Player.Profession = professionCard.profession;
        }
        else if (card is RaceCard raceCard)
        {
            CurrentPlayer.Player.Race = raceCard.race;
        }
        else
        {
            Debug.LogWarning("Cannot apply cange!");
        }
    }

    private void GameManagerOnGameStageChanged(GameStage stage)
    {
        // New round start
        if (stage == GameStage.InventoryManagement)
        {
            if (CurrentPlayer.Player != null)
            {
                CurrentPlayer.Player.RoundBonus = 0;
            }
        }
    }
}
