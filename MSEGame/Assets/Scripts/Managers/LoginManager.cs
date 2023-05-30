using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : Manager<LoginManager>
{

    [SerializeField] TMP_InputField inputUsername;
    private Outline outline;

    public void Start()
    {
        if(!inputUsername.TryGetComponent(out outline))
        {
            Debug.LogWarning("Couldn't get Outline component of Username Input");
        }

        outline.enabled = false;
    }

    public async void SignIn()
    {
        outline.enabled = false;
        string username = inputUsername.text;

        bool isValid = await NetworkManager.Instance.PostSignIn(username);

        if(!isValid)
        {
            if(inputUsername.TryGetComponent<Outline>(out var outline))
            {
                outline.enabled = true;
            }
            return;
        }

        SessionData.Username = username;
        SceneManager.LoadScene("MainMenu");
    }
}
