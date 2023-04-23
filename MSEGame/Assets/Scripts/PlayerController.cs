using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player player;

    [SerializeField] public Dictionary<EquipmentSlot, EquipmentCard> equipment;

    public List<Card> backpack;
    public List<Card> hand;

    void Start()
    {
        player = new Player("Kay", Gender.Male, Race.Human, Profession.Warrior);
        
        equipment = new Dictionary<EquipmentSlot, EquipmentCard>();
        backpack = new List<Card>();
        hand = new List<Card>();

        NetworkManager.instance.PostPlayer(player);
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
        return this.player.combatLevel > mob.GetCombatLevel();
    }
}
