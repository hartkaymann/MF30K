using UnityEngine;

public class MonsterCard : DoorCard
{
    public int level;
    public int trasures;

    public MonsterCard(string name, Sprite artwork, int combatLevel, int treasures) : base(name, CardType.Monster, artwork)
    {
        this.level = combatLevel;
        this.trasures = treasures;
    }
}
