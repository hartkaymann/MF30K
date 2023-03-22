using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gender
{
    Male,
    Female
}

public class Player
{
    private int level = 0;
    private int combatLevel = 0;

    private Gender gender;
    private RaceCard race;
    private ProfessionCard profession;

    private EquipmentCard weaponL;
    private EquipmentCard weaponR;
    private EquipmentCard helmet;
    private EquipmentCard armor;
    private EquipmentCard boots;

    private List<Card> backpack;
    private List<Card> hand;

    public Player(Gender gender, Race race, Profession profession) { 
        backpack = new List<Card>();
        hand = new List<Card>();

        this.gender = gender;
        this.race = new RaceCard(race);
        this.profession = new ProfessionCard(profession);
    }

    public bool equip(Card card) {
        return true;
    }

    public bool fight(Monster mob)
    {
        return this.combatLevel > mob.GetCombatLevel();
    }
}
