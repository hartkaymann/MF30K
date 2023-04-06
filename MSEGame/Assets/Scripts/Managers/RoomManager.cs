using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance; 

    public Monster monster;
    public Player player;

    public GameObject enemy;
    public GameObject treasure;

    void Awake()
    {
        instance = this; 
    }

    public void InstantiateRoom()
    {
        
    }
}
