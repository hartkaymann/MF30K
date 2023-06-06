using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public void ExitRound()
    {
        Player player = PlayerManager.Instance.PlayerController.Player;
        StartCoroutine(NetworkManager.Instance.PostEndRun(player));

        SceneManager.LoadScene("MainMenu");
    }
}
