using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IDropHandler
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

    private int roundBonus;
    public int RoundBonus
    {
        get
        {
            return roundBonus;
        }
        set
        {
            if (roundBonus != value)
            {
                roundBonus = value;
                CalculateCombatLevel();
            }
        }
    }

    [SerializeField] PlayerRenderer playerRenderer;

    [SerializeField] private Dictionary<EquipmentSlot, EquipmentCard> equipment;

    void Start()
    {
        equipment = new Dictionary<EquipmentSlot, EquipmentCard>();
        roundBonus = 0;
    }

    public void Equip(EquipmentSlot slot, EquipmentCard card)
    {
        equipment[slot] = card;

        CalculateCombatLevel();

        NetworkManager.Instance.PutEquipment(player, equipment);
    }

    public void Uneqip(EquipmentSlot slot)
    {
        equipment[slot] = null;

        CalculateCombatLevel();

        NetworkManager.Instance.PutEquipment(player, equipment);
    }

    private void CalculateCombatLevel()
    {
        int newCombatLevel = 0;
        foreach (var card in equipment.Values)
        {
            if (card == null)
                return;

            newCombatLevel += card.bonus;
        }
        Player.CombatLevel = newCombatLevel + roundBonus;
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        if (GameManager.Instance.Stage != GameStage.CombatPreparations)
            return;

        GameObject draggedObj = eventData.pointerDrag;
        if (draggedObj.TryGetComponent<CardController>(out var cc))
        {
            if (cc.Card is ConsumableCard consumable)
            {
                // Add consumable bonus to round bonus
                RoundBonus += consumable.bonus;

                Destroy(draggedObj);
            }
        }
    }
}
