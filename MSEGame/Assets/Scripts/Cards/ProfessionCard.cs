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

public class ProfessionCard
{
    private int id;
    private Profession profession;
    private string ability;

    public ProfessionCard(Profession profession)
    {
        this.profession = profession;
    }
}
