using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Door,
    Treasure,
    Consumable,
    Equipment,
    Item,
    Profession,
    Race,
    Monster
}

public abstract class Card
{
    public int id;
    public string title;
    public CardType type;

    public Sprite artwork;

    public int cost;
    public int stat;

    public Card(string name, CardType type, Sprite artwork, int cost, int stat)
    {
        this.title = name;
        this.type = type;
        this.cost = cost;
        this.stat = stat;
        this.artwork = null;
    }
}
