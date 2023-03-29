using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject canvas;

    // Generating Card info, will be replaced by backend later
    public string cardTitle;
    public string cardType;
    public Sprite cardArtwork;
    public int cardCost;
    public int cardStat;

    void Awake()
    {
        instance = this;
    }

    public void InstantiateCard(Card card, Vector3 pos)
    {
        GameObject go = Instantiate(cardPrefab, pos, Quaternion.identity);
        go.transform.SetParent(canvas.transform, false);
        CardRenderer cd = go.GetComponent<CardRenderer>();
        if (cd != null)
        {
            cd.setCard(card);
        }
    }

    public void DrawCards(int amount)
    {
        Card card = new Card(cardTitle, cardType, cardArtwork, cardCost, cardStat);

        int baseX = -50 * amount;
        for (int i = 0; i < amount; i++)
        {
            InstantiateCard(card, new Vector3(baseX + i * 100, 0, 0));
        }
    }
}
