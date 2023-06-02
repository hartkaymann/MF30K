using TMPro;
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
    private TextMeshProUGUI textEquipStat;

    void Awake()
    {
        Transform value = transform.parent.transform.Find("Value");
        if (value != null)
        {
            value.TryGetComponent(out textEquipStat);
        }
    }

    void Start()
    {
        
    }

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
        PlayerController pc = PlayerManager.Instance.PlayerController;
        pc.Equip(slot, card);
        isEquipped = true;

        textEquipStat.text = card.bonus.ToString();

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
        PlayerController pc = PlayerManager.Instance.PlayerController;
        if (pc == null)
            return;

        // Unequip item from player if no children
        if (isEquipped && transform.childCount == 0)
        {
            isEquipped = false;

            pc.Uneqip(slot);

            textEquipStat.text = "0";
        }
    }
}
