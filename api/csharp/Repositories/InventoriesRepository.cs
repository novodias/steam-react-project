namespace SteamReactProject.Repositories;

public class InventoriesRepository
{
    private readonly SteamClient _client;
    private readonly SteamContext _context;
    private ILogger<InventoriesRepository>? _logger;

    public InventoriesRepository(SteamClient client, SteamContext context, ILogger<InventoriesRepository>? logger = null)
    {
        _client = client;
        _context = context;
        _logger = logger;
    }

    private static bool VerifyLastUpdate(DateTime lastUpdate, int minimumHours)
    {
        return DateTime.Now.Subtract(lastUpdate).TotalHours > minimumHours;
    }

    public async Task<CSGOInventory?> GetById(ulong id)
    {
        var result = await _context.FindInventoryAsync(id);

        if (result is not null && !VerifyLastUpdate(result.LastUpdate!.Value, 12))
        {
            _logger?.LogInformation("[CSGOInventory/{MethodName}] Found steam user inventory in database", nameof(GetById));
            return result;
        }

        _logger?.LogInformation("[CSGOInventory/{MethodName}] Not found steam user inventory in database", nameof(GetById));
        var updated = await _client.GetInventoryAsync(id, 730);

        if (updated is null) 
        {
            return default;
        }

        result = await _context.AddOrUpdateAsync(updated, id);
        return result;
    }

    public void Dispose()
    {
        _context.Dispose();
        _client.Dispose();
        _logger = null;
    }
}