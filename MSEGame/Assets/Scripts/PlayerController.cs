using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player player;
    public Player Player { get { return player; } set { player = value; } }

    [SerializeField] private Dictionary<EquipmentSlot, EquipmentCard> equipment;

    void Start()
    {
        // Will later move to character creation
        player = new Player("Kay")
        {
            Gender = Gender.Male,
            Race = Race.Human,
            Profession = Profession.Rogue,
            Level = 1,
            CombatLevel = 5
        };

        equipment = new Dictionary<EquipmentSlot, EquipmentCard>();

        NetworkManager.instance.PostPlayer(player);
    }

    public void Equip(EquipmentSlot slot, EquipmentCard card)
    {
        Debug.Log($"Equip {card.title} to {slot}");
        equipment[slot] = card;

        NetworkManager.instance.PutEquipment(player, equipment);
    }

    public void Uneqip(EquipmentSlot slot)
    {
        Debug.Log($"Unequip {slot}");
        equipment[slot] = null;

        NetworkManager.instance.PutEquipment(player, equipment);
    }

    public bool Fight(MonsterCard mob)
    {
        return this.player.CombatLevel > mob.level;
    }
}
