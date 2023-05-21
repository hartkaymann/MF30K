using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private TMP_InputField inputName;
    [SerializeField] private TMP_Dropdown inputRace;
    [SerializeField] private TMP_Dropdown inputProfession;
    [SerializeField] private TMP_Dropdown inputGender;

    public static MenuManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void StartGame()
    {
        string name = inputName.text;
        if (name.Length == 0)
            return;

        Race race = ParseEnum<Race>(inputRace.options[inputRace.value].text);
        Profession profession = ParseEnum<Profession>(inputProfession.options[inputProfession.value].text);
        Gender gender = ParseEnum<Gender>(inputGender.options[inputGender.value].text);

        Player player = new(name, race, profession, gender, 1, 0);
        StartCoroutine(NetworkManager.Instance.PostPlayer(player));
        LoadSceneInformation.PlayerName = name;
        SceneManager.LoadScene(1);
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
}
