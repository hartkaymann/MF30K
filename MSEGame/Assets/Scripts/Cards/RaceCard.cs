using UnityEngine;

public enum Race
{
    Orc,
    Elf,
    Dwarf,
    Human
}

public class RaceCard : DoorCard
{
    private string ability;

    private readonly Race race;

    public RaceCard(Race race, string id, Sprite artwork) : base(race.ToString(), CardType.Race, id, artwork)
    {
        this.race = race;
    }
}
