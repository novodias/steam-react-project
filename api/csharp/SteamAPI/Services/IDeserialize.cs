namespace SteamReactProject.SteamAPI.Services;

public interface IDeserialize
{
    public static abstract object? Deserialize(string json);
}