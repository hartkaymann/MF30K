using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : Manager<MenuManager>
{
    [SerializeField] private Image playerImg;
    [SerializeField] private Sprite imgHuman;
    [SerializeField] private Sprite imgElf;
    [SerializeField] private Sprite imgOrc;


    [SerializeField] private TMP_Dropdown inputRace;
    [SerializeField] private TMP_Dropdown inputProfession;
    [SerializeField] private TMP_Dropdown inputGender;

    private void Awake()
    {
        AudioListener.volume = 0.0f;
    }
    public void StartGame()
    {
        Race race = ParseEnum<Race>(inputRace.options[inputRace.value].text);
        Profession profession = ParseEnum<Profession>(inputProfession.options[inputProfession.value].text);
        Gender gender = ParseEnum<Gender>(inputGender.options[inputGender.value].text);

        if (SessionData.Username.Length == 0)
        {
            SessionData.Username = "Empty";
        }

        Player player = new(SessionData.Username, race, profession, gender, 1, 0);
        StartCoroutine(NetworkManager.Instance.PostPlayer(player));
        SceneManager.LoadScene("Intro");
    }

    public void UpdatePlayerImage()
    {
        playerImg.sprite = ParseEnum<Race>(inputRace.options[inputRace.value].text) switch
        {
            Race.Human => imgHuman,
            Race.Elf => imgElf,
            Race.Orc => imgOrc,
            _ => imgHuman,
        };
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public void LogOut()
    {
        SceneManager.LoadScene("Login");
    }
}
