using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // State management
    public static GameManager instance;
    public GameStage stage;
    public static event Action<GameStage> OnGameStateChange;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Event handlers and listeners
        UpdateGameStage(GameStage.InventoryManagement);
    }

    public void UpdateGameStage(GameStage newStage)
    {
        stage = newStage;
        Debug.Log("New Stage: " + stage.ToString());

        switch (newStage)
        {
            case GameStage.InventoryManagement:
                break;
            case GameStage.DrawCard:
                DrawDoorCard();
                break;
            case GameStage.CombatPreparations:
                break;
            case GameStage.Combat:
                Combat();
                break;
            case GameStage.Victory:
                Victory();
                break;
            case GameStage.Defeat:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newStage), newStage, null);
        }

        OnGameStateChange?.Invoke(newStage);
        NetworkManager.instance.PutStage(stage);
    }

    async void DrawDoorCard()
    {
        Card card = await NetworkManager.instance.GetCard(CardType.Door);
        CardManager.instance.InstantiateCard(card);
    }

    void Combat()
    {
        Debug.Log("DOING COMBAT!");

        UpdateGameStage(GameStage.Victory);
    }

    private void Victory()
    {
        Debug.Log("VICTORY!");
        UpdateGameStage(GameStage.InventoryManagement);
    }

    public void NextStage()
    {
        switch (stage)
        {
            case GameStage.InventoryManagement:
                UpdateGameStage(GameStage.DrawCard);
                break;
            case GameStage.DrawCard:
                UpdateGameStage(GameStage.CombatPreparations);
                break;
            case GameStage.CombatPreparations:
                UpdateGameStage(GameStage.Combat);
                break;
            case GameStage.Combat:
                UpdateGameStage(GameStage.Victory);
                break;
            case GameStage.Victory:
                UpdateGameStage(GameStage.InventoryManagement);
                break;
            case GameStage.Defeat:
                UpdateGameStage(GameStage.InventoryManagement);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(stage), stage, null);
        }
    }
}

public enum GameStage
{
    InventoryManagement,
    DrawCard,
    CombatPreparations,
    Combat,
    Victory,
    Defeat
}