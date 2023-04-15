using Newtonsoft.Json;
using SteamReactProject.SteamAPI.Services;

namespace SteamReactProject.SteamAPI.Entities;

public class SteamUser
{
    [JsonConstructor]
    public SteamUser(
        [JsonProperty("steamid")] string steamid,
        [JsonProperty("communityvisibilitystate")] int communityvisibilitystate,
        [JsonProperty("profilestate")] int profilestate,
        [JsonProperty("personaname")] string personaname,
        [JsonProperty("commentpermission")] int commentpermission,
        [JsonProperty("profileurl")] string profileurl,
        [JsonProperty("avatar")] string avatar,
        [JsonProperty("avatarmedium")] string avatarmedium,
        [JsonProperty("avatarfull")] string avatarfull,
        [JsonProperty("avatarhash")] string avatarhash,
        [JsonProperty("lastlogoff")] int lastlogoff,
        [JsonProperty("personastate")] int personastate,
        [JsonProperty("primaryclanid")] string primaryclanid,
        [JsonProperty("timecreated")] int timecreated,
        [JsonProperty("personastateflags")] int personastateflags
    )
    {
        SteamId = steamid;
        CommunityVisibilityState = communityvisibilitystate;
        ProfileState = profilestate;
        PersonaName = personaname;
        CommentPermission = commentpermission;
        ProfileUrl = profileurl;
        Avatar = avatar;
        AvatarMedium = avatarmedium;
        AvatarFull = avatarfull;
        AvatarHash = avatarhash;
        LastLogoff = lastlogoff;
        PersonaState = personastate;
        PrimaryClanId = primaryclanid;
        TimeCreated = timecreated;
        PersonaStateFlags = personastateflags;
    }

    [JsonProperty("steamid")]
    public string SteamId { get; }

    [JsonProperty("communityvisibilitystate")]
    public int CommunityVisibilityState { get; }

    [JsonProperty("profilestate")]
    public int ProfileState { get; }

    [JsonProperty("personaname")]
    public string PersonaName { get; }

    [JsonProperty("commentpermission")]
    public int CommentPermission { get; }

    [JsonProperty("profileurl")]
    public string ProfileUrl { get; }

    [JsonProperty("avatar")]
    public string Avatar { get; }

    [JsonProperty("avatarmedium")]
    public string AvatarMedium { get; }

    [JsonProperty("avatarfull")]
    public string AvatarFull { get; }

    [JsonProperty("avatarhash")]
    public string AvatarHash { get; }

    [JsonProperty("lastlogoff")]
    public int LastLogoff { get; }

    [JsonProperty("personastate")]
    public int PersonaState { get; }

    [JsonProperty("primaryclanid")]
    public string PrimaryClanId { get; }

    [JsonProperty("timecreated")]
    public int TimeCreated { get; }

    [JsonProperty("personastateflags")]
    public int PersonaStateFlags { get; }

    public object? Deserialize(string json)
    {
        var instance = JsonConvert.DeserializeObject<RootSteamUsersSummaries>(json);
        return instance?.Response.Players;
    }
}

public class SteamUsersSummaries
{
    [JsonConstructor]
    public SteamUsersSummaries(
        [JsonProperty("players")] List<SteamUser> players
    )
    {
        Players = players;
    }

    [JsonProperty("players")]
    public IReadOnlyList<SteamUser>? Players { get; }
}

public class RootSteamUsersSummaries : IDeserialize
{
    [JsonConstructor]
    public RootSteamUsersSummaries(
        [JsonProperty("response")] SteamUsersSummaries response
    )
    {
        Response = response;
    }

    [JsonProperty("response")]
    public SteamUsersSummaries Response { get; }

    public static object? Deserialize(string json)
    {
        var instance = JsonConvert.DeserializeObject<RootSteamUsersSummaries>(json);
        return instance?.Response.Players;
    }
}

