using Newtonsoft.Json;
using SteamReactProject.SteamAPI.Services;

namespace SteamReactProject.SteamAPI.Entities;

public class RootVanityUrl : IDeserialize
{
    [JsonProperty("response")]
    public VanityUrlResponse Response { get; }

    public RootVanityUrl(
        [JsonProperty("response")] VanityUrlResponse response
    )
    {
        Response = response;
    }

    public static object? Deserialize(string json)
    {
        var instance = JsonConvert.DeserializeObject<RootVanityUrl>(json);
        return instance?.Response;
    }
}

public class VanityUrlResponse
{
    public VanityUrlResponse(
        [JsonProperty("success")] int success,
        [JsonProperty("steamid")] ulong? steamid,
        [JsonProperty("message")] string? message
    )
    {
        _success = success;
        SteamId = steamid;
        Message = message;
    }

    [JsonProperty("success")] 
    private readonly int _success;

    public bool Success => _success == 1;

    [JsonProperty("steamid")] 
    public ulong? SteamId { get; }

    [JsonProperty("message")]
    public string? Message { get; }

    public static VanityUrlResponse? Deserialize(string json)
    {
        var instance = JsonConvert.DeserializeObject<RootVanityUrl>(json);
        return instance?.Response;
    }
}