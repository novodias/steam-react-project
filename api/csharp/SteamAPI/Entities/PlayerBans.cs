using Newtonsoft.Json;
using SteamReactProject.SteamAPI.Services;

namespace SteamReactProject.SteamAPI.Entities;

public class SteamUserStatus
{
    [JsonConstructor]
    public SteamUserStatus(
        [JsonProperty(nameof(SteamId))] string steamId,
        [JsonProperty(nameof(CommunityBanned))] bool communityBanned,
        [JsonProperty(nameof(VACBanned))] bool vacBanned,
        [JsonProperty("NumberOfVACBans")] int numberOfVACBans,
        [JsonProperty("DaysSinceLastBan")] int daysSinceLastBan,
        [JsonProperty("NumberOfGameBans")] int numberOfGameBans,
        [JsonProperty(nameof(EconomyBan))] string economyBan
    )
    {
        SteamId = steamId;
        CommunityBanned = communityBanned;
        VACBanned = vacBanned;
        VACBans = numberOfVACBans;
        LastBan = daysSinceLastBan;
        GameBans = numberOfGameBans;
        EconomyBan = economyBan;
    }

    [JsonProperty(nameof(SteamId))]
    public string SteamId { get; }

    [JsonProperty(nameof(CommunityBanned))]
    public bool CommunityBanned { get; }

    [JsonProperty(nameof(VACBanned))]
    public bool VACBanned { get; }

    [JsonProperty("NumberOfVACBans")]
    public int VACBans { get; }

    [JsonProperty("DaysSinceLastBan")]
    public int LastBan { get; }

    [JsonProperty("NumberOfGameBans")]
    public int GameBans { get; }

    [JsonProperty(nameof(EconomyBan))]
    public string EconomyBan { get; }

    public static IReadOnlyList<SteamUserStatus>? Deserialize(string json)
    {
        var instance = JsonConvert.DeserializeObject<RootSteamUserBanStatus>(json);
        return instance?.PlayersStatus;
    }
}

public class RootSteamUserBanStatus : IDeserialize
{
    [JsonConstructor]
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

