using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutroManager : Manager<OutroManager>
{

    [SerializeField] private Transform scroll;
    [SerializeField] private GameObject textWindow;

    void Start()
    {
        textWindow.SetActive(true);
    }

    public void StartSequence()
    {
        textWindow.SetActive(false);

        StartCoroutine(OutroSequence());
    }

    private IEnumerator OutroSequence()
    {

        float duration = 3f;
        float currentTime = 0f;

        float scrollHeightStart = 5.5f;
        float scrollHeightEnd = 1.3f;

        SpriteRenderer scrollRenderer = scroll.GetComponent<SpriteRenderer>();

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float scrollHeightCurrent = Mathf.SmoothStep(scrollHeightStart, scrollHeightEnd, currentTime / duration);
            scrollRenderer.size = new Vector2(9, scrollHeightCurrent);

            yield return null;
        }

        yield return new WaitForSeconds(.5f);

        EndGame();
    }

    public void EndGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
