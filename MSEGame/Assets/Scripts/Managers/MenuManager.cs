using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : Manager<NetworkManager>
{

    [SerializeField] private TMP_InputField inputName;
    [SerializeField] private TMP_Dropdown inputRace;
    [SerializeField] private TMP_Dropdown inputProfession;
    [SerializeField] private TMP_Dropdown inputGender;

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
        SessionData.Username = name;
        SceneManager.LoadScene("Gameplay");
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
