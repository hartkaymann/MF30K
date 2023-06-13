using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRunController : MonoBehaviour
{
    [SerializeField] private GameObject details;

    [SerializeField] private GameObject combatPrefab;
    [SerializeField] private Transform combatParent;

    public void SetData(Run run)
    {
        if (transform.Find("Overview").TryGetComponent<Image>(out var img))
        {
            img.color = run.PlayerLevel == 10 ? GameColor.Green : GameColor.Red;
        }

        if (transform.Find("TextName").TryGetComponent<TextMeshProUGUI>(out var textName))
            textName.text = SessionData.Username;

        if (transform.Find("TextLevel").TryGetComponent<TextMeshProUGUI>(out var textLevel))
            textLevel.text = run.PlayerLevel.ToString();

        if (transform.Find("TextCombatLevel").TryGetComponent<TextMeshProUGUI>(out var textCombatlvl))
            textCombatlvl.text = textCombatlvl.ToString();

        if (transform.Find("TextProfession").TryGetComponent<TextMeshProUGUI>(out var textProfession))
            textProfession.text = run.Profession;

        if (transform.Find("TextRace").TryGetComponent<TextMeshProUGUI>(out var textRace))
            textRace.text = run.Race;

        if (transform.Find("TextGold").TryGetComponent<TextMeshProUGUI>(out var textGold))
            textGold.text = run.Goldsold.ToString();

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
