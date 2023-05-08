using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player player;
    public Player Player
    {
        get
        {
            return player;
        }
        set
        {
            if (player != value)
            {
                player = value;
                playerRenderer.Render();
            }
        }
    }

    [SerializeField] private Dictionary<EquipmentSlot, EquipmentCard> equipment;

    [SerializeField] PlayerRenderer playerRenderer;

    void Start()
    {
        equipment = new Dictionary<EquipmentSlot, EquipmentCard>();
    }

    public void Equip(EquipmentSlot slot, EquipmentCard card)
    {
        Debug.Log($"Equip {card.title} to {slot}");
        equipment[slot] = card;

        NetworkManager.Instance.PutEquipment(player, equipment);
    }

    public void Uneqip(EquipmentSlot slot)
    {
        Debug.Log($"Unequip {slot}");
        equipment[slot] = null;

        NetworkManager.Instance.PutEquipment(player, equipment);
    }

    public bool Fight(MonsterCard mob)
    {
        return this.player.CombatLevel > mob.level;
    }
}
