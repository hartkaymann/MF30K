using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IDropHandler
{
    [SerializeField] private PlayerRenderer playerRenderer;

    private Player player;
    private int roundBonus = 0;

    private Animator animator;
    private int isRunningHash;

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

    [SerializeField] private Dictionary<EquipmentSlot, EquipmentCard> equipment;

    void Start()
    {
        animator = GetComponent<Animator>();
        isRunningHash = Animator.StringToHash("isRunning");

        equipment = new Dictionary<EquipmentSlot, EquipmentCard>();

        EquipmentCard starterWeaponR = new("Wooden Shield", EquipmentType.Weapon, "0", SpriteManager.Instance.GetStarterSprite(EquipmentSlot.WeaponR), 0, 1);
        EquipmentCard starterWeaponL = new("Rusty Sword", EquipmentType.Weapon, "0", SpriteManager.Instance.GetStarterSprite(EquipmentSlot.WeaponL), 0, 1);
        EquipmentCard starterHelmet = new("Rusty Helmet", EquipmentType.Helmet, "0", SpriteManager.Instance.GetStarterSprite(EquipmentSlot.Helmet), 0, 1);
        EquipmentCard starterArmor = new("Rusty Armor", EquipmentType.Armor, "0", SpriteManager.Instance.GetStarterSprite(EquipmentSlot.Armor), 0, 1);
        EquipmentCard starterBoots = new("Rusty Boots", EquipmentType.Boots, "0", SpriteManager.Instance.GetStarterSprite(EquipmentSlot.Boots), 0, 1);

        CardController ccWeaponR = CardManager.instance.InstantiateCard(starterWeaponR);
        CardController ccWeaponL = CardManager.instance.InstantiateCard(starterWeaponL);
        CardController ccHelmet = CardManager.instance.InstantiateCard(starterHelmet);
        CardController ccArmor = CardManager.instance.InstantiateCard(starterArmor);
        CardController ccBoots = CardManager.instance.InstantiateCard(starterBoots);

        EquipToSlot(EquipmentSlot.WeaponR, ccWeaponR);
        EquipToSlot(EquipmentSlot.WeaponL, ccWeaponL);
        EquipToSlot(EquipmentSlot.Helmet, ccHelmet);
        EquipToSlot(EquipmentSlot.Boots, ccBoots);
        EquipToSlot(EquipmentSlot.Armor, ccArmor);
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

        //Debug.Log($"Attaching to Slots/{slot}");
        GameObject slotGo = equipmentGo.transform.Find($"Slots/{slot}").gameObject;
        if (slotGo == null)
            return;

        if (slotGo.TryGetComponent<EquipmentController>(out var equipmentController))
        {
            equipmentController.EquipItem(cardController);
            cardController.transform.SetParent(slotGo.transform);
        }
    }

    private void CalculateCombatLevel()
    {
        int newCombatLevel = 0;
        foreach (var card in equipment.Values)
        {
            if (card == null)
                continue;

            newCombatLevel += card.bonus;
        }
        Player.CombatLevel = newCombatLevel + roundBonus;
    }

    public void OnDrop(PointerEventData eventData)
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

    public void RunForDuration(float duration)
    {
        StartRunning();
        Invoke(nameof(StopRunning), duration);
    }

    [ContextMenu("Player Run")] 
    public void StartRunning()
    {
        animator.SetBool(isRunningHash, true);
    }

    [ContextMenu("Player Stop")]
    public void StopRunning()
    {
        animator.SetBool(isRunningHash, false);
    }
}
