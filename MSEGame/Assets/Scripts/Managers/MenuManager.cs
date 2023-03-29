using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
 
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject btnNext;
    [SerializeField] private GameObject btnCombat;

    private void Awake()
    {
        instance = this;
        GameManager.OnGameStateChange += GameManagerOnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState state)
    {
        btnNext.SetActive(state == GameState.InventoryManagement);
        btnCombat.SetActive(state == GameState.CombatPreparations);
    }

}
