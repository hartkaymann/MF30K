using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableCard : Card
{
    public ConsumableCard(string name, Sprite artwork, int cost, int stat) : base(name, CardTypes.Consumable, artwork, cost, stat){
        
    }
}
