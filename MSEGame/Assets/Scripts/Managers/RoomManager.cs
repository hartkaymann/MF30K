using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;


    private RoomController currentRoom;
    public RoomController CurrentRoom { get { return currentRoom; } }

    [SerializeField] private GameObject roomPrefab;

    void Awake()
    {
        instance = this;
    }

    public void InstantiateRoom(DoorCard card)
    {
        // Destroy old room
        if (currentRoom != null)
            Destroy(currentRoom.gameObject);

        // Create new room
        GameObject go = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);
        currentRoom = go.GetComponent<RoomController>();
        currentRoom.Card = card;
    }
}
