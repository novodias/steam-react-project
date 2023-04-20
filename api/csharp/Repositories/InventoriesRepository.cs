using SteamReactProject.Models;

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

    private bool VerifyLastUpdate(DateTime lastUpdate, int minimumHours)
    {
        var sub = DateTime.UtcNow.Subtract(lastUpdate).TotalHours;
        var result = sub > minimumHours;

        _logger?.LogDebug("[CSGOInventory/VerifyLastUpdate] Last: {LastUpdate}, Now: {Now}, ResultTH: {sub}, bool: {Bool}",
            lastUpdate.ToString(), DateTime.UtcNow, sub, result);

        return result;
    }

    public async Task<CSGOInventory?> GetById(ulong id)
    {
        var result = await _context.FindInventoryAsync(id);

        if (result is not default(CSGOInventory) && !VerifyLastUpdate(result.LastUpdate!.Value, 12))
        {
            _logger?.LogInformation("[CSGOInventory/{MethodName}] Found steam user inventory in database", nameof(GetById));
            return result;
        }

        if (result is null)
        {
            _logger?.LogInformation("[CSGOInventory/{MethodName}] Not found steam user inventory in database", nameof(GetById));
        }
        else 
        {
            _logger?.LogInformation("[CSGOInventory/{MethodName}] CSGO Inventory entity not updated", nameof(GetById));
        }

        Inventory? updated;
        try
        {
            updated = await _client.GetInventoryAsync(id, 730);
        }
        catch (Exception)
        {
            updated = null;
        }

        if (updated is null) 
        {
            // TODO: If database entity exists, send that instead.
            _logger?.LogInformation("[CSGOInventory/{MethodName}] Request not successful - Sending local entity instead", nameof(GetById));
            return result is not null ? result : default;

            // return default;
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