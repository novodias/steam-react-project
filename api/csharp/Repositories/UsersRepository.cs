namespace SteamReactProject.Repositories;

public class UsersRepository
{
    private readonly SteamClient _client;
    private readonly SteamContext _context;
    private ILogger<UsersRepository>? _logger;

    public UsersRepository(SteamClient client, SteamContext context, ILogger<UsersRepository>? logger = null)
    {
        _client = client;
        _context = context;
        _logger = logger;
    }

    private static bool VerifyLastUpdate(DateTime lastUpdate, int minimumHours)
    {
        return DateTime.Now.Subtract(lastUpdate).TotalHours > minimumHours;
    }

    private bool IsVanityUrlEqualTo(SteamUser s, string name)
    {
        var array = s.ProfileUrl.Split('/');
        
        // We take the second value of the array since
        // the url has a '/' at the end.
        // Taking the last would result in a empty string
        var id = array[Index.FromEnd(2)];

        _logger?.LogDebug("[UsersRepository/SteamContext] Searching name: {name}", id);

        if (id is not null && id == name)
        {
            return true;
        }

        return false;
    }

    public async Task<SteamUser?> GetByNameId(string name)
    {
        var collection = _context.SteamUsers
            .Where(s => s.ProfileUrl.ToLower().Contains(name.ToLower()))
            .ToList();

        var result = collection.FirstOrDefault(s => IsVanityUrlEqualTo(s, name));

        if (result is not null && !VerifyLastUpdate(result.LastUpdate!.Value, 5))
        {
            _logger?.LogInformation("[SteamUsers/{MethodName}] Found steam user in database", nameof(GetByNameId));
            return result;
        }

        _logger?.LogInformation("[SteamUsers/{MethodName}] Not found steam user in database", nameof(GetByNameId));
        var vanity = await _client.ResolveVanityUrl(name);

        if (vanity is null) 
        {
            return default;
        }

        if (vanity.Success != true)
        {
            return default;
        }

        // We try again
        result = await GetById(vanity.SteamId!.Value);
        return result;
    }

    public async Task<SteamUser?> GetById(ulong id)
    {
        // var result = await _context.FindAsync<SteamUser>(id);
        var result = await _context.FindUserAsync(id);

        if (result is not null && !VerifyLastUpdate(result.LastUpdate!.Value, 5))
        {
            _logger?.LogInformation("[SteamUsers/{MethodName}] Found steam user in database", nameof(GetById));
            return result;
        }

        _logger?.LogInformation("[SteamUsers/{MethodName}] Not found steam user in database", nameof(GetById));
        var updated = await _client.GetPlayerSummariesAsync(id);

        if (updated is null) 
        {
            return default;
        }

        updated = await _context.AddOrUpdateAsync(updated!);
        return updated;
    }

    public IReadOnlyList<SteamUser>? GetAll()
    {
        return _context.SteamUsers
            .OrderBy(u => u.LastUpdate)
            .Take(5000)
            .ToList()
            .AsReadOnly();
    }

    public void Dispose()
    {
        _client.Dispose();
        _context.Dispose();
        _logger = null;
    }
}