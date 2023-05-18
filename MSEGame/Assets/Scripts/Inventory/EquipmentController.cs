using System;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EquipmentSlot
{
    Helmet,
    Armor,
    Boots,
    WeaponL,
    WeaponR
}

public class EquipmentController : MonoBehaviour, IDropHandler
{
    [SerializeField] private EquipmentSlot slot;
    [SerializeField] private EquipmentType type;

    bool isEquipped = false;

    public void OnDrop(PointerEventData eventData)
    {
        // Check if slot is empty
        if (isEquipped)
            return;

        GameObject dropped = eventData.pointerDrag;
        Draggable draggable = dropped.GetComponent<Draggable>();

        // Check if card is equipment card
        Card card = dropped.GetComponent<CardController>().Card;
        if (card.type != CardType.Equipment)
            return;

        EquipmentCard equipmentCard = card as EquipmentCard;
        if (!CanEquip(equipmentCard))
            return;

        Equip(equipmentCard);

        // Attach to self
        draggable.parentAfterDrag = transform;
    }

    public bool CanEquip(EquipmentCard card)
    {
        return card.equipType == type;
    }

    private void Equip(EquipmentCard card)
    {

        PlayerController playerController = PlayerManager.Instance.CurrentPlayer;
        playerController.Equip(slot, card);
        isEquipped = true;

        //Debug.Log($"Equipped {card.title} to slot {slot}.");
    }

    public void EquipItem(CardController cardController)
    {
        Card card = cardController.Card;
        if (card.type != CardType.Equipment)
            return;

        EquipmentCard equipmentCard = card as EquipmentCard;
        if (!CanEquip(equipmentCard))
            return;

        Equip(equipmentCard);

        isEquipped = true;
    }

    void Update()
    {
        PlayerController playerController = PlayerManager.Instance.CurrentPlayer;
        if (playerController == null)
            return;

        // Unequip item from player if no children
        if (isEquipped && transform.childCount == 0)
        {
            isEquipped = false;

            playerController.Uneqip(slot);
        }
    }
}
