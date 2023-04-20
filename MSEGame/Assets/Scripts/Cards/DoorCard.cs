using UnityEngine;

public abstract class DoorCard : Card
{
    public DoorCard(string name, CardType type, Sprite artwork, int cost, int stat) : base(name, type, artwork, cost, stat)
    {
    }
}
