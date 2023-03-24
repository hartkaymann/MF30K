using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    // State management
    public static GameManager instance;
    public GameState state;
    public static event Action<GameState> OnGameStateChange;

    // Cards
    public List<Card> deck = new List<Card>();
    public Transform[] cardSlots;
    public bool[] availableCardSlots;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateGameStates(GameState.InventoryManagement);
    }

    // Update is called once per frame
    void UpdateGameStates(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.InventoryManagement:
                break;
            case GameState.DrawCard:
                break;
            case GameState.Combat:
                break;
            case GameState.Victory:
                break;
            case GameState.Defeat:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChange?.Invoke(newState);
    }

    public void DrawCard()
    {
        if(deck.Count >= 1)
        {
            Card randomCard = deck[Random.Range(0, deck.Count)];
        }
    }
}

public enum GameState
{
    InventoryManagement,
    DrawCard,
    Combat,
    Victory,
    Defeat
}