using UnityEngine;

public enum Race
{
    Human,
    Elf,
    Dwarf,
    Orc
}

public class RaceCard : DoorCard
{
    private string ability;

    private Race race;

    public RaceCard(Race race, Sprite artwork, int cost, int stat) : base(race.ToString(), CardType.Race, artwork, cost, stat)
    {
        this.race = race;
    }
}
