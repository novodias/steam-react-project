namespace SteamReactProject.SteamAPI.Services;

public sealed class ISteamUserService : ServiceRequest
{
    public ISteamUserService()
    {
        _base = "api.steampowered.com";
        _interface = "ISteamUser";
    }

    [MethodConfiguration("ResolveVanityUrl", "v0001")]
    public async Task<VanityUrlResponse?> ResolveVanityUrlAsync(
        SteamClient client, 
        string vanityUrl, 
        string format = "json",
        CancellationToken token = default)
    {
        // var method = nameof(ResolveVanityUrl);
        // var version = "v0001";
        // var path = CreatePath(_interface, method, version);

        var path = CreatePath();
        var query = CreateQuery(
            new Query("key", client.Key),
            new Query("format", format),
            new Query("vanityurl", vanityUrl)
        );
        var uri = CreateUri(path, query);

        // This looks dumb;
        return await client.GetAsync<RootVanityUrl>(uri, token) as VanityUrlResponse;
    }

    [MethodConfiguration("GetPlayerSummaries", "v0002")]
    public async Task<IList<SteamUser>?> GetPlayerSummariesAsync(
        SteamClient client,
        string steamIds,
        string format = "json",
        CancellationToken token = default)
    {
        // var method = "GetPlayerSummaries";
        // var version = "v0002";
        // var path = CreatePath(_interface, method, version);

        var path = CreatePath();
        var query = CreateQuery(
            new Query("key", client.Key),
            new Query("format", format),
            new Query("steamids", steamIds)
        );
        var uri = CreateUri(path, query);

        var instance = await client.GetAsync<RootSteamUsersSummaries>(uri, token);

        return instance as IList<SteamUser>;
    }

    [MethodConfiguration("GetPlayerBans", "v0001")]
    public async Task<IReadOnlyList<SteamUserStatus>?> GetPlayerBansAsync(
        SteamClient client,
        string steamId,
        string format = "json",
        CancellationToken token = default)
    {
        // var method = "GetPlayerBans";
        // var version = "v0001";
        // var path = CreatePath(_interface, method, version);

        var path = CreatePath();
        var query = CreateQuery(
            new Query("key", client.Key),
            new Query("format", format),
            new Query("steamids", steamId)
        );
        var uri = CreateUri(path, query);

        var instance = await client.GetAsync<RootSteamUserBanStatus>(uri, token);

        return instance as IReadOnlyList<SteamUserStatus>;
    }
}
