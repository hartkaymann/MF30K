using System;
using UnityEditor.PackageManager;
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
        UpdateGameStage(GameStage.DrawCard);
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
                HandleDrawCard();
                break;
            case GameStage.CombatPreparations:
                break;
            case GameStage.Combat:
                HandleCombat();
                break;
            case GameStage.Victory:
                HandleVictory();
                break;
            case GameStage.Defeat:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newStage), newStage, null);
        }

        OnGameStateChange?.Invoke(newStage);
        NetworkManager.instance.PutStage(stage);
    }

    async void HandleDrawCard()
    {
        DoorCard card = await NetworkManager.instance.GetCard(CardCategory.Door) as DoorCard;
        if (card is null)
            return;

        RoomManager.instance.InstantiateRoom(card);
    }

    async void DrawTreasureCard()
    {
        Card card = await NetworkManager.instance.GetCard(CardCategory.Treasure);
        CardManager.instance.InstantiateCard(card);
    }

    void HandleCombat()
    {


        UpdateGameStage(GameStage.Victory);
    }

    private void HandleVictory()
    {
        Debug.Log("VICTORY!");
        DrawTreasureCard();
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