using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCard : Card
{
    public ItemCard(string name, Sprite artwork, int cost, int stat) : base(name, CardTypes.Item, artwork, cost, stat)
    {

    }
}
