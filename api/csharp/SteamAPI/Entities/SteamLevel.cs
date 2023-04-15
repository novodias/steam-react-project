using Newtonsoft.Json;
using SteamReactProject.SteamAPI.Services;

namespace SteamReactProject.SteamAPI.Entities;

public class SteamLevel : IDeserialize
{
    [JsonProperty("player_level")]
    public int Level { get; }

    public SteamLevel([JsonProperty("player_level")] int playerLevel)
    {
        Level = playerLevel;
    }

    public static object? Deserialize(string json)
    { 
        var instance = JsonConvert.DeserializeObject<SteamLevel>(json);
        return instance;
    }
}