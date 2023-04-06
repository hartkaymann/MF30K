using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gender
{
    Male,
    Female
}

public class Player : MonoBehaviour
{
    private int level = 0;
    private int combatLevel = 0;

    private Gender gender;
    private Race race;
    private Profession profession;

    private EquipmentCard weaponL;
    private EquipmentCard weaponR;
    private EquipmentCard helmet;
    private EquipmentCard armor;
    private EquipmentCard boots;

    private List<Card> backpack;
    private List<Card> hand;

    public Player(Gender gender, Race race, Profession profession)
    {
        backpack = new List<Card>();
        hand = new List<Card>();

        this.gender = gender;
        this.race = race;
        this.profession = profession;
    }

    public bool equip(Card card)
    {
        return true;
    }

    public bool fight(Monster mob)
    {
        return this.combatLevel > mob.GetCombatLevel();
    }
}
