using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
        CardRenderer cd = go.GetComponent<CardRenderer>();
        if (cd != null)
        {
            cd.Render(card);
        }
    }
}
