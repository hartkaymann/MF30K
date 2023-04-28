using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Net;

enum RequestType
{
    POST,
    GET,
    PUT,
    DELETE
}

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    [SerializeField] private string url;
    [SerializeField] private string port;

    private void Awake()
    {
        instance = this;
    }

    [ContextMenu("Send Request")]
    private async void testRequest()
    {

        string apiKey = "0d8dc82ca22ed494ecc0955e0a6187cc";
        float lat = 37.532600f;
        float lon = 127.024612f;
        string path = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={apiKey}";

        UnityWebRequest req = CreateRequest(path, RequestType.GET);
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
            JObject o = JObject.Parse(jsonResponse);
            Debug.Log($"Response Code: {o.SelectToken("weather[0].main")}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Could not parse {jsonResponse}. {ex.Message}");
        }
    }

    private UnityWebRequest CreateRequest(string path, RequestType type = RequestType.GET, object data = null)
    {
        Debug.Log("path: " + path);
        var request = new UnityWebRequest(path, type.ToString());

        if (data != null)
        {
            var bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        return request;
    }

    async Task<JObject> SendRequestWithResponse(UnityWebRequest req)
    {
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
            JObject obj = JObject.Parse(jsonResponse);
            return obj;
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Could not parse {jsonResponse.Prettify()}. {ex.Message}");
        }

        return null;
    }

    //////////
    // POST //
    //////////
    public async void PostPlayer(Player player)
    {
        UnityWebRequest req = CreateRequest($"{url}:{port}/player", RequestType.POST, player);

        var response = await SendRequestWithResponse(req);

        req.Dispose();
        //TODO: set assigned player id to new player
    }

    /////////
    // GET //
    /////////
    public async Task<Card> GetCard(CardType type)
    {
        Debug.Log("Get card");
        UnityWebRequest req = CreateRequest($"http://{url}:{port}/card?type={type}", RequestType.GET);

        var obj = await SendRequestWithResponse(req);
        string cardType = (string)obj.SelectToken("type");
        string name = (string)obj.SelectToken("name");
        Card card = null;

        Debug.Log($"Class: {cardType}, Name: {name}");
        Sprite dummySprite = Sprite.Create(Texture2D.whiteTexture, new Rect(1, 1, 1, 1), Vector2.zero);
        switch (cardType)
        {
            default:
                {
                    string equipType = (string)obj.SelectToken("equipType");
                    int cost = int.Parse((string)obj.SelectToken("goldValue"));
                    int stat = int.Parse((string)obj.SelectToken("combatBonus"));
                    card = new EquipmentCard(name, ParseEnum<EquipmentType>(equipType), dummySprite, cost, stat);
                    break;
                }
            case "consumable":
                {
                    int cost = int.Parse((string)obj.SelectToken("goldValue"));
                    int stat = int.Parse((string)obj.SelectToken("combatBonus"));
                    card = new ConsumableCard(name, dummySprite, cost, stat);
                    break;
                }
            case "monster":
                {
                    int combatLvl = int.Parse((string)obj.SelectToken("combatLevel"));
                    int treasures = int.Parse((string)obj.SelectToken("treasureAmount"));
                    card = new MonsterCard(name, dummySprite, combatLvl, treasures);
                    break;
                }
            case "equipment":
                {
                    Debug.LogAssertion($"Warning! No/Unknown class in generated card: {cardType}");
                    break;
                }
        }

        req.Dispose();
        
        return card;
        //return new EquipmentCard("Helmet of Coolness", EquipmentType.Helmet, Sprite.Create(Texture2D.whiteTexture, new Rect(1, 1, 1, 1), Vector2.zero), 10, 5);
    }

    public async Task<Player> GetPlayer(int id)
    {
        Debug.Log("Get Player");
        UnityWebRequest req = CreateRequest($"http://{url}:{port}/player?id={id}", RequestType.GET);
        
        var obj = await SendRequestWithResponse(req);
        string name = (string) obj.SelectToken("name");
        string gender = (string)obj.SelectToken("gender");
        string race = (string)obj.SelectToken("race");
        string profession = (string)obj.SelectToken("profession");
        int level = int.Parse((string)obj.SelectToken("playerLevel"));
        int combatLvl = int.Parse((string)obj.SelectToken("combatLevel"));

        Player p = new Player(id, name)
        {
            Gender = ParseEnum<Gender>(gender),
            Race = ParseEnum<Race>(race),
            Profession = ParseEnum<Profession>(profession),
            Level = level,
            CombatLevel = combatLvl,
        };

        req.Dispose();

        return p;
    }

    public async Task<GameStage> GetStage()
    {
        Debug.Log("Get Stage");
        UnityWebRequest req = CreateRequest($"http://{url}:{port}/stage", RequestType.GET);

        var obj = await SendRequestWithResponse(req);

        req.Dispose();

        return ParseEnum<GameStage>((string) obj.SelectToken("GameStage"));
    }

    /////////
    // PUT //
    /////////

    public void PutPlayer(Player player)
    {
        Debug.Log("Put Player");
        UnityWebRequest req = CreateRequest($"http://{url}:{port}/player?id={player.Id}", RequestType.PUT, player);
        req.SendWebRequest();
        req.Dispose();

    }

    public void PutStage(GameStage stage)
    {
        Debug.Log("Put Stage");
        UnityWebRequest req = CreateRequest($"http://{url}:{port}/stage", RequestType.PUT, stage.ToString());
        req.SendWebRequest();
        req.Dispose();
    }

    public void PutBackpack(Player player, List<Card> backpack)
    {
        Debug.Log("Put Backpack");
        UnityWebRequest req = CreateRequest($"http://{url}:{port}/player/{player.Id}/backpack", RequestType.PUT, backpack);
        req.SendWebRequest();
        req.Dispose();
    }
    public void PutEquipment(Player player, Dictionary<EquipmentSlot, EquipmentCard> equipment)
    {
        Debug.Log("Put Backpack");
        UnityWebRequest req = CreateRequest($"http://{url}:{port}/player/{player.Id}/backpack", RequestType.PUT, equipment);
        req.SendWebRequest();
        req.Dispose();
    }

    ///////////
    // DELET //
    ///////////

        
    public static T ParseEnum<T>(string value)
    {   
        return (T)Enum.Parse(typeof(T), value, true);
    }
}
