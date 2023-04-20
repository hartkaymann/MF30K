using UnityEngine;

public class ItemCard : TreasureCard
{
    public ItemCard(string name, Sprite artwork, int cost, int stat) : base(name, CardType.Item, artwork, cost, stat)
    {

    }
}
