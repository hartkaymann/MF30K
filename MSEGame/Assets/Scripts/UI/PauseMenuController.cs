using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textMusic;
    [SerializeField] private TextMeshProUGUI textSound;

    public void ExitRound()
    {
        Player player = PlayerManager.Instance.PlayerController.Player;
        StartCoroutine(NetworkManager.Instance.PutRun(player));

        SceneManager.LoadScene("MainMenu");
    }

    public void OnButtonMusicClicked()
    {
        SoundManager.Instance.ToggleMusic();
        textMusic.text = $"Music: {(SoundManager.Instance.MusicOn ? "On" : "Off")}";
    }

    public void OnButtonSoundClicked()
    {
        SoundManager.Instance.ToggleSound();
        textSound.text = $"Sound: {(SoundManager.Instance.SoundOn ? "On" : "Off")}";
    }
}
