using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private PlayerRenderer playerRenderer;

    private Player player;
    private GameObject description;

    private Animator animator;
    private int isRunningHash;
    private int isAttackingHash;

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

                player.OnProfessionChanged += OnPlayerProfessionChanged;
                player.OnRaceChanged += OnPlayerRaceChanged;
                player.OnPropertyChanged += OnPlayerPropertyChanged;

                OnPlayerProfessionChanged();
                OnPlayerRaceChanged();
                OnPlayerPropertyChanged();
            }
        }
    }

    void Awake()
    {
        description = GameObject.Find("PlayerDescription");
        animator = GetComponent<Animator>();
        isRunningHash = Animator.StringToHash("isRunning");
        isAttackingHash = Animator.StringToHash("Attack");
    }

    void Start()
    {
        description.SetActive(false);
    }

    private void OnPlayerPropertyChanged()
    {
        if (description == null)
            return;

        if (description.transform.Find("Name").TryGetComponent<TextMeshProUGUI>(out var textDescName))
        {
            textDescName.text = $"Name: {Player.Name}";
        }

        if (description.transform.Find("Level").TryGetComponent<TextMeshProUGUI>(out var textDescCooldown))
        {
            textDescCooldown.text = $"Level: {Player.Level}";
        }

        if (description.transform.Find("CombatLevel").TryGetComponent<TextMeshProUGUI>(out var textDescCombat))
        {
            int buff = Player.RaceEffect + Player.RoundBonus;
            string buffTxt = $"({buff})";
            textDescCombat.text = $"Combat:: {Player.CombatLevel} {(buff > 0 ? buffTxt : "")}";
        }

        if (description.transform.Find("Race").TryGetComponent<TextMeshProUGUI>(out var textDescRace))
        {
            textDescRace.text = $"Race: {Player.Race}";
        }

        if (description.transform.Find("Profession").TryGetComponent<TextMeshProUGUI>(out var textDescRrofession))
        {
            textDescRrofession.text = $"Profession: {Player.Profession}";
        }
    }

    private void OnPlayerProfessionChanged()
    {
        Debug.Log("Handing On Player Profession changed");

        if (TryGetComponent<ProfessionController>(out var oldProfCtrl))
            Destroy(oldProfCtrl);

        switch (player.Profession)
        {
            case Profession.Knight:
                gameObject.AddComponent<KnightController>();
                break;
            case Profession.Wizard:
                gameObject.AddComponent<WizardController>();
                break;
            case Profession.Rogue:
                gameObject.AddComponent<RogueController>();
                break;
            default:
                break;
        }
    }

    private void OnPlayerRaceChanged()
    {
        Debug.Log("Handing On Player Race changed");
        if (TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            spriteRenderer.sprite = SpriteManager.Instance.GetSprite(Player.Race.ToString());
        }

        if (TryGetComponent<Animator>(out var animator))
        {
            animator.runtimeAnimatorController = AnimationManager.Instance.GetAnimator(Player.Race.ToString());
        }
    }

    public void EquipToSlot(EquipmentSlot slot, CardController cardController)
    {
        GameObject equipmentGo = GameObject.Find("Equipment");
        if (equipmentGo == null)
        {
            Debug.LogWarning($"Could not find Equipment GameObject");
            return;
        }

        GameObject slotGo = equipmentGo.transform.Find($"Slots/{slot}/Slot").gameObject;
        if (slotGo == null)
        {
            Debug.LogWarning($"Could not find Slots/{slot} GameObject");
            return;
        }


        if (slotGo.TryGetComponent<EquipmentController>(out var equipmentController))
        {
            equipmentController.EquipItem(cardController);
            cardController.transform.SetParent(slotGo.transform);
        }
        else
        {
            Debug.LogWarning($"Could not find EquipmentController Component");
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (GameManager.Instance.Stage != GameStage.CombatPreparation)
            return;

        GameObject draggedObj = eventData.pointerDrag;
        if (draggedObj.TryGetComponent<CardController>(out var cc))
        {
            if (cc.Card is ConsumableCard consumable)
            {
                // Add consumable bonus to round bonus
                Player.RoundBonus += consumable.bonus;

                Destroy(draggedObj);
            }
        }
    }

    public void Equip(EquipmentSlot slot, EquipmentCard card)
    {
        Player.Equipment[slot] = card;
        Player.CalculateCombatLevel();
        StartCoroutine(NetworkManager.Instance.PutEquipment(Player, Player.Equipment));
    }

    public void Uneqip(EquipmentSlot slot)
    {
        Player.Equipment[slot] = null;
        Player.CalculateCombatLevel();
        StartCoroutine(NetworkManager.Instance.PutEquipment(Player, Player.Equipment));
    }

    public void EquipStarterGear()
    {
        EquipmentCard starterWeaponR = new("Wooden Shield", EquipmentType.Weapon, null, SpriteManager.Instance.GetStarterSprite(EquipmentSlot.WeaponR), 0, 1);
        EquipmentCard starterWeaponL = new("Rusty Sword", EquipmentType.Weapon, null, SpriteManager.Instance.GetStarterSprite(EquipmentSlot.WeaponL), 0, 1);
        EquipmentCard starterHelmet = new("Rusty Helmet", EquipmentType.Helmet, null, SpriteManager.Instance.GetStarterSprite(EquipmentSlot.Helmet), 0, 1);
        EquipmentCard starterArmor = new("Rusty Armor", EquipmentType.Armor, null, SpriteManager.Instance.GetStarterSprite(EquipmentSlot.Armor), 0, 1);
        EquipmentCard starterBoots = new("Rusty Boots", EquipmentType.Boots, null, SpriteManager.Instance.GetStarterSprite(EquipmentSlot.Boots), 0, 1);

        CardController ccWeaponR = CardManager.Instance.InstantiateCard(starterWeaponR);
        CardController ccWeaponL = CardManager.Instance.InstantiateCard(starterWeaponL);
        CardController ccHelmet = CardManager.Instance.InstantiateCard(starterHelmet);
        CardController ccArmor = CardManager.Instance.InstantiateCard(starterArmor);
        CardController ccBoots = CardManager.Instance.InstantiateCard(starterBoots);

        EquipToSlot(EquipmentSlot.WeaponR, ccWeaponR);
        EquipToSlot(EquipmentSlot.WeaponL, ccWeaponL);
        EquipToSlot(EquipmentSlot.Helmet, ccHelmet);
        EquipToSlot(EquipmentSlot.Boots, ccBoots);
        EquipToSlot(EquipmentSlot.Armor, ccArmor);
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

    [ContextMenu("Player Attack")]
    public void Attack()
    {
        animator.SetTrigger(isAttackingHash);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        description.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        description.SetActive(false);
    }
}
