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
    private int id;
    private Profession profession;
    private string ability;

    public ProfessionCard(Profession profession) : base(profession.ToString())
    {
        this.profession = profession;
    }
}
