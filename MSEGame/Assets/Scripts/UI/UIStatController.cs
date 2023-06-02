using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIStatController : MonoBehaviour
{

    private static int[] testData = { 4, 2, 5, 1, 2, 5, 6, 3, 8, 13 };
    private int dataMax;

    [SerializeField] private GameObject barPrefab;

    private Transform bars;


    void Awake()
    {
        dataMax = testData.Max();
        bars = transform.Find("Bars"); ;
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

    public void OnEnable()
    {
        for (int i = 0; i < bars.childCount; i++)
        {
            if (i >= testData.Length)
                return;

            Transform barParent = bars.GetChild(i);

            GameObject bar = (barParent.childCount == 0) ?
                Instantiate(barPrefab, barParent.transform) :
                barParent.GetChild(0).gameObject;

            if (bar.TryGetComponent<Slider>(out var slider))
            {
                slider.maxValue = dataMax;
                slider.value = testData[i];
                StartCoroutine(FillBar(slider, testData[i], 1f));
            }
        }
    }
}
