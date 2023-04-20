using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SteamReactProject.SteamAPI.Entities;

public class SteamUser
{
    // Get AccountID by subtracting SteamID with this number: 76561197960265728  

    #pragma warning disable 8618
    public SteamUser()
    {
        
    }

    [Newtonsoft.Json.JsonConstructor]
    public SteamUser(
    #region Public Data Parameters
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
        [JsonProperty("personastateflags")] int personastateflags,
    #endregion

    #region Private Data Parameters
        [JsonProperty("realname")] string? realname,
        [JsonProperty("primaryclanid")] string? primaryclanid,
        [JsonProperty("timecreated")] long? timecreated,
        [JsonProperty("gameid")] int? gameid,
        [JsonProperty("gameserverip")] string? gameserverip,
        [JsonProperty("gameextrainfo")] string? gameextrainfo,
        [JsonProperty("cityid")] string? cityid,
        [JsonProperty("loccountrycode")] string? loccountrycode,
        [JsonProperty("locstatecode")] string? locstatecode,
        [JsonProperty("loccityid")] string? loccityid
    #endregion
    )
    {
        SteamId = ulong.Parse(steamid);
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
        PersonaStateFlags = personastateflags;

        RealName = realname;
        PrimaryClanId = primaryclanid;
        TimeCreated = timecreated;
        GameID = gameid;
        GameServerIP = gameserverip;
        GameExtraInfo = gameextrainfo;
        CityID = cityid;
        LocationCountryCode = loccountrycode;
        LocationStateCode = locstatecode;
        LocationCityID = loccityid;
    }

    public static SteamUser operator +(SteamUser oldEntity, SteamUser newEntity)
    {
        oldEntity.SteamId = newEntity.SteamId;
        oldEntity.CommunityVisibilityState = newEntity.CommunityVisibilityState;
        oldEntity.ProfileState = newEntity.ProfileState;
        oldEntity.PersonaName = newEntity.PersonaName;
        oldEntity.CommentPermission = newEntity.CommentPermission;
        oldEntity.ProfileUrl = newEntity.ProfileUrl;
        oldEntity.Avatar = newEntity.Avatar;
        oldEntity.AvatarMedium = newEntity.AvatarMedium;
        oldEntity.AvatarFull = newEntity.AvatarFull;
        oldEntity.AvatarHash = newEntity.AvatarHash;
        oldEntity.LastLogoff = newEntity.LastLogoff;
        oldEntity.PersonaState = newEntity.PersonaState;
        oldEntity.PersonaStateFlags = newEntity.PersonaStateFlags;
        oldEntity.RealName = newEntity.RealName;
        oldEntity.PrimaryClanId = newEntity.PrimaryClanId;
        oldEntity.TimeCreated = newEntity.TimeCreated;
        oldEntity.GameID = newEntity.GameID;
        oldEntity.GameServerIP = newEntity.GameServerIP;
        oldEntity.GameExtraInfo = newEntity.GameExtraInfo;
        oldEntity.CityID = newEntity.CityID;
        oldEntity.LocationCountryCode = newEntity.LocationCountryCode;
        oldEntity.LocationStateCode = newEntity.LocationStateCode;
        oldEntity.LocationCityID = newEntity.LocationCityID;
        oldEntity.LastUpdate = newEntity.LastUpdate;

        return oldEntity;
    }

    #region Public Data

    [JsonProperty("steamid"), JsonPropertyName("steamid"), Key]
    public ulong SteamId { get; set; }

    [JsonProperty("personaname"), JsonPropertyName("personaname")]
    public string PersonaName { get; set; }

    [JsonProperty("profileurl"), JsonPropertyName("profileurl")]
    public string ProfileUrl { get; set; }

    [JsonProperty("communityvisibilitystate"), JsonPropertyName("communityvisibilitystate")]
    public int CommunityVisibilityState { get; set; }

    [JsonProperty("profilestate"), JsonPropertyName("profilestate")]
    public int ProfileState { get; set; }

    [JsonProperty("commentpermission"), JsonPropertyName("commentpermission")]
    public int CommentPermission { get; set; }

    [JsonProperty("avatar"), JsonPropertyName("avatar")]
    public string Avatar { get; set; }

    [JsonProperty("avatarmedium"), JsonPropertyName("avatarmedium")]
    public string AvatarMedium { get; set; }

    [JsonProperty("avatarfull"), JsonPropertyName("avatarfull")]
    public string AvatarFull { get; set; }

    [JsonProperty("avatarhash"), JsonPropertyName("avatarhash")]
    public string AvatarHash { get; set; }

    [JsonProperty("lastlogoff"), JsonPropertyName("lastlogoff")]
    public int LastLogoff { get; set; }

    [JsonProperty("personastate"), JsonPropertyName("personastate")]
    public int PersonaState { get; set; }


    [JsonProperty("personastateflags"), JsonPropertyName("personastateflags")]
    public int PersonaStateFlags { get; set; }

    #endregion

    #region Private Data

    [JsonProperty("realname", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("realname")] 
    public string? RealName { get; set; }
    
    [JsonProperty("primaryclanid", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("primaryclanid")] 
    public string? PrimaryClanId { get; set; }
    
    [JsonProperty("timecreated", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("timecreated")] 
    public long? TimeCreated { get; set; }
    
    [JsonProperty("gameid", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("gameid")] 
    public int? GameID { get; set; }
    
    [JsonProperty("gameserverip", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("gameserverip")] 
    public string? GameServerIP { get; set; }
    
    [JsonProperty("gameextrainfo", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("gameextrainfo")] 
    public string? GameExtraInfo { get; set; }
    
    [JsonProperty("cityid", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("cityid")] 
    public string? CityID { get; set; }
    
    [JsonProperty("loccountrycode", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("loccountrycode")] 
    public string? LocationCountryCode { get; set; }
    
    [JsonProperty("locstatecode", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("locstatecode")] 
    public string? LocationStateCode { get; set; }
    
    [JsonProperty("loccityid", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("loccityid")] 
    public string? LocationCityID { get; set; }

    #endregion

    [JsonProperty("lastupdate", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("lastupdate")]
    public DateTime? LastUpdate { get; set; }

    public object? Deserialize(string json)
    {
        var instance = JsonConvert.DeserializeObject<RootSteamUsersSummaries>(json);
        return instance?.Response.Players;
    }
}

public class SteamUsersSummaries
{
    [Newtonsoft.Json.JsonConstructor]
    public SteamUsersSummaries(
        [JsonProperty("players")] List<SteamUser> players
    )
    {
        Players = players;
    }

    [JsonProperty("players"), JsonPropertyName("players")]
    public IList<SteamUser>? Players { get; }
}

public class RootSteamUsersSummaries : IDeserialize
{
    [Newtonsoft.Json.JsonConstructor]
    public RootSteamUsersSummaries(
        [JsonProperty("response")] SteamUsersSummaries response
    )
    {
        Response = response;
    }

    [JsonProperty("response"), JsonPropertyName("response")]
    public SteamUsersSummaries Response { get; }

    public static object? Deserialize(string json)
    {
        var instance = JsonConvert.DeserializeObject<RootSteamUsersSummaries>(json);
        return instance?.Response.Players;
    }
}

