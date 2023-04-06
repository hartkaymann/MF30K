using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;

    private Stack<Card> deck = new Stack<Card>();
    public int deckSize = 10;

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

    private void Start()
    {
        for (int i = 0; i < deckSize; i++)
        {
            Card card = null;
            switch ((int)Random.Range(1, 5))
            {
                case 1:
                    card = new ConsumableCard("Consumable", cardArtwork, 1, 1);
                    break;
                case 2:
                    EquipmentType type = (EquipmentType)Random.Range(0, 3);
                    card = new EquipmentCard("Equipment", type, cardArtwork, 1, 1);
                    break;
                case 3:
                    card = new ItemCard("Item", cardArtwork, 2, 2);
                    break;
                case 4:
                    Profession profession = (Profession)Random.Range(0, 3);
                    card = new ProfessionCard(profession, cardArtwork, 1, 1);
                    break;
                case 5:
                    Race race = (Race)Random.Range(0, 3);
                    card = new RaceCard(race, cardArtwork, 1, 1);
                    break;
            }
            deck.Push(card);
        }
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
        int baseX = -50 * amount;
        for (int i = 0; i < amount; i++)
        {
            InstantiateCard(deck.Pop(), new Vector3(baseX + i * 100, 0, 0));
        }
    }
}
