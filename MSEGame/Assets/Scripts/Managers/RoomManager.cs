using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    public RoomRenderer roomRenderer;

    private Room currentRoom;

    private GameObject roomPrefab;
    public GameObject treasure;
    public GameObject enemy;

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
