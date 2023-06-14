using Newtonsoft.Json;
using UnityEngine;

public class MonsterCard : DoorCard
{
    private int level;
    private int treasures;

    [JsonIgnore]
    public int CombatBuff { get; set; }

    [JsonIgnore]
    public int Level
    {
        get { return level + CombatBuff; }
        private set { level = value; }
    }

    [JsonIgnore]
    public int Treasures
    {
        get { return treasures; }
        set { treasures = value; }
    }

    public MonsterCard(string name, string id, Sprite artwork, int combatLevel, int treasures) : base(name, CardType.Monster, id, artwork)
    {
        this.level = combatLevel;
        this.treasures = treasures;
    }
}
