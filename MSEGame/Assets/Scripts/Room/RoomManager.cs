using TMPro;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    [SerializeField] private RoomController currentRoom;
    public RoomController CurrentRoom { get { return currentRoom; } }

    [SerializeField] private GameObject roomPrefab;

    [SerializeField] private GameObject npcInfoPrefab;
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
        GameObject obj = Instantiate(roomPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        currentRoom = obj.GetComponent<RoomController>();
        currentRoom.Card = card;

        // Create NPC info
        if (currentNpcInfo != null)
            Destroy(currentNpcInfo);

        currentNpcInfo = Instantiate(npcInfoPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("UI").transform);
        Transform npc = obj.transform.Find("NPC");
        if (currentNpcInfo.TryGetComponent<ObjectFollow>(out var follow))
        {
            follow.Follow = npc.Find("Info");
        }

        if (currentNpcInfo.transform.Find("Name").TryGetComponent<TextMeshProUGUI>(out var infoName))
        {
            infoName.text = card.title;
        }

        if (currentNpcInfo.transform.Find("Level").TryGetComponent<TextMeshProUGUI>(out var infoLevel))
        {
            if (card is MonsterCard monsterCard)
            {
                infoLevel.gameObject.SetActive(true);
                infoLevel.text = monsterCard.level.ToString();
            }
            else
            {
                infoLevel.gameObject.SetActive(false);
            }
        }
    }
}
