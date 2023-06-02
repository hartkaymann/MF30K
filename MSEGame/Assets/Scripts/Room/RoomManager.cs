using TMPro;
using UnityEngine;

public class RoomManager : Manager<RoomManager>
{
    [SerializeField] private RoomController currentRoom;
    public RoomController CurrentRoom { get { return currentRoom; } }

    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private GameObject ghostPrefab;

    public void InstantiateRoom(DoorCard card)
    {
        // Destroy old room
        if (currentRoom != null)
            Destroy(currentRoom.gameObject);

        // Create new room
        GameObject obj = Instantiate(roomPrefab, new Vector3(0, 1.5f, 0), Quaternion.identity);
        currentRoom = obj.GetComponent<RoomController>();
        currentRoom.Card = card;

        Transform npcPosition = currentRoom.transform.Find("NPC");
        if (card is MonsterCard monsterCard)
        {
            GameObject prefab = monsterCard.title.ToLower().EndsWith("slime") ? slimePrefab : ghostPrefab;
            currentRoom.NPC = Instantiate(prefab, npcPosition.position, Quaternion.identity, npcPosition).GetComponent<NpcController>();
        }
        else
        {
            currentRoom.NPC = Instantiate(npcPrefab, npcPosition.position, Quaternion.identity, npcPosition).GetComponent<NpcController>();
        }
    }
}
