using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Race
{
    Human,
    Elf,
    Dwarf,
    Orc
}

public class RaceCard : Card
{
    private string ability;

    private Race race;

    public RaceCard(Race race, Sprite artwork, int cost, int stat) : base(race.ToString(), CardTypes.Race, artwork, cost, stat)
    {
        this.race = race;
    }
}
