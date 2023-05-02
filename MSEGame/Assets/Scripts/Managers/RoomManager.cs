using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    private RoomRenderer roomRenderer;

    // Current room infomation
    private DoorCard card;
    private RoomController currentRoom; 

    [SerializeField] private GameObject roomPrefab;

    void Awake()
    {
        instance = this;
        roomRenderer = roomPrefab.GetComponent<RoomRenderer>();
    }

    public void InstantiateRoom(DoorCard card)
    {
        this.card = card;

        // Destroy old room
        Destroy(currentRoom.gameObject);

        // Create new room
        GameObject go = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);
        currentRoom = go.GetComponent<RoomController>();
        roomRenderer.Render(card);
    }
}
