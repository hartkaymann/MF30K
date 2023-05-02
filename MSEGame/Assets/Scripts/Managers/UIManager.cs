using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject btnNext;
    [SerializeField] private GameObject backpack;
    [SerializeField] private GameObject equipment;
    [SerializeField] private GameObject playerInfo;
    [SerializeField] private GameObject npcInfo;


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
        playerInfo.SetActive(true); 
        npcInfo.SetActive(true);
    }

    private void GameManagerOnGameStateChanged(GameStage state)
    {
        btnNext.SetActive(true);
        textStage.text = state.ToString();

        switch (state)
        {
            case GameStage.DrawCard:
                UpdateNpcInfo();

                break;
            default:
                break;
        }
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

    private void UpdateNpcInfo()
    {
        if (npcInfo.TryGetComponent<ObjectFollow>(out var objectFollow))
        {
            // Follow room's npc
            objectFollow.Follow = RoomManager.instance.CurrentRoom.Renderer.Enemy.transform;

            // Update info
            TextMeshProUGUI name = npcInfo.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            DoorCard room = RoomManager.instance.CurrentRoom.Card;
            name.text = room.title;

            if(room is MonsterCard mob)
            {
                npcInfo.transform.Find("Level").GetComponent<TextMeshProUGUI>().text = mob.level.ToString();

            }

            
        }
    }

}
