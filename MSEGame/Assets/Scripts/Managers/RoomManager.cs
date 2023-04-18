using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    private RoomRenderer roomRenderer;

    private Room currentRoom;

    [SerializeField] private GameObject roomPrefab;

    void Awake()
    {
        instance = this;
        roomRenderer = roomPrefab.GetComponent<RoomRenderer>();
    }

    public void InstantiateRoom(Room room)
    {
        currentRoom = room;

        GameObject go = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);
        roomRenderer.Render(room);
    }
}
