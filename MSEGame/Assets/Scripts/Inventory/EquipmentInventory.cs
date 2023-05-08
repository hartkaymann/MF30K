using System;
using System.Collections;
using System.Collections.Generic;
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

public class EquipmentInventory : Inventory
{
    [SerializeField] EquipmentSlot slot;
    [SerializeField] EquipmentType type;

    Boolean isEquipped = false;

    public override void OnDrop(PointerEventData eventData)
    {
        // Check if slot is empty
        if (isEquipped)
            return;

        GameObject dropped = eventData.pointerDrag;
        Draggable draggable = dropped.GetComponent<Draggable>();

        // Check if card is equipment card
        Card card = dropped.GetComponent<CardController>().getCard();
        if (card.type != CardType.Equipment)
            return;

        // Check if card types match
        EquipmentCard equipmentCard = card as EquipmentCard;
        if (equipmentCard.equipType != type)
            return;

        Attach(draggable);
    }

    private void Update()
    {
        PlayerController playerController = PlayerManager.Instance.CurrentPlayer;
        if (playerController == null) 
            return;

        if (!isEquipped && transform.childCount > 0)
        {
            isEquipped = true;

            CardController cc = transform.GetComponentInChildren<CardController>();
            if (cc != null)
            {
                playerController.Equip(slot, cc.getCard() as EquipmentCard);
            }

        }
        else if (isEquipped && transform.childCount == 0)
        {
            isEquipped = false;

            playerController.Uneqip(slot);
        }


    }
}
