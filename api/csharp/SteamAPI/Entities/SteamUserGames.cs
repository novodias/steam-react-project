using Newtonsoft.Json;
using SteamReactProject.SteamAPI.Services;

namespace SteamReactProject.SteamAPI.Entities;

public class RootPlayedGames : IDeserialize
{
    [JsonProperty("response")]
    public SteamUserGames Response { get; }

    public RootPlayedGames(
        [JsonProperty("response")] SteamUserGames response
    )
    {
        Response = response;
    }

    public static object? Deserialize(string json)
    {
        var instance = JsonConvert.DeserializeObject<RootPlayedGames>(json);
        return instance?.Response;
    }
}

public class SteamUserGames
{
    [JsonProperty("games")]
    public IReadOnlyList<Game> Games { get; }
    
    /// <summary>
    /// This value will not be null if the method is GetRecentlyPlayedGames
    /// </summary>
    [JsonProperty("total_count")]
    public int? TotalCount { get; }

    /// <summary>
    /// This value will not be null if the method is GetOwnedGames
    /// </summary>
    [JsonProperty("game_count")]
    public int? GameCount { get; }

    public SteamUserGames(
        [JsonProperty("games")] IReadOnlyList<Game> games,
        [JsonProperty("total_count")] int? totalCount,
        [JsonProperty("game_count")] int? gameCount
    )
    {
        Games = games;
        TotalCount = totalCount;
        GameCount = gameCount;
    }
}

public class Game
{
    [JsonProperty("appid"), JsonRequired] 
    public int AppId { get; }

    /// <summary>
    /// This value may be null in GetOwnedGames if set to true include_appinfo
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; }

    /// <summary>
    /// This value may be null in GetOwnedGames if set to true include_appinfo
    /// </summary>
    [JsonProperty("playtime_2weeks")] 
    public TimeSpan? PlaytimeTwoWeeks { get; }

    /// <summary>
    /// This value may be null in GetOwnedGames if set to true include_appinfo
    /// </summary>
    [JsonProperty("playtime_forever")] 
    public TimeSpan? PlaytimeForever { get; }

    /// <summary>
    /// This value may be null in GetOwnedGames if set to true include_appinfo
    /// </summary>
    [JsonProperty("img_icon_url")] 
    public string? ImageIconHash { get; }

    /// <summary>
    /// This value may be null in GetOwnedGames if set to true include_appinfo
    /// </summary>
    [JsonProperty("img_logo_url")] 
    public string? ImageLogoHash { get; }

    /// <summary>
    /// This value may be null in GetOwnedGames if set to true include_appinfo
    /// 
    /// Not available in the method GetRecentlyPlayedGames
    /// </summary>
    [JsonProperty("has_community_visible_stats")] 
    public int? HasCommunityVisibleStats { get; }

    public string? ImageIconUrl =>
        "https://media.steampowered.com/steamcommunity/public/images/apps/" +
        AppId + "/" + ImageIconHash + ".jpg";

    public string? ImageLogoUrl =>
        "https://media.steampowered.com/steamcommunity/public/images/apps/" +
        AppId + "/" + ImageLogoHash + ".jpg";

    public string HeaderUrl { get; }

    public Game(
        [JsonProperty("appid")] int appid, 
        [JsonProperty("name")] string? name,
        [JsonProperty("playtime_2weeks")] int? playtimeTwoWeeks, 
        [JsonProperty("playtime_forever")] int? playtimeForever,
        [JsonProperty("img_icon_url")] string? imageIcon, 
        [JsonProperty("img_logo_url")] string? imageLogo,
        [JsonProperty("has_community_visible_stats")] int? hasCommunityVisibleStats
    )
    {
        AppId = appid;
        Name = name;
        PlaytimeTwoWeeks = TimeSpan.FromMinutes(playtimeTwoWeeks.GetValueOrDefault());
        PlaytimeForever = TimeSpan.FromMinutes(playtimeForever.GetValueOrDefault());
        ImageIconHash = imageIcon;
        ImageLogoHash = imageLogo;
        HasCommunityVisibleStats = hasCommunityVisibleStats;
        HeaderUrl = "https://cdn.akamai.steamstatic.com/steam/apps/" + AppId + "/header.jpg";
    }
}