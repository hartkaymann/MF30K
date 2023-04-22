using UnityEngine;

public class MonsterCard : DoorCard
{
    public MonsterCard(string name, Sprite artwork, int cost, int stat) : base(name, CardType.Monster, artwork, cost, stat)
    {
    }
}
