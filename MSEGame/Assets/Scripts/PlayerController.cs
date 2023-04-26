using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player player;

    [SerializeField] public Dictionary<EquipmentSlot, EquipmentCard> equipment;


    void Start()
    {
        player = new Player(0, "Kay");

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

    public bool Fight(Monster mob)
    {
        return this.player.CombatLevel > mob.GetCombatLevel();
    }

    public Player GetPlayer() { return this.player; }
    public void setPlayer(Player player) { this.player = player; }
}
