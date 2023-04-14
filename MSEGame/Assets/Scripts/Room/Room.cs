using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    TreasureRoom,
    MonsterRoom
}

public class Room
{
    public int id;
    public RoomType type;
    public Monster monster;
    public Sprite sprite;

    public Room(int id, RoomType type, Monster monster, Sprite sprite)
    {
        this.id = id;
        this.type = type;
        this.monster = monster;
        this.sprite = sprite;
    }   
}
