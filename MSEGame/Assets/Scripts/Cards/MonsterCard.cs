using UnityEngine;

public class MonsterCard : DoorCard
{
    public int level;
    public int treasures;

    public MonsterCard(string name, string id, Sprite artwork, int combatLevel, int treasures) : base(name, CardType.Monster, id, artwork)
    {
        this.level = combatLevel;
        this.treasures = treasures;
    }
}
