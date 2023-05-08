using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;

enum RequestType
{
    POST,
    GET,
    PUT,
    DELETE
}

public class NetworkManager : MonoBehaviour
{
    private readonly string url = "localhost";
    private readonly string port = "8080";

    public static NetworkManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [ContextMenu("Send Request")]
    private async void TestRequest()
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
        Debug.Log("New Request: " + path);
        var request = new UnityWebRequest(path, type.ToString());

        if (data != null)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            Debug.Log($"Request Body: {jsonData}");
            var bodyRaw = Encoding.UTF8.GetBytes(jsonData);
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
            Debug.Log($"JSON Respose: {jsonResponse}");
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
    }

    /////////
    // GET //
    /////////
    public async Task<Card> GetCard(CardCategory type)
    {
        UnityWebRequest req = CreateRequest($"http://{url}:{port}/card?type={type.ToString().ToLower()}", RequestType.GET);

        var obj = await SendRequestWithResponse(req);
        string cardType = (string)obj.SelectToken("type");
        string name = (string)obj.SelectToken("name");
        string id = (string)obj.SelectToken("id");

        Card card = null;

        //Debug.Log($"Class: {cardType}, Name: {name}");
        Sprite dummySprite = Sprite.Create(Texture2D.whiteTexture, new Rect(1, 1, 1, 1), Vector2.zero);
        switch (cardType)
        {
            case "Consumable":
                {
                    int value = int.Parse((string)obj.SelectToken("goldValue"));
                    int bonus = int.Parse((string)obj.SelectToken("combatBonus"));
                    BuffTarget target = ParseEnum<BuffTarget>((string)obj.SelectToken("target"));
                    card = new ConsumableCard(name, id, dummySprite, value, bonus, target);
                    break;
                }
            case "Monster":
                {
                    int combatLvl = int.Parse((string)obj.SelectToken("combatLevel"));
                    int treasures = int.Parse((string)obj.SelectToken("treasureAmount"));
                    card = new MonsterCard(name, id, dummySprite, combatLvl, treasures);
                    break;
                }
            case "Equipment":
                {
                    EquipmentType equipType = ParseEnum<EquipmentType>((string)obj.SelectToken("equipType"));
                    int bonus = int.Parse((string)obj.SelectToken("combatBonus"));
                    int value = int.Parse((string)obj.SelectToken("goldValue"));
                    card = new EquipmentCard(name, equipType, id, dummySprite, value, bonus);
                    break;
                }
            case "Profession":
                {
                    Profession profession = ParseEnum<Profession>((string)obj.SelectToken("profession"));
                    card = new ProfessionCard(profession, id, dummySprite);
                    break;
                }
            case "Race":
                {
                    Race race = ParseEnum<Race>((string)obj.SelectToken("race"));
                    card = new RaceCard(race, id, dummySprite);
                    break;
                }
            default:
                {
                    Debug.LogAssertion($"Unknown card type: {cardType}");
                    break;
                }
        }

        req.Dispose();

        return card;
    }

    public async Task<Player> GetPlayer(string name)
    {
        Debug.Log("Get Player");
        UnityWebRequest req = CreateRequest($"http://{url}:{port}/player?id={name}", RequestType.GET);

        var obj = await SendRequestWithResponse(req);
        string gender = (string)obj.SelectToken("gender");
        string race = (string)obj.SelectToken("race");
        string profession = (string)obj.SelectToken("profession");
        int level = int.Parse((string)obj.SelectToken("playerLevel"));
        int combatLvl = int.Parse((string)obj.SelectToken("combatLevel"));

        req.Dispose();

        return new Player(
            name,
            ParseEnum<Race>(race),
            ParseEnum<Profession>(profession),
            ParseEnum<Gender>(gender),
            level,
            combatLvl
        );
    }

    public async Task<GameStage> GetStage()
    {
        Debug.Log("Get Stage");
        UnityWebRequest req = CreateRequest($"http://{url}:{port}/stage", RequestType.GET);

        var obj = await SendRequestWithResponse(req);

        req.Dispose();

        return ParseEnum<GameStage>((string)obj.SelectToken("GameStage"));
    }

    /////////
    // PUT //
    /////////

    public void PutPlayer(Player player)
    {
        Debug.Log("Put Player");
        UnityWebRequest req = CreateRequest($"http://{url}:{port}/player?id={player.Name}", RequestType.PUT, player);
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
        UnityWebRequest req = CreateRequest($"http://{url}:{port}/player/{player.Name}/backpack", RequestType.PUT, backpack);
        req.SendWebRequest();
        req.Dispose();
    }
    public void PutEquipment(Player player, Dictionary<EquipmentSlot, EquipmentCard> equipment)
    {
        Debug.Log("Put Backpack");
        UnityWebRequest req = CreateRequest($"http://{url}:{port}/player/{player.Name}/backpack", RequestType.PUT, equipment);
        req.SendWebRequest();
        req.Dispose();
    }

    ////////////
    // DELETE //
    ////////////


    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }
}
