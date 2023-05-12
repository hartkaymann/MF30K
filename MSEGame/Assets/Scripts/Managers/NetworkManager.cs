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
    public static NetworkManager Instance { get; private set; }
    
    private readonly string url = "localhost";
    private readonly string port = "8080";

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

    private UnityWebRequest CreateRequest(string path, RequestType type = RequestType.GET, object data = null)
    {
        //Debug.Log("New Request: " + path);
        var request = new UnityWebRequest(path, type.ToString());

        if (data != null)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            //Debug.Log($"Request Body: {jsonData}");
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

        if (req == null)
            return null;

        var obj = await SendRequestWithResponse(req);
        if (!obj.HasValues)
            return null;

        //TODO: Check if key exist before trying to get it
        string cardType = (string)obj.SelectToken("type");
        string name = (string)obj.SelectToken("name");
        string id = (string)obj.SelectToken("id");

        Card card = null;

        //Debug.Log($"Class: {cardType}, Name: {name}");
        switch (cardType)
        {
            case "Consumable":
                {
                    int value = int.Parse((string)obj.SelectToken("goldValue"));
                    int bonus = int.Parse((string)obj.SelectToken("combatBonus"));
                    BuffTarget target = ParseEnum<BuffTarget>((string)obj.SelectToken("target"));
                    Sprite artwork = SpriteManager.Instance.GetConsumableSprite();
                    card = new ConsumableCard(name, id, artwork, value, bonus, target);
                    break;
                }
            case "Monster":
                {
                    int combatLvl = int.Parse((string)obj.SelectToken("combatLevel"));
                    int treasures = int.Parse((string)obj.SelectToken("treasureAmount"));
                    Sprite artwork = SpriteManager.Instance.GetSprite("Slime");
                    card = new MonsterCard(name, id, artwork, combatLvl, treasures);
                    break;
                }
            case "Equipment":
                {
                    EquipmentType equipType = ParseEnum<EquipmentType>((string)obj.SelectToken("equipType"));
                    int bonus = int.Parse((string)obj.SelectToken("combatBonus"));
                    int value = int.Parse((string)obj.SelectToken("goldValue"));
                    Sprite artwork = SpriteManager.Instance.GetEquipmentSprite(equipType);
                    card = new EquipmentCard(name, equipType, id, artwork, value, bonus);
                    break;
                }
            case "Profession":
                {
                    Profession profession = ParseEnum<Profession>((string)obj.SelectToken("profession"));
                    Sprite artwork = SpriteManager.Instance.GetProfessionSprite();
                    card = new ProfessionCard(profession, id, artwork);
                    break;
                }
            case "Race":
                {
                    Race race = ParseEnum<Race>((string)obj.SelectToken("race"));
                    Sprite artwork = SpriteManager.Instance.GetRaceSprite();
                    card = new RaceCard(race, id, artwork);
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
        UnityWebRequest req = CreateRequest($"http://{url}:{port}/player?id={player.Name}", RequestType.PUT, player);
        req.SendWebRequest();
        req.Dispose();

    }

    public void PutStage(GameStage stage)
    {
        UnityWebRequest req = CreateRequest($"http://{url}:{port}/stage", RequestType.PUT, stage.ToString());
        req.SendWebRequest();
        req.Dispose();
    }

    public void PutBackpack(Player player, List<Card> backpack)
    {
        UnityWebRequest req = CreateRequest($"http://{url}:{port}/player/{player.Name}/backpack", RequestType.PUT, backpack);
        req.SendWebRequest();
        req.Dispose();
    }
    public void PutEquipment(Player player, Dictionary<EquipmentSlot, EquipmentCard> equipment)
    {
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
