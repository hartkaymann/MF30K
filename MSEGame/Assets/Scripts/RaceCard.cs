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
    private int id;
    private string ability;

    private Race race;

    public RaceCard(Race race) : base(race.ToString())
    {
        this.race = race;
    }
}
