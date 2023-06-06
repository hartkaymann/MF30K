using System.Collections.Generic;
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
            img.color = run.Level == 10 ? GameColor.Green : GameColor.Red;
        }

        FillCombatData(run.Combats);
    }

    public void ToggleDetails()
    {
        if (details == null)
            return;

        details.SetActive(!details.activeInHierarchy);

        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent as RectTransform);
    }

    public void FillCombatData(List<Combat> combats)
    {
        for (int i = 0; i < combats.Count; i++)
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
