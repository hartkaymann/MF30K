using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardTypes
{
    Consumable,
    Equipment,
    Item,
    Profession,
    Race
}

public abstract class Card
{
    public string name;
    public CardTypes type;

    public Sprite artwork;

    public int cost;
    public int stat;

    public Card(string name, CardTypes type, Sprite artwork, int cost, int stat)
    {
        this.name = name;
        this.type = type;
        this.cost = cost;
        this.stat = stat;
        this.artwork = null;
    }
}
