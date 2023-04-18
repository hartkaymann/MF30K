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

    [SerializeField] private Dictionary<EquipmentSlot, EquipmentCard> equipment;

    private List<Card> backpack;
    private List<Card> hand;

    private void Start()
    {
        equipment = new Dictionary<EquipmentSlot, EquipmentCard>();
        backpack = new List<Card>();
        hand = new List<Card>();
    }

    void Create(Gender gender, Race race, Profession profession)
    {
        this.gender = gender;
        this.race = race;
        this.profession = profession;
    }

    public void Equip(EquipmentSlot slot, EquipmentCard card)
    {
        Debug.Log($"Equip {card.title} to {slot}");
        equipment[slot] = card;
    }

    public void Uneqip(EquipmentSlot slot)
    {
        Debug.Log($"Unequip {slot}");
        equipment[slot] = null;
    }

    public bool Fight(Monster mob)
    {
        return this.combatLevel > mob.GetCombatLevel();
    }
}
