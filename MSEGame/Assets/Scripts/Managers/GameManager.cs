using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    // State management
    public static GameManager instance;
    public GameState state;
    public static event Action<GameState> OnGameStateChange;
    public Button nextStageButton;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateGameStates(GameState.InventoryManagement);
        nextStageButton.onClick.AddListener(HandleNextStageButtonClicked);
    }

    public void UpdateGameStates(GameState newState)
    {
        state = newState;
        Debug.Log("New Stage: " + state.ToString());

        switch (newState)
        {
            case GameState.InventoryManagement:
                break;
            case GameState.DrawCard:
                CardManager.instance.DrawCards(1);
                break;
            case GameState.Combat:

                break;
            case GameState.Victory:
                CardManager.instance.DrawCards(3);
                break;
            case GameState.Defeat:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChange?.Invoke(newState);
    }

    void HandleNextStageButtonClicked()
    {
        UpdateGameStates(GameState.DrawCard);
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