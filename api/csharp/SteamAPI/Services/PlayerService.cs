using SteamReactProject.SteamAPI.Attributes;
using SteamReactProject.SteamAPI.Entities;

namespace SteamReactProject.SteamAPI.Services;

public class IPlayerService : ServiceRequest
{
    public IPlayerService(string baseAddress)
    {
        _base = baseAddress;
        _interface = "IPlayerService";
    }

    [MethodConfiguration("GetRecentlyPlayedGames", "v1")]
    public async Task<SteamUserGames?> GetRecentlyPlayedGamesAsync(
        SteamClient client, 
        string steamId, 
        int count, 
        CancellationToken token = default)
    {
        var serialized = SerializeAnonymousObject(
            new { steamid = steamId, count }
        );

        var path = CreatePath();
        var query = CreateQuery(
            new Query("key", client.Key),
            new Query("input_json", serialized)
        );
        var uri = CreateUri(path, query);

        var instance = await client.GetAsync<RootPlayedGames>(uri, token);
        return instance as SteamUserGames;
    }

    [MethodConfiguration("GetOwnedGames", "v1")]
    public async Task<SteamUserGames?> GetOwnedGames(
        SteamClient client, 
        string steamId, 
        bool? includeAppInfo = null, 
        bool? includePlayedFreeGames = null, 
        int? appIdsFilter = null,
        CancellationToken token = default)
    {
        var serialized = SerializeAnonymousObject(new
        {
            steamid = steamId,
            include_appinfo = includeAppInfo,
            include_played_free_games = includePlayedFreeGames,
            appids_filter = appIdsFilter
        });

        var path = CreatePath();
        var query = CreateQuery(
            new Query("key", client.Key),
            new Query("input_json", serialized)
        );
        var uri = CreateUri(path, query);

        var instance = await client.GetAsync<RootPlayedGames>(uri, token);
        return instance as SteamUserGames;
    }

    [MethodConfiguration("GetSteamLevel", "v1")]
    public async Task<SteamLevel?> GetSteamLevel(
        SteamClient client, 
        string steamId,
        CancellationToken token = default)
    {
        var serialized = SerializeAnonymousObject(new
        {
            steamid = steamId,
        });

        var path = CreatePath();
        var query = CreateQuery(
            new Query("key", client.Key),
            new Query("input_json", serialized)
        );

        var uri = CreateUri(path, query);

        var instance = await client.GetAsync<SteamLevel>(uri, token);
        return instance as SteamLevel;
    }
}