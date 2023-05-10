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

public class EquipmentController : Inventory
{
    [SerializeField] EquipmentSlot slot;
    [SerializeField] EquipmentType type;

    Boolean isEquipped = false;

    public override void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Equipment OnDrop");

        // Check if slot is empty
        if (isEquipped)
            return;

        GameObject dropped = eventData.pointerDrag;
        Draggable draggable = dropped.GetComponent<Draggable>();

        // Check if card is equipment card
        Card card = dropped.GetComponent<CardController>().Card;
        if (card.type != CardType.Equipment)
            return;

        // Check if card types match
        EquipmentCard equipmentCard = card as EquipmentCard;
        if (equipmentCard.equipType != type)
            return;

        // Attach to self
        draggable.parentAfterDrag = transform;
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
                playerController.Equip(slot, cc.Card as EquipmentCard);
            }

        }
        else if (isEquipped && transform.childCount == 0)
        {
            isEquipped = false;

            playerController.Uneqip(slot);
        }
    }
}
