using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject btnNext;
    [SerializeField] private GameObject backpack;
    [SerializeField] private GameObject equipment;


    [SerializeField] private TextMeshProUGUI textStage;

    private void Awake()
    {
        instance = this;
        GameManager.OnGameStateChange += GameManagerOnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChanged;
    }

    private void Start()
    {
    }

    private void GameManagerOnGameStateChanged(GameStage state)
    {
        btnNext.SetActive(true);
        textStage.text = state.ToString();
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
