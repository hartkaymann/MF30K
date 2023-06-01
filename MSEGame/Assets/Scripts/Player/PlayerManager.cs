using TMPro;
using UnityEngine;

public class PlayerManager : Manager<PlayerManager>
{
    public PlayerController PlayerController { get; private set; }

    [SerializeField] private GameObject humanPrefab;
    [SerializeField] private GameObject elfPrefab;
    [SerializeField] private GameObject playerInfoPrefab;
    [SerializeField] private TextMeshProUGUI playerGold;

    private GameObject currentPlayerInfo;

    protected override void Init()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStageChanged;

        // Destroy initial player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            Destroy(player);
        }
    }

    public void InstantiatePlayer(Player player)
    {
        if (PlayerController != null)
        {
            Destroy(PlayerController.gameObject);
        }

        // Instantiate player at specified position
        GameObject prefab = player.Race == Race.Human? humanPrefab : elfPrefab;
        GameObject obj = Instantiate(prefab, new Vector3(-2.5f, -0.72f, 0f), Quaternion.identity);
        if (obj.TryGetComponent<PlayerController>(out var pc))
        {
            pc.Player = player;
            PlayerController = pc;

            PlayerController.EquipStarterGear();
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

        player.OnPropertyChanged += UpdatePlayer;

        UpdatePlayer();
    }

    public void UpdatePlayer()
    {
        Player player = PlayerController.Player;
        if (player == null)
            return;

        StartCoroutine(NetworkManager.Instance.PutPlayer(player));

        if (currentPlayerInfo == null)
            return;

        if (currentPlayerInfo.transform.Find("Name").TryGetComponent<TextMeshProUGUI>(out var infoName))
        {
            infoName.text = player.Name;
        }

        if (currentPlayerInfo.transform.Find("Level").Find("Value").TryGetComponent<TextMeshProUGUI>(out var infoLevel))
        {
            infoLevel.text = player.Level.ToString();
        }

        if (currentPlayerInfo.transform.Find("CombatLevel").Find("Value").TryGetComponent<TextMeshProUGUI>(out var infoCombatLevel))
        {
            infoCombatLevel.text = player.CombatLevel.ToString();

            if (player.RaceEffect < 0)
                infoCombatLevel.color = GameColor.Red;
            else if (player.RaceEffect > 0)
                infoCombatLevel.color = GameColor.Green;
            else
                infoCombatLevel.color = GameColor.White;
        }

        if (playerGold != null)
        {
            playerGold.text = $"{player.Gold}/10";
        }

        // Player level maxed, end of game
        if (player.Level == 10)
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
            PlayerController.Player.Profession = professionCard.profession;
        }
        else if (card is RaceCard raceCard)
        {
            PlayerController.Player.Race = raceCard.race;
        }
        else
        {
            Debug.LogWarning("Cannot apply cange!");
        }
    }

    private void GameManagerOnGameStageChanged(GameStage stage)
    {
        if (PlayerController.Player == null)
        {
            Debug.Log("No player, returning and not updating");
            return;
        }

        Player player = PlayerController.Player;

        // New round start
        if (stage == GameStage.InventoryManagement)
        {
            player.RoundBonus = 0;
        }

        if (stage == GameStage.CombatPreparations)
        {
            // WE CHANGE IT HERE BECAUSE I DONT HAVE TIME TO MAKE PRETTY CODE RIGHT NOW SO MAYBE CHANGE THIS LATER IF YOU CAN BECAUSE ITS MAKING MY EYES BLEED
            if (player.Race == Race.Orc)
            {
                if (RoomManager.Instance.CurrentRoom.Card.title.ToLower().EndsWith("slime"))
                    player.RaceEffect = 1; // Orc & Slime = +1
                else if (RoomManager.Instance.CurrentRoom.Card.title.ToLower().EndsWith("ghost"))
                    player.RaceEffect = -1; // Orc & Slime = -1
            }
            else if (player.Race == Race.Elf)
            {
                if (RoomManager.Instance.CurrentRoom.Card.title.ToLower().EndsWith("ghost"))
                    player.RaceEffect = 1; // Elf & Ghost = +1
                else if (RoomManager.Instance.CurrentRoom.Card.title.ToLower().EndsWith("slime"))
                    player.RaceEffect = -1; // Elf & Slime = -1
            }
            else
            {
                player.RaceEffect = 0; // Human = 0
            }

            Debug.Log($"Applying Race Effect: {player.Race}");
        }
    }
}