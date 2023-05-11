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

    private int roundBonus = 0;
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

        EquipmentCard starterHelmet = new("Leather Headgear", EquipmentType.Helmet, "0", SpriteManager.Instance.GetDummySprite(), 0, 1);
        EquipmentCard starterArmor = new("Rusty Armor", EquipmentType.Armor, "0", SpriteManager.Instance.GetDummySprite(), 0, 1);
        EquipmentCard starterBoots = new("Leather Boots", EquipmentType.Boots, "0", SpriteManager.Instance.GetDummySprite(), 0, 1);
        EquipmentCard starterWeaponL = new("Rusty Sword", EquipmentType.Weapon, "0", SpriteManager.Instance.GetDummySprite(), 0, 1);
        EquipmentCard starterWeaponR = new("Wooden Shield", EquipmentType.Weapon, "0", SpriteManager.Instance.GetDummySprite(), 0, 1);

        CardController ccHelmet = CardManager.instance.InstantiateCard(starterHelmet);
        CardController ccArmor = CardManager.instance.InstantiateCard(starterArmor);
        CardController ccBoots = CardManager.instance.InstantiateCard(starterBoots);
        CardController ccWeaponL = CardManager.instance.InstantiateCard(starterWeaponL);
        CardController ccWeaponR = CardManager.instance.InstantiateCard(starterWeaponR);

        EquipToSlot(EquipmentSlot.Helmet, ccHelmet);
        EquipToSlot(EquipmentSlot.Armor, ccArmor);
        EquipToSlot(EquipmentSlot.Boots, ccBoots);
        EquipToSlot(EquipmentSlot.WeaponR, ccWeaponL);
        EquipToSlot(EquipmentSlot.WeaponL, ccWeaponR);
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

    public void EquipToSlot(EquipmentSlot slot, CardController cardController)
    {
        GameObject equipmentGo = GameObject.Find("Equipment");
        if (equipmentGo == null)
            return;

        Debug.Log($"Attaching to Slots/{slot}");
        GameObject slotGo = equipmentGo.transform.Find($"Slots/{slot}").gameObject;
        if (slotGo == null)
            return;

        if (slotGo.TryGetComponent<EquipmentController>(out var equipmentController))
        {
            equipmentController.EquipItem(cardController);
            cardController.transform.parent = slotGo.transform;
        }
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
