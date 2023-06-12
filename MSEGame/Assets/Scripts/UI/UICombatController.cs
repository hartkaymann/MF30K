using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICombatController : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI textPlayer;
    [SerializeField] private TextMeshProUGUI textMonster;

    [SerializeField] private Slider slider;

    void Start()
    {
        if (!TryGetComponent(out slider))
        {
            Debug.LogWarning("Couldn't get slider component of combat bar.");
        }
    }

    public void Render(Combat combat)
    {
        background.color = combat.Win ? GameColor.Green : GameColor.Red;
        fill.color = combat.Win ? GameColor.Green : GameColor.Red;

        textPlayer.text = combat.CombatLvlPlayer.ToString();
        textMonster.text = combat.CombatLvlMonster.ToString();

        if (slider == null)
            return;

        Debug.Log($"Combat: Win: {combat.Win}, PLvl: {combat.CombatLvlPlayer}, MLvl:{combat.CombatLvlMonster}, Consequence: {combat.Consequence}");
        slider.maxValue = combat.CombatLvlPlayer + combat.CombatLvlMonster;
        slider.value = combat.CombatLvlPlayer;
    }
}
