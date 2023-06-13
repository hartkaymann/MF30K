using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIStatController : MonoBehaviour
{
    [SerializeField] private GameObject barPrefab;

    private Transform bars;

    private void Awake()
    {
        bars = transform.Find("Bars");    
    }

    public void FillLevelBars(int[] levelCount)
    {
        int levelCountMax = levelCount.Max();
        for (int i = 0; i < bars.childCount; i++)
        {
            if (i >= levelCount.Length)
                return;

            Transform barParent = bars.GetChild(i);

            GameObject bar = (barParent.childCount == 0) ?
                Instantiate(barPrefab, barParent.transform) :
                barParent.GetChild(0).gameObject;

            if (bar.TryGetComponent<Slider>(out var slider))
            {
                slider.maxValue = levelCountMax;
                slider.value = levelCount[i];
                StartCoroutine(FillBar(slider, levelCount[i], 1f));
            }
        }
    }

    private IEnumerator FillBar(Slider slider, int value, float time)
    {
        float currTime = 0f;
        while (currTime <= time)
        {
            currTime += Time.deltaTime;
            slider.value = Mathf.SmoothStep(0, value, currTime / time);
            yield return null;
        }
    }
}
