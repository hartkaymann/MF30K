using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Profession
{
    Warrior,
    Mage,
    Priest,
    Ranger
}

public class ProfessionCard : Card
{
    private Profession profession;
    private string ability;

    public ProfessionCard(Profession profession, Sprite artwork, int cost, int stat) : base(profession.ToString(), CardTypes.Profession, artwork, cost, stat)
    {
        this.profession = profession;
    }
}
