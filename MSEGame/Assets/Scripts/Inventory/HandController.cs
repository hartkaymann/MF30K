using System.Collections.Generic;
using UnityEngine;

public class HandController : Inventory
{
    private List<Card> items;
    private int noItems;


    private void Start()
    {
        noItems = 0;
        items = new List<Card>();
    }

    void Update()
    {
        // If items changed
        if (noItems != transform.childCount)
        {
            // Clear and refill items
            items.Clear();
            foreach(Transform child in transform)
            {
                if(child.TryGetComponent<CardController>(out var card))
                {
                    items.Add(card.getCard());
                }
            }

            noItems = transform.childCount;
        }
    }
}
