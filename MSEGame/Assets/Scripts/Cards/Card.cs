using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public string name;
    public string type;

    public Sprite artwork;

    public int cost;
    public int stat;

    public Card(string name, string type, Sprite artwork, int cost, int stat)
    {
        this.name = name;
        this.type = type;
        this.cost = cost;
        this.stat = stat;
        this.artwork = null;
    }
}
