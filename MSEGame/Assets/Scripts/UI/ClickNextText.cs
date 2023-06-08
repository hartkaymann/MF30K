using UnityEngine;

public class ClickNextText : MonoBehaviour
{
    [SerializeField] private Transform textParent;
    public int NoTexts {  get; private set; }

    private int currentIndex;
    public int CurrentIndex
    {
        get
        {
            return currentIndex;
        }
        set
        {
            
            if(value >= NoTexts)
            {
                if(IntroManager.Instance != null)
                {
                IntroManager.Instance.StartGame();

                } else if(OutroManager.Instance != null)
                {
                    OutroManager.Instance.StartSequence();
                }
                return;
            }

            currentIndex = value;
            for (int i = 0; i < NoTexts; i++)
            {
                textParent.GetChild(i).gameObject.SetActive(i == currentIndex);
            }
        }
    }

    private void Start()
    {
        NoTexts = textParent.childCount;

        CurrentIndex = 0;
    }

    public void NextText()
    {
        CurrentIndex++;
    }
}
