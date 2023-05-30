using TMPro;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    [SerializeField] private RoomController currentRoom;
    public RoomController CurrentRoom { get { return currentRoom; } }

    [SerializeField] private GameObject roomPrefab;

    [SerializeField] private GameObject npcInfoPrefab;
    [SerializeField] private GameObject enemyInfoPrefab;
    private GameObject currentNpcInfo;

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

    public void InstantiateRoom(DoorCard card)
    {
        // Destroy old room
        if (currentRoom != null)
            Destroy(currentRoom.gameObject);

        // Create new room
        GameObject obj = Instantiate(roomPrefab, new Vector3(0, 1.5f, 0), Quaternion.identity);
        currentRoom = obj.GetComponent<RoomController>();
        currentRoom.Card = card;


        // Create NPC info
        if (currentNpcInfo != null)
            Destroy(currentNpcInfo);

        if (card is MonsterCard monsterCard)
        {
            currentNpcInfo = Instantiate(enemyInfoPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("UI").transform);

            if (currentNpcInfo.transform.Find("Level").TryGetComponent<TextMeshProUGUI>(out var infoLevel))
            {
                infoLevel.text = monsterCard.level.ToString();
            }
        }
        else
        {
            currentNpcInfo = Instantiate(npcInfoPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("UI").transform);
        }

        Transform npc = obj.transform.Find("NPC");
        if (currentNpcInfo.TryGetComponent<ObjectFollow>(out var follow))
        {
            follow.Follow = npc.Find("Info");
        }

        if (currentNpcInfo.transform.Find("Name").TryGetComponent<TextMeshProUGUI>(out var infoName))
        {
            infoName.text = card.title;
        }
    }
}
