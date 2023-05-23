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
        background.color = combat.Victory ? GameColor.Green : GameColor.Red;
        fill.color = combat.Victory ? GameColor.Green : GameColor.Red;

        textPlayer.text = combat.PlayerLevel.ToString();
        textMonster.text = combat.MonsterLevel.ToString();

        if (slider == null)
            return;

        slider.maxValue = combat.PlayerLevel + combat.MonsterLevel;
        slider.value = combat.PlayerLevel;
    }
}
