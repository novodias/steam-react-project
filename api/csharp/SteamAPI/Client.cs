namespace SteamReactProject.SteamAPI;

public sealed class SteamClient
{
    private readonly EventId _id = new(1, "SteamClient");
    private ILogger? _logger;
    private HttpClient? _client;
    private string? _key;

    private readonly ISteamUserService SteamUserInterface = new();
    private readonly IPlayerService PlayerService = new();
    private readonly InventoryScraper InventoryScraper = new();

    // Not sure about this property;
    public string Key { get => _key ?? ""; }

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


    public async Task<object?> GetAsync<T>(Uri uri, CancellationToken token = default) where T : IDeserialize
    {
        if (_client is null)
            throw new NullReferenceException(nameof(_client));

        _logger?.LogInformation(_id, "Sending a request to: {AbsolutePath}", uri.AbsolutePath);
        _logger?.LogDebug(_id, "Sending a request to: {AbsoluteUri}", uri.AbsoluteUri);
        
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

            throw;
        }

        // return default;
    }

    public async Task<VanityUrlResponse?> ResolveVanityUrl(
        string name,
        FormatExtension formatExtension = FormatExtension.JSON,
        CancellationToken token = default
    )
    {
        var format = ConvertFormatExtension(formatExtension);

        var vanity = await SteamUserInterface.ResolveVanityUrlAsync(this, name, format, token);

        if (vanity is null)
        {
            return null;
        }

        return vanity;
    }

    public async Task<SteamUser?> GetPlayerSummariesAsync(
        ulong steamId, 
        FormatExtension formatExtension = FormatExtension.JSON, 
        CancellationToken token = default
    )
    {
        var format = ConvertFormatExtension(formatExtension);

        var players = await SteamUserInterface
            .GetPlayerSummariesAsync(this, steamId.ToString(), format, token);

        if (players is null)
        {
            return null;
        }

        return players[0];
    }

    public async Task<IList<SteamUser>?> GetPlayersSummariesAsync(
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

    public async Task<SteamUserStatus?> GetPlayerBansAsync(
        ulong steamID,
        FormatExtension formatExtension = FormatExtension.JSON,
        CancellationToken token = default
    )
    {
        var format = ConvertFormatExtension(formatExtension) 
            ?? throw new Exception("Format or Steam IDs is null");

        var status = await SteamUserInterface
            .GetPlayerBansAsync(this, steamID.ToString(), format, token);

        if (status is null || status.Count == 0)
        {
            return default;
        }

        return status[0];
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

    public async Task<SteamUserGames?> GetPlayerRecentGames(
        ulong steamID, 
        int count, 
        FormatExtension formatExtension = FormatExtension.JSON,
        CancellationToken token = default)
    {
        var format = ConvertFormatExtension(formatExtension)
            ?? throw new Exception("Format or Steam IDs is null");
        
        return await PlayerService
            .GetRecentlyPlayedGamesAsync(this, steamID.ToString(), count, format, token);
    }

    public async Task<Inventory?> GetInventoryAsync(ulong steamId, int appId, int count = 5000, string language = "english", CancellationToken token = default)
        => await InventoryScraper.GetInventoryAsync(this, steamId, appId, count, language, token);

    public void Dispose()
    {
        _client?.Dispose();
        _client = null;
        _logger = null;
        _key = null;
    }
}