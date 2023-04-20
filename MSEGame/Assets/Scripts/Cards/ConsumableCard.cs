using UnityEngine;

public class ConsumableCard : TreasureCard
{



    public ConsumableCard(string name, Sprite artwork, int cost, int stat) : base(name, CardType.Consumable, artwork, cost, stat){
        
    }
}
