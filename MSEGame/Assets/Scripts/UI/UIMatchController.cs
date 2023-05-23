using UnityEngine;
using UnityEngine.UI;

public class UIMatchController : MonoBehaviour
{

    [SerializeField] private GameObject details;

    [SerializeField] private GameObject combatPrefab;
    [SerializeField] private Transform combatParent;

    private void Start()
    {
        if (transform.Find("Overview").TryGetComponent<Image>(out var img))
        {
            img.color = Random.Range(0, 3) > 0 ? GameColor.Green : GameColor.Red;
        }

        // Needs to be replaced by network manager call later
        Combat[] combats = new Combat[Random.Range(5, 30)];
        for (int i = 0; i < combats.Length; i++)
        {
            combats[i] = new() { MonsterLevel = Random.Range(i + 1, i + 10), PlayerLevel = Random.Range(i + 1, i + 10), Victory = Random.Range(0, 2) != 0 };
        };

        FillCombatData(combats);
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
