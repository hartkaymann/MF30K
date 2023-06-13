using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Globalization;

public partial class UserData
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("wins")]
    public long Wins { get; set; }

    [JsonProperty("losses")]
    public long Losses { get; set; }

    [JsonProperty("registrationDate")]
    public DateTimeOffset RegistrationDate { get; set; }

    [JsonProperty("runs")]
    public Run[] Runs { get; set; }
}

public partial class Run
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("combatLevel")]
    public long CombatLevel { get; set; }

    [JsonProperty("playerLevel")]
    public long PlayerLevel { get; set; }

    [JsonProperty("goldsold")]
    public long Goldsold { get; set; }

    [JsonProperty("profession")]
    public string Profession { get; set; }

    [JsonProperty("race")]
    public string Race { get; set; }

    [JsonProperty("combats")]
    public Combat[] Combats { get; set; }
}

public partial class Combat
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("combatLvlPlayer")]
    public long CombatLvlPlayer { get; set; }

    [JsonProperty("combatLvlMonster")]
    public long CombatLvlMonster { get; set; }

    [JsonProperty("win")]
    public bool Win { get; set; }

    [JsonProperty("consequence")]
    public long Consequence { get; set; }
}

public partial class UserData
{
    public static UserData FromJson(string json) => JsonConvert.DeserializeObject<UserData>(json, Converter.Settings);
}

public static class Serialize
{
    public static string ToJson(this UserData self) => JsonConvert.SerializeObject(self, Converter.Settings);
}

internal static class Converter
{
    public static readonly JsonSerializerSettings Settings = new()
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
    };
}
