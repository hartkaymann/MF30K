using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject canvas;

    void Awake()
    {
        instance = this;
    }
    public void InstantiateCard(Card card)
    {
        GameObject go = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity);
        go.transform.SetParent(canvas.transform, false);
        CardController cd = go.GetComponent<CardController>();
        if (cd != null)
        {
            cd.setCard(card);
        }
    }
}
