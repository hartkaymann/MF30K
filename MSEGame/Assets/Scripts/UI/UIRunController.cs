using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRunController : MonoBehaviour
{
    [SerializeField] private GameObject details;

    [SerializeField] private Sprite imgHuman;
    [SerializeField] private Sprite imgElf;
    [SerializeField] private Sprite imgOrc;

    [SerializeField] private GameObject combatPrefab;
    [SerializeField] private Transform combatParent;

    public void SetData(Run run)
    {
        Transform overview = transform.Find("Overview");
        if (overview.TryGetComponent<Image>(out var img))
        {
            img.color = run.PlayerLevel == 10 ? GameColor.Green : GameColor.Red;
        }

        if (overview.Find("TextName").TryGetComponent<TextMeshProUGUI>(out var textName))
            textName.text = SessionData.Username;

        if (overview.Find("TextLevel").TryGetComponent<TextMeshProUGUI>(out var textLevel))
            textLevel.text = run.PlayerLevel.ToString();

        if (overview.Find("TextCombatLevel").TryGetComponent<TextMeshProUGUI>(out var textCombatlvl))
            textCombatlvl.text = run.CombatLevel.ToString();

        if (overview.Find("TextProfession").TryGetComponent<TextMeshProUGUI>(out var textProfession))
            textProfession.text = run.Profession;

        if (overview.Find("TextRace").TryGetComponent<TextMeshProUGUI>(out var textRace))
            textRace.text = run.Race;

        if (overview.Find("TextGold").TryGetComponent<TextMeshProUGUI>(out var textGold))
            textGold.text = run.Goldsold.ToString();

        if (overview.Find("Image").TryGetComponent<Image>(out var playerImage))
            playerImage.sprite = run.Race switch
            {
                "Human" => imgHuman,
                "Elf" => imgElf,
                "Orc" => imgOrc,
                _ => imgHuman,
            };

        FillCombatData(run.Combats);
    }

    public void ToggleDetails()
    {
        if (details == null)
            return;

        details.SetActive(!details.activeInHierarchy);

        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent as RectTransform);
    }

    public void FillCombatData(Combat[] combats)
    {
        for (int i = 0; i < combats.Length; i++)
        {
            Combat combat = combats[i];

            GameObject obj = Instantiate(combatPrefab, combatParent);

            if (obj.TryGetComponent<UICombatController>(out var combatCtrl))
            {
                combatCtrl.Render(combat);
            }
        }
    }
}
