using UnityEngine;

public class ClickNextText : MonoBehaviour
{
    [SerializeField] private Transform textParent;

    private int currentIndex;
    public int CurrentIndex
    {
        get
        {
            return currentIndex;
        }
        set
        {
            
            if(value >= textParent.childCount)
            {
                IntroManager.Instance.StartGame();
            }

            currentIndex = value;
            for (int i = 0; i < textParent.childCount; i++)
            {
                textParent.GetChild(i).gameObject.SetActive(i == currentIndex);
            }
        }
    }

    private void Start()
    {
        CurrentIndex = 0;
    }

    public void NextText()
    {
        CurrentIndex++;
    }
}
