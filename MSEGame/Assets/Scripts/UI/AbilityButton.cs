using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textCooldown;

    [SerializeField] private GameObject description;
    [SerializeField] private GameObject cooldownMask;

    private ProfessionController profCtrl;
    public ProfessionController ProfessionController
    {
        get { return profCtrl; }
        set
        {
            profCtrl = value;
            ChangeAbility();
        }
    }

    void Awake()
    {
        GameManager.OnGameStageChange += GameManagerOnStageChange;
        GameManager.OnNewCycle += UpdateCooldown;
        button.onClick.AddListener(UpdateCooldown);
    }

    private void ChangeAbility()
    {
        button.enabled = true;
        textName.text = profCtrl.AbilityName;
        textCooldown.text = profCtrl.Cooldown.ToString();

        if (description.transform.Find("Name").TryGetComponent<TextMeshProUGUI>(out var textDescName))
        {
            textDescName.text = $"{profCtrl.AbilityName}";
        }

        if (description.transform.Find("Cooldown").TryGetComponent<TextMeshProUGUI>(out var textDescCooldown))
        {
            textDescCooldown.text = $"Cooldown: {profCtrl.Cooldown} Round{(profCtrl.Cooldown > 1 ? "s" : "")}";
        }

        if (description.transform.Find("Description").TryGetComponent<TextMeshProUGUI>(out var textDescDesc))
        {
            textDescDesc.text = $"{profCtrl.Description}";
        }

        UpdateCooldown();
    }

    private void GameManagerOnStageChange(GameStage stage)
    {
        button.interactable = (stage == GameStage.CombatPreparation);
    }

    private void UpdateCooldown()
    {
        if (this.profCtrl == null)
        {
            return;
        }

        if (PlayerManager.Instance.PlayerController.TryGetComponent<ProfessionController>(out var profCtrl))
        {
            button.enabled = (profCtrl.CooldownRemaining == 0);

            cooldownMask.SetActive(profCtrl.CooldownRemaining != 0);

            if (cooldownMask.transform.Find("Remaining").TryGetComponent<TextMeshProUGUI>(out var textRemaining))
            {
                textRemaining.text = profCtrl.CooldownRemaining.ToString();
            }

        }
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
