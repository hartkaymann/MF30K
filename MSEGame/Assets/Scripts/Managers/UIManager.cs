using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject panelHand;
    [SerializeField] private GameObject panelChange;
    [SerializeField] private GameObject panelVictory;
    [SerializeField] private GameObject panelDefeat;
    [SerializeField] private GameObject btnNext;
    [SerializeField] private GameObject backpack;
    [SerializeField] private GameObject equipment;


    [SerializeField] private TextMeshProUGUI textStage;

    private void Awake()
    {
        instance = this;
        GameManager.OnGameStateChange += GameManagerOnGameStateChanged;
        GameManager.OnChangeClass += GameManagerOnChangeClass;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameStage state)
    {
        textStage.text = state.ToString();
        panelChange.SetActive(state == GameStage.ChangeClass);
        panelVictory.SetActive(state == GameStage.Victory);
        panelDefeat.SetActive(state == GameStage.Defeat);

        if (state == GameStage.Victory)
            UpdateVictoryPanel();
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

    public void GameManagerOnChangeClass(DoorCard card)
    {
        string type = card.type.ToString();
        string from = "";
        string to = "";

        if (card is ProfessionCard professionCard)
        {
            from = PlayerManager.Instance.CurrentPlayer.Player.Profession.ToString();
            to = professionCard.title;
        }
        else if (card is RaceCard raceCard)
        {
            from = PlayerManager.Instance.CurrentPlayer.Player.Race.ToString();
            to = raceCard.title;
        }
        else
        {
            Debug.LogWarning("Trying to change something that cant be chagned! " + card.type.ToString());
        }

        if (panelChange.transform.Find("Title").TryGetComponent<TextMeshProUGUI>(out var textTitle))
        {
            textTitle.text = $"Change current {type} from {from} to {to}?";
        }
    }
    public void UpdateVictoryPanel()
    {
        Player player = PlayerManager.Instance.CurrentPlayer.Player;
        if (panelVictory.transform.Find("LevelUp/OldLevel").TryGetComponent<TextMeshProUGUI>(out var oldLevel))
        {
            oldLevel.text = (player.Level - 1).ToString();
        }

        if (panelVictory.transform.Find("LevelUp/NewLevel").TryGetComponent<TextMeshProUGUI>(out var newLevel))
        {
            newLevel.text = player.Level.ToString();
        }
    }
}
