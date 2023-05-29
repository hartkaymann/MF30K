using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using System.Collections;

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
        Debug.Log($"New {type} Request: {path}");
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
            Debug.Log($"JSON Respose: {jsonResponse.Prettify()}");
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

    [ContextMenu("Test Post")]
    private IEnumerator TestPost()
    {
        string path = $"http://{url}:{port}/test";
        UnityWebRequest req = CreateRequest(path, RequestType.POST);
        yield return req.SendWebRequest();
        while(!req.isDone)
        {
            yield return null;
        }
        req.Dispose();
    }

    [ContextMenu("Post Dummy Player")]
    private void PostDummyPlayer()
    {
        StartCoroutine(PostPlayer(Player.GetDummy()));
    }

    public IEnumerator PostPlayer(Player player)
    {
        string path = $"http://{url}:{port}/player";
        UnityWebRequest req = CreateRequest(path, RequestType.POST, player);
        yield return req.SendWebRequest();
        while (!req.isDone)
        {
            Debug.Log("Not done here");
            yield return null;
        }
        req.Dispose();
    }

    public IEnumerator PostEndRun(Player player)
    {
        string path = $"http://{url}:{port}/endrun";
        UnityWebRequest req = CreateRequest(path, RequestType.POST, player);
        yield return req.SendWebRequest();
        while (!req.isDone)
        {
            Debug.Log("Not done here");
            yield return null;
        }
        req.Dispose();
    }

    /////////
    // GET //
    /////////

    [ContextMenu("Get Door Card Test")]
    private async void GetDoorCard()
    {
        Card card = await GetCard(CardCategory.Door);
        Debug.Log("Card: " + card);
    }

    public async Task<Card> GetCard(CardCategory type)
    {
        string path = $"http://{url}:{port}/card?type={type.ToString().ToLower()}";
        Debug.Log("Get Card: " + path);
        UnityWebRequest req = CreateRequest(path, RequestType.GET);

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
        string path = $"http://{url}:{port}/player/{name}";
        Debug.Log("Get Player: " + path);
        UnityWebRequest req = CreateRequest(path, RequestType.GET);

        var obj = await SendRequestWithResponse(req);
        if (obj == null)
        {
            Debug.LogWarning("Couldnt get Player, was null. Proceeding with dummy...");
            return Player.GetDummy();
        }

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

    public IEnumerator PutPlayer(Player player)
    {
        UnityWebRequest req = CreateRequest($"http://{url}:{port}/player/{player.Name}", RequestType.PUT, player);
        yield return req.SendWebRequest();
        req.Dispose();

    }

    public IEnumerator PutStage(GameStage stage)
    {
        UnityWebRequest req = CreateRequest($"http://{url}:{port}/stage", RequestType.PUT, stage.ToString());
        yield return req.SendWebRequest();
        req.Dispose();
    }

    public IEnumerator PutBackpack(Player player, List<Card> backpack)
    {
        UnityWebRequest req = CreateRequest($"http://{url}:{port}/player/{player.Name}/backpack", RequestType.PUT, backpack);
        yield return req.SendWebRequest();
        req.Dispose();
    }
    public IEnumerator PutEquipment(Player player, Dictionary<EquipmentSlot, EquipmentCard> equipment)
    {
        UnityWebRequest req = CreateRequest($"http://{url}:{port}/player/{player.Name}/backpack", RequestType.PUT, equipment);
        yield return req.SendWebRequest();
        req.Dispose();
    }

    ////////////
    // DELETE //
    ////////////

    public IEnumerator DiscardCard(Player player, Card card)
    {
        UnityWebRequest req = CreateRequest($"http://{url}:{port}/discard?cardId={card.id}", RequestType.DELETE);
        yield return req.SendWebRequest();
        req.Dispose();
    }

    public IEnumerator SellCard(Player player, TreasureCard card)
    {
        UnityWebRequest req = CreateRequest($"http://{url}:{port}/player/{player.Name}/sell?cardId={card.id}", RequestType.PUT);
        yield return req.SendWebRequest();
        req.Dispose();
    }
    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }
}
