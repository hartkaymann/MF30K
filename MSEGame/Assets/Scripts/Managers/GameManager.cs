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
    public Button btnNext;
    public Button btnCombat;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateGameStates(GameState.InventoryManagement);
        btnNext.onClick.AddListener(HandleNextStageButtonClicked);
        btnCombat.onClick.AddListener(HandleCombatButtonClicked);
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
                Invoke("DrawCard", 10);
                break;
            case GameState.CombatPreparations:
                break;
            case GameState.Combat:
                Combat();
                break;
            case GameState.Victory:
                Victory();
                CardManager.instance.DrawCards(3);
                break;
            case GameState.Defeat:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChange?.Invoke(newState);
    }

    void DrawCard()
    {
        Debug.Log("DRAW CARD (10sec)!");
        UpdateGameStates(GameState.CombatPreparations);
    }

    void Combat()
    {
        Debug.Log("DOING COMBAT!");

        UpdateGameStates(GameState.Victory);
    }

    private void Victory()
    {
        Debug.Log("VICTORY!");
        UpdateGameStates(GameState.InventoryManagement);
    }

    void HandleNextStageButtonClicked()
    {
        UpdateGameStates(GameState.DrawCard);
    }
    void HandleCombatButtonClicked()
    {
        UpdateGameStates(GameState.Combat);
    }
}

public enum GameState
{
    InventoryManagement,
    DrawCard,
    CombatPreparations,
    Combat,
    Victory,
    Defeat
}