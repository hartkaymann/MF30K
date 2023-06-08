using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : Manager<IntroManager>
{

    [SerializeField] private Transform scroll;
    [SerializeField] private GameObject textWindow;

    void Start()
    {
        textWindow.SetActive(false);

        StartCoroutine(IntroSequence());
    }

    private IEnumerator IntroSequence()
    {
        float duration = 3f;
        float currentTime = 0f;

        float scrollHeightStart = 1.3f;
        float scrollHeightEnd = 5.5f;

        SpriteRenderer scrollRenderer = scroll.GetComponent<SpriteRenderer>();

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float scrollHeightCurrent = Mathf.SmoothStep(scrollHeightStart, scrollHeightEnd, currentTime / duration);
            scrollRenderer.size = new Vector2(9, scrollHeightCurrent);

            yield return null;
        }

        yield return new WaitForSeconds(.5f);
        textWindow.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
