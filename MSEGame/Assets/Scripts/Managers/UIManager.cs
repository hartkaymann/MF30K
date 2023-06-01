using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Manager<UIManager> 
{
    [SerializeField] private GameObject panelChange;
    [SerializeField] private GameObject panelVictory;
    [SerializeField] private GameObject panelDefeat;
    [SerializeField] private GameObject panelBlack;
    [SerializeField] private GameObject panelCombat;

    [SerializeField] private GameObject panelHand;
    [SerializeField] private GameObject nextStage;
    [SerializeField] private GameObject backpack;
    [SerializeField] private GameObject equipment;


    [SerializeField] private TextMeshProUGUI textStage;

    protected override void Init()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChanged;
        GameManager.OnChangeClass += GameManagerOnChangeClass;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameStage stage)
    {
        textStage.text = stage.ToString();
        panelChange.SetActive(stage == GameStage.Selection);
        panelVictory.SetActive(stage == GameStage.Victory);
        panelDefeat.SetActive(stage == GameStage.Defeat);
        nextStage.SetActive(!(stage == GameStage.Combat || stage == GameStage.DrawCard));
        panelCombat.SetActive(stage == GameStage.Combat);

        if (stage == GameStage.Victory)
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
            from = PlayerManager.Instance.PlayerController.Player.Profession.ToString();
            to = professionCard.title;
        }
        else if (card is RaceCard raceCard)
        {
            from = PlayerManager.Instance.PlayerController.Player.Race.ToString();
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
        Player player = PlayerManager.Instance.PlayerController.Player;
        if (panelVictory.transform.Find("LevelUp/OldLevel").TryGetComponent<TextMeshProUGUI>(out var oldLevel))
        {
            oldLevel.text = (player.Level - 1).ToString();
        }

        if (panelVictory.transform.Find("LevelUp/NewLevel").TryGetComponent<TextMeshProUGUI>(out var newLevel))
        {
            newLevel.text = player.Level.ToString();
        }
    }

    public IEnumerator FadeToBlack(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            panelBlack.GetComponent<Image>().color = Color.black.WithAlpha(alpha);
            yield return null;
        }
    }

    public void ToggleBlackScreen()
    {
        panelBlack.SetActive(!panelBlack.activeInHierarchy);
    }
}
