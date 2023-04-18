using System;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

enum RequestType
{
    POST,
    GET,
    PUT,
    DELETE
}

class ResponseObject
{
    public int userId;
    public int id;
    public string title;
    public string body;
}

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    private void Awake()
    {
        instance = this;
    }

    [ContextMenu("Send Request")]
    private async void testRequest()
    {
        ResponseObject data = new ResponseObject();
        data.userId = 12345;
        data.id = 1;
        data.title = "Test Title";
        data.body = "Test Body Lorem Ipsum ladida";
        

        UnityWebRequest req = CreateRequest("http://192.168.0.37:5555/test", RequestType.POST, data);
        var operation = req.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        var jsonResponse = req.downloadHandler.text;

        if (req.result == UnityWebRequest.Result.Success)
            Debug.Log($"Success: {req.downloadHandler.text}");
        else
            Debug.Log($"Failed: {req.error}");

        try
        {
            ResponseObject obj = JsonUtility.FromJson<ResponseObject>(jsonResponse);
            Debug.Log($"Response Object Title: " +  obj.title);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Could not parse {jsonResponse}. {ex.Message}");
        }
    }

    private UnityWebRequest CreateRequest(string path, RequestType type = RequestType.GET, object data = null)
    {
        var request = new UnityWebRequest(path, type.ToString());

        if (data != null)
        {
            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        return request;
    }

    public Card getCard()
    {
        return new EquipmentCard("Helmet of Coolness", EquipmentType.Helmet, Sprite.Create(Texture2D.whiteTexture, new Rect(1, 1, 1, 1), Vector2.zero), 10, 5);
    }

    public void getRoom()
    {

    }
}
