using UnityEngine;

public abstract class TreasureCard : Card
{
    public TreasureCard(string name, CardType type, Sprite artwork, int cost, int stat) : base(name, type, artwork, cost, stat)
    {
    }
}
