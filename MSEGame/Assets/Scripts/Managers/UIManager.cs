using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject btnNext;
    [SerializeField] private GameObject backpack;
    [SerializeField] private GameObject equipment;

    private void Awake()
    {
        instance = this;
        GameManager.OnGameStateChange += GameManagerOnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameStage state)
    {
        btnNext.SetActive(true);
        //btnNext.SetActive(state == GameState.InventoryManagement);
    }

    public void HandleToggleEquipment()
    {
        if (backpack.activeInHierarchy)
            backpack.SetActive(false); 

        equipment.SetActive(!equipment.activeInHierarchy);
    }

    public void HandleToggleBackpack()
    {
        if (equipment.activeInHierarchy)
            equipment.SetActive(false);

        backpack.SetActive(!backpack.activeInHierarchy);
    }

}
