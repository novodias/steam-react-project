using SteamReactProject.SteamAPI.Entities;

namespace SteamReactProject.SteamAPI.Scrapers;

public class InventoryScraper : ServiceRequest
{
    /// <summary>
    /// Creates a scraper that can get any inventory that it is public.
    /// 
    /// (It is very important to store the inventory data somewhere,
    /// the endpoint allows a very little amount of requests.)
    /// </summary>
    public InventoryScraper()
    {
        _base = "https://steamcommunity.com/";
        _interface = "inventory";
    }

    /// <summary>
    /// All available languages.
    /// 
    /// https://partner.steamgames.com/doc/store/localization/languages
    /// </summary>
    private readonly static IReadOnlyList<string> Languages = new List<string>()
    {
        "brazilian", "english", "arabic", "bulgarian", "schinese", "tchinese",
        "czech", "danish", "dutch", "finnish", "french", "german", "greek",
        "hungarian", "italian", "japanese", "koreana", "norwegian", "polish",
        "portuguese", "romanian", "russian", "spaninsh", "latam", "swedish",
        "thai", "turkish", "ukrainian", "vietnamese"
    };

    private static string GetLanguage(string language)
    {
        var result = Languages.FirstOrDefault(l => l.StartsWith(language));

        if (result is not null)
        {
            return result;
        }

        result = Languages.FirstOrDefault(l => l.Contains(language));

        if (result is not null)
        {
            return result;
        }

        return Languages.Single(l => l == "english");
    }

    // TODO: Use query "start_assetid=[assetid]" to get more items
    // https://steamcommunity.com/inventory/XXXXXXXXXXXXXXXXX/730/2?l=english&count=5000&start_assetid=XXXXXXXXXX

    public async Task<Inventory?> GetInventoryAsync(SteamClient client, ulong steamID, 
        int appID, int count = 5000, string language = "english",
        CancellationToken token = default)
    {
        language = GetLanguage(language);

        var path = string.Format("{0}/{1}/{2}/{3}", _interface, steamID, appID, 2);
        var query = CreateQuery(
            new Query("l", language),
            new Query("count", count)
        );
        var uri = CreateUri(path, query);

        var inventory = await client.GetAsync<Inventory>(uri, token);
        return inventory as Inventory;
    }
}