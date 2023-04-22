using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // State management
    public static GameManager instance;
    public GameState state;
    public static event Action<GameState> OnGameStateChange;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Event handlers and listeners
        UpdateGameStates(GameState.InventoryManagement);
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
                DrawDoorCard();
                break;
            case GameState.CombatPreparations:
                break;
            case GameState.Combat:
                Combat();
                break;
            case GameState.Victory:
                Victory();
                break;
            case GameState.Defeat:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChange?.Invoke(newState);
    }

    async void DrawDoorCard()
    {
        Card card = await NetworkManager.instance.GetCard(CardType.Door);
        CardManager.instance.InstantiateCard(card);
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

    public void NextStage()
    {
        switch (state)
        {
            case GameState.InventoryManagement:
                UpdateGameStates(GameState.DrawCard);
                break;
            case GameState.DrawCard:
                UpdateGameStates(GameState.CombatPreparations);
                break;
            case GameState.CombatPreparations:
                UpdateGameStates(GameState.Combat);
                break;
            case GameState.Combat:
                UpdateGameStates(GameState.Victory);
                break;
            case GameState.Victory:
                UpdateGameStates(GameState.InventoryManagement);
                break;
            case GameState.Defeat:
                UpdateGameStates(GameState.InventoryManagement);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
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