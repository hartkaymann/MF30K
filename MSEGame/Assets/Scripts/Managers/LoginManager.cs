using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : Manager<LoginManager>
{

    [SerializeField] Transform connection;
    private TextMeshProUGUI connectionStatus;
    private Image connectoinIcon;

    [SerializeField] TMP_InputField inputUsername;
    [SerializeField] GameObject textError;
    private Outline outline;

    public void Start()
    {
        if (!inputUsername.TryGetComponent(out outline))
        {
            Debug.LogWarning("Couldn't get Outline component of Username Input");
        }
        outline.enabled = false;

        if (!connection.Find("Status").TryGetComponent(out connectionStatus))
        {
            Debug.LogWarning("Couldn't get Text component in Connection GameObject");
        }

        if (!connection.Find("Icon").TryGetComponent(out connectoinIcon))
        {
            Debug.LogWarning("Couldn't get Image component in Connection GameObject");
        }

        StartCoroutine(MonitorConnection());
    }

    private IEnumerator MonitorConnection()
    {
        for (; ; )
        {
            CheckConnection();
            yield return new WaitForSeconds(3);
        }
    }

    private async void CheckConnection()
    {
        bool connected = await NetworkManager.Instance.GetConnection();
        connectionStatus.text = (connected ? "" : "Not ") + "Connected";
        connectoinIcon.color = connected ? GameColor.Green : GameColor.Red;
    }

    public async void SignIn()
    {
        SetInputValid(true);
        string username = inputUsername.text.ToLower();

        if (username.Length == 0)
        {
            SetInputValid(false);
            return;
        }

        bool isValid = await NetworkManager.Instance.PostSignIn(username);
        if (!isValid)
        {
            SetInputValid(false);
            DisplayErrorMessage($"Sign In failed. User '{username}' doesn't exist.");
            
            return;
        }

        SessionData.Username = username;
        SceneManager.LoadScene("MainMenu");
    }

    public async void SignUp()
    {
        SetInputValid(true);
        string username = inputUsername.text.ToLower();

        if (username.Length == 0)
        {
            SetInputValid(false);
            DisplayErrorMessage($"Please enter a name.");
            return;
        }

        bool isValid = await NetworkManager.Instance.PostSignUp(username);
        if (!isValid)
        {
            SetInputValid(false);
            DisplayErrorMessage($"Sign Up failed. User '{username}' already exists.");

            return;
        }

        SessionData.Username = username;
        SceneManager.LoadScene("MainMenu");
    }

    private void DisplayErrorMessage(string errorMsg)
    {
        textError.SetActive(true);
        if (TryGetComponent<TextMeshProUGUI>(out var textComponent))
        {
            textComponent.text = errorMsg;
        }
    }

    private void SetInputValid(bool valid)
    {
        outline.enabled = !valid;

        if (valid)
        {
            textError.SetActive(false);
        }
    }
}
