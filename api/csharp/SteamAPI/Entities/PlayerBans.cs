using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SteamReactProject.SteamAPI.Entities;

public class SteamUserStatus
{
    #pragma warning disable 8618
    public SteamUserStatus()
    {

    }

    [Newtonsoft.Json.JsonConstructor]
    public SteamUserStatus(
        [JsonProperty("SteamId")] ulong steamId,
        [JsonProperty(nameof(CommunityBanned))] bool communityBanned,
        [JsonProperty(nameof(VACBanned))] bool vacBanned,
        [JsonProperty("NumberOfVACBans")] int numberOfVACBans,
        [JsonProperty("DaysSinceLastBan")] int daysSinceLastBan,
        [JsonProperty("NumberOfGameBans")] int numberOfGameBans,
        [JsonProperty(nameof(EconomyBan))] string economyBan
    )
    {
        SteamID = steamId;
        CommunityBanned = communityBanned;
        VACBanned = vacBanned;
        VACBans = numberOfVACBans;
        LastBan = daysSinceLastBan;
        GameBans = numberOfGameBans;
        EconomyBan = economyBan;
    }

    [JsonProperty("steamid"), JsonPropertyName("steamid"), Key]
    public ulong SteamID { get; set; }

    [JsonProperty("community_banned"), JsonPropertyName("community_banned")]
    public bool CommunityBanned { get; set; }

    [JsonProperty("vac_banned"), JsonPropertyName("vac_banned")]
    public bool VACBanned { get; set; }

    [JsonProperty("number_of_vac_bans"), JsonPropertyName("number_of_vac_bans")]
    public int VACBans { get; set; }

    [JsonProperty("days_since_last_ban"), JsonPropertyName("days_since_last_ban")]
    public int LastBan { get; set; }

    [JsonProperty("number_of_game_bans"), JsonPropertyName("number_of_game_bans")]
    public int GameBans { get; set; }

    [JsonProperty("economy_ban"), JsonPropertyName("economy_ban")]
    public string EconomyBan { get; set; }

    [JsonProperty("lastupdate", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("lastupdate")]
    public DateTime? LastUpdate { get; set; }

    public static IReadOnlyList<SteamUserStatus>? Deserialize(string json)
    {
        var instance = JsonConvert.DeserializeObject<RootSteamUserBanStatus>(json);
        return instance?.PlayersStatus;
    }
}

public class RootSteamUserBanStatus : IDeserialize
{
    [Newtonsoft.Json.JsonConstructor]
    public RootSteamUserBanStatus(
        [JsonProperty("players")] List<SteamUserStatus> playersStatus
    )
    {
        PlayersStatus = playersStatus;
    }

    [JsonProperty("players")]
    public IReadOnlyList<SteamUserStatus> PlayersStatus { get; }

    public static object? Deserialize(string json)
    {
        var instance = JsonConvert.DeserializeObject<RootSteamUserBanStatus>(json);
        return instance?.PlayersStatus;
    }
}

