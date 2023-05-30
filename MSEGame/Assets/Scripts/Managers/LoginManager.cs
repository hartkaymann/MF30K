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
    private Outline outline;

    public void Start()
    {
        if(!inputUsername.TryGetComponent(out outline))
        {
            Debug.LogWarning("Couldn't get Outline component of Username Input");
        }
        outline.enabled = false;
   
        if(!connection.Find("Status").TryGetComponent(out connectionStatus))
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

        if(username.Length == 0)
        {
            SetInputValid(false);
            return;
        }

        bool isValid = await NetworkManager.Instance.PostSignIn(username);
        if(!isValid)
        {
            if(inputUsername.TryGetComponent<Outline>(out var outline))
            {
                SetInputValid(false);
            }
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
            return;
        }

        bool isValid = await NetworkManager.Instance.PostSignUp(username);
        if (!isValid)
        {
            if (inputUsername.TryGetComponent<Outline>(out var outline))
            {
                SetInputValid(false);
            }
            return;
        }

        SessionData.Username = username;
        SceneManager.LoadScene("MainMenu");
    }

    private void SetInputValid(bool valid)
    {
        outline.enabled = !valid;
    }
}
