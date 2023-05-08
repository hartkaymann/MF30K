using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private PlayerController currentPlayer;
    public PlayerController CurrentPlayer { get { return currentPlayer; } }

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerInfoPrefab;
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
    }

    public void InstantiatePlayer(Player player)
    {
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }

        // Instantiate player at specified position
        GameObject obj = Instantiate(playerPrefab, new Vector3(-2.5f, -0.55f, 0f), Quaternion.identity);
        if (obj.TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.Player = player;
            currentPlayer = playerController;
        }

        // Create player info
        if(currentPlayerInfo != null)
        {
            Destroy(currentPlayerInfo);
        }

        currentPlayerInfo = Instantiate(playerInfoPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("UI").transform);
        if (currentPlayerInfo.TryGetComponent<ObjectFollow>(out var follow))
        {
            follow.Follow = obj.transform.Find("Info").transform;
        }

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

    }
}
