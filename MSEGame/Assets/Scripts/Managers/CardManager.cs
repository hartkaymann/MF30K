using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject canvas;

    // Generating Card info, will be replaced by backend later
    public string cardTitle;
    public string cardType;
    public Sprite cardArtwork;
    public int cardCost;
    public int cardStat;
    public int amount;

    void Start()
    {
        Card card = new Card(cardTitle, cardType, cardArtwork, cardCost, cardStat);

        int baseX = amount * -5;
        for(int i = 0; i < amount; i++)
        {
            Vector2 pos = new Vector3(baseX + i * 10, -50, 0);
            InstantiateCard(card, pos);

        }
    }

    void Update()
    {

    }

    void InstantiateCard(Card card, Vector3 pos)
    {
        GameObject go = Instantiate(cardPrefab, pos, Quaternion.identity);
        go.transform.SetParent(canvas.transform, false);
        CardRenderer cd = go.GetComponent<CardRenderer>();
        if (cd != null)
        {
            cd.setCard(card);
        }

        Debug.Log("Instantiating Card" + card.name);
    }
}
