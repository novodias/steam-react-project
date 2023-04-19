using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SteamReactProject.SteamAPI.Services;

namespace SteamReactProject.SteamAPI.Entities;

/// <summary>
/// CSGO Inventory
/// </summary>
public class CSGOInventory
{

    [Key] 
    public required ulong SteamID { get; set; }

    [Required]
    public required bool IsPublic { get; set; }

    [Required]
    public required DateTime? LastUpdate { get; set; }

    public Inventory? Inventory { get; set; }
}

public class InventoryConfiguration : IEntityTypeConfiguration<CSGOInventory>
{
    public void Configure(EntityTypeBuilder<CSGOInventory> builder)
    {
        builder.Property(e => e.Inventory).HasConversion(
            v => JsonConvert.SerializeObject(v),
            v => JsonConvert.DeserializeObject<Inventory>(v)
        );
    }
}

public class Action
{
    [JsonProperty("link", NullValueHandling = NullValueHandling.Ignore)]
    public string Link { get; set; }

    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }
}

public class Asset
{
    [JsonProperty("appid", NullValueHandling = NullValueHandling.Ignore)]
    public int? AppID { get; set; }

    [JsonProperty("contextid", NullValueHandling = NullValueHandling.Ignore)]
    public string ContextID { get; set; }

    [JsonProperty("assetid", NullValueHandling = NullValueHandling.Ignore)]
    public string AssetID { get; set; }

    [JsonProperty("classid", NullValueHandling = NullValueHandling.Ignore)]
    public string ClassID { get; set; }

    [JsonProperty("instanceid", NullValueHandling = NullValueHandling.Ignore)]
    public string InstanceID { get; set; }

    [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
    public string Amount { get; set; }
}

public class ItemDescription
{
    [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
    public string Type { get; set; }

    [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
    public string Value { get; set; }

    [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
    public string? Color { get; set; }
}

public class Description
{
    [JsonProperty("appid", NullValueHandling = NullValueHandling.Ignore)]
    public int? AppID { get; set; }

    [JsonProperty("classid", NullValueHandling = NullValueHandling.Ignore)]
    public string ClassID { get; set; }

    [JsonProperty("instanceid", NullValueHandling = NullValueHandling.Ignore)]
    public string InstanceID { get; set; }

    [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
    public int? Currency { get; set; }

    [JsonProperty("background_color", NullValueHandling = NullValueHandling.Ignore)]
    public string BackgroundColor { get; set; }

    [JsonProperty("icon_url", NullValueHandling = NullValueHandling.Ignore)]
    public string IconUrl { get; set; }

    [JsonProperty("icon_url_large", NullValueHandling = NullValueHandling.Ignore)]
    public string IconUrlLarge { get; set; }

    [JsonProperty("descriptions", NullValueHandling = NullValueHandling.Ignore)]
    public List<ItemDescription> Descriptions { get; set; }

    [JsonProperty("tradable", NullValueHandling = NullValueHandling.Ignore)]
    public int? Tradable { get; set; }

    [JsonProperty("actions", NullValueHandling = NullValueHandling.Ignore)]
    public List<Action> Actions { get; set; }

    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    [JsonProperty("name_color", NullValueHandling = NullValueHandling.Ignore)]
    public string NameColor { get; set; }

    [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
    public string Type { get; set; }

    [JsonProperty("market_name", NullValueHandling = NullValueHandling.Ignore)]
    public string MarketName { get; set; }

    [JsonProperty("market_hash_name", NullValueHandling = NullValueHandling.Ignore)]
    public string MarketHashName { get; set; }

    [JsonProperty("market_actions", NullValueHandling = NullValueHandling.Ignore)]
    public List<MarketAction> MarketActions { get; set; }

    [JsonProperty("commodity", NullValueHandling = NullValueHandling.Ignore)]
    public int? Commodity { get; set; }

    [JsonProperty("market_tradable_restriction", NullValueHandling = NullValueHandling.Ignore)]
    public int? MarketTradableRestriction { get; set; }

    [JsonProperty("marketable", NullValueHandling = NullValueHandling.Ignore)]
    public int? Marketable { get; set; }

    [JsonProperty("tags", NullValueHandling = NullValueHandling.Ignore)]
    public List<Tag> Tags { get; set; }

    [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
    public string Value { get; set; }

    [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
    public string Color { get; set; }
}

public class MarketAction
{
    [JsonProperty("link", NullValueHandling = NullValueHandling.Ignore)]
    public string Link { get; set; }

    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }
}

public class Inventory : IDeserialize
{
    [JsonProperty("assets", NullValueHandling = NullValueHandling.Ignore)]
    public List<Asset> Assets { get; set; }

    [JsonProperty("descriptions", NullValueHandling = NullValueHandling.Ignore)]
    public List<Description> Descriptions { get; set; }

    [JsonProperty("total_inventory_count", NullValueHandling = NullValueHandling.Ignore)]
    public int? TotalInventoryCount { get; set; }

    [JsonProperty("success", NullValueHandling = NullValueHandling.Ignore)]
    public int? Success { get; set; }

    [JsonProperty("rwgrsn", NullValueHandling = NullValueHandling.Ignore)]
    public int? Rwgrsn { get; set; }

    public static object? Deserialize(string json)
    {
        return JsonConvert.DeserializeObject<Inventory>(json);
    }
}

public class Tag
{
    [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
    public string Category { get; set; }

    [JsonProperty("internal_name", NullValueHandling = NullValueHandling.Ignore)]
    public string InternalName { get; set; }

    [JsonProperty("localized_category_name", NullValueHandling = NullValueHandling.Ignore)]
    public string LocalizedCategoryName { get; set; }

    [JsonProperty("localized_tag_name", NullValueHandling = NullValueHandling.Ignore)]
    public string LocalizedTagName { get; set; }

    [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
    public string? Color { get; set; }
}

