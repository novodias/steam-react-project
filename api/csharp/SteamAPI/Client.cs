using System.Reflection;
using SteamReactProject.SteamAPI.Services;
using SteamReactProject.SteamAPI.Entities;

namespace SteamReactProject.SteamAPI;

public sealed class SteamClient
{
    private readonly EventId _id = new(1, "SteamClient");
    private readonly ILogger? _logger;
    private readonly HttpClient _client;
    private readonly string _key;

    private readonly ISteamUserService SteamUserInterface = new("api.steampowered.com");

    // Not sure about this property;
    public string Key { get => _key; }

    public SteamClient(string key, ILogger? logger = null) 
    {
        _key = ValidateKey(key);
        _client = new HttpClient();
        _logger = logger;
    }

    private static string ValidateKey(string key)
    {
        if (key.Length != 32)
        {
            throw new Exception("The key is not 32 characters long");
        }

        return key;
    }

    private static string ConvertFormatExtension(FormatExtension format)
        => format.ToString().ToLower();

    private static string JoinSteamIds(IEnumerable<string> steamIds)
        => string.Join(',', steamIds);

    // private object? Deserialize(IDeserialize deserializer, string json)
    // {
    //     // var type = typeof(IDeserialize);
    //     // var method = type.GetMethod("Deserialize", BindingFlags.Static);
    //     // var instance = (T?)method?.Invoke(null, new object[] { json });

    //     var instance = deserializer.Deserialize(json);

    //     if (instance is null)
    //     {
    //         _logger?.LogDebug(_id, "Instance didn't deserialize {json}", json);
    //     }

    //     return instance;
    // }

    public async Task<object?> GetAsync<T>(Uri uri, CancellationToken token = default) where T : IDeserialize
    {
        _logger?.LogInformation(_id, "Sending a request to: {AbsolutePath}", uri.AbsolutePath);
        
        try
        {
            using var response = await _client.GetAsync(uri, token);
            response.EnsureSuccessStatusCode();

            _logger?.LogInformation(_id, "Request to {AbsolutePath} successful", uri.AbsolutePath);
            
            var json = await response.Content.ReadAsStringAsync(token);
            var instance = T.Deserialize(json);

            return instance;
        }
        catch (Exception e)
        {
            if (e is HttpRequestException requestException)
            {
                _logger?.LogError(_id, requestException, "Request was not successful");
            }
            else if (e is TaskCanceledException taskCanceledException)
            {
                _logger?.LogError(_id, taskCanceledException, "Task was canceled");
            }
            else
            {
                throw;
            }
        }

        return default;
    }

    public async Task<SteamUser?> GetPlayerSummariesAsync(
        string steamId, 
        FormatExtension formatExtension = FormatExtension.JSON, 
        CancellationToken token = default
    )
    {
        var format = ConvertFormatExtension(formatExtension) ?? 
            throw new Exception("Format or Steam IDs is null");

        var players = await SteamUserInterface
            .GetPlayerSummariesAsync(this, steamId, format, token);

        if (players is null)
        {
            return null;
        }

        return players[0];
    }

    public async Task<IReadOnlyList<SteamUser>?> GetPlayersSummariesAsync(
        IEnumerable<string> steamIds, 
        FormatExtension formatExtension = FormatExtension.JSON, 
        CancellationToken token = default
    )
    {
        var format = ConvertFormatExtension(formatExtension);
        var ids = JoinSteamIds(steamIds);

        if (ids is null || format is null)
        {
            throw new Exception("Format or Steam IDs is null");
        }

        return await SteamUserInterface
            .GetPlayerSummariesAsync(this, ids, format, token);
    }

    public async Task<IReadOnlyList<SteamUserStatus>?> GetPlayersBansAsync(
        IEnumerable<string> steamIds,
        FormatExtension formatExtension = FormatExtension.JSON,
        CancellationToken token = default
    )
    {
        var format = ConvertFormatExtension(formatExtension);
        var ids = JoinSteamIds(steamIds);

        if (ids is null || format is null)
        {
            throw new Exception("Format or Steam IDs is null");
        }

        return await SteamUserInterface
            .GetPlayerBansAsync(this, ids, format, token);
    }
}