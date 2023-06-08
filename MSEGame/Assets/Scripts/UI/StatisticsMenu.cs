using UnityEngine;

public class StatisticsMenu : MonoBehaviour
{

    [SerializeField] private GameObject totalLevelBars;
    [SerializeField] private Transform runsParent;
    [SerializeField] private GameObject matchPanelPrefab;

    private void Start()
    {
        FillMatchHistory();
    }

    private async void FillMatchHistory()
    {
        User user = await NetworkManager.Instance.GetUserStats();
        if (user == null)
            return;

        int[] levelCount = new int[10];
        for (int i = 0; i < user.Runs.Count; i++)
        {
            levelCount[user.Runs[i].Level - 1] += 1;
            Instantiate(matchPanelPrefab, runsParent).GetComponent<UIRunController>().SetData(user.Runs[i]);
        }

        if (totalLevelBars.TryGetComponent<UIStatController>(out var statCtrl))
        {
            statCtrl.FillLevelBars(levelCount);
        }
    }
}
