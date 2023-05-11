using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private Transform inputName;
    [SerializeField] private Transform inputRace;
    [SerializeField] private Transform inputProfession;
    [SerializeField] private Transform inputGender;

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
        // Shouldn't be setting default values here
        string name = "";
        Race race = Race.Human;
        Profession profession = Profession.Barbarian;
        Gender gender = Gender.Male;

        if (inputName.TryGetComponent<InputField>(out var fieldName))
        {
            name = fieldName.text;
        }

        if (inputRace.TryGetComponent<Dropdown>(out var fieldRace))
        {
            race = ParseEnum<Race>(fieldRace.options[fieldRace.value].text);
        }

        if (inputRace.TryGetComponent<Dropdown>(out var fieldProfession))
        {
            profession = ParseEnum<Profession>(fieldProfession.options[fieldProfession.value].text);
        }

        if (inputRace.TryGetComponent<Dropdown>(out var fieldGender))
        {
            gender = ParseEnum<Gender>(fieldGender.options[fieldGender.value].text);
        }


        NetworkManager.Instance.PostPlayer(new Player(name, race, profession, gender, 1, 1));

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
