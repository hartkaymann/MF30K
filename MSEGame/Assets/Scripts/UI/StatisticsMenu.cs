using System;
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
        UserData user = await NetworkManager.Instance.GetUserStats();
        if (user == null)
            return;

        int[] levelCount = new int[11];
        for (int i = 0; i < user.Runs.Length; i++)
        {
            int clampedLevel = (int)Math.Clamp(user.Runs[i].PlayerLevel, 0, 10);

            levelCount[clampedLevel] += 1;
            Instantiate(matchPanelPrefab, runsParent).GetComponent<UIRunController>().SetData(user.Runs[i]);
        }

        if (totalLevelBars.TryGetComponent<UIStatController>(out var statCtrl))
        {
            statCtrl.FillLevelBars(levelCount);
        }
    }
}
