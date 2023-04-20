using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SteamReactProject.SteamAPI.Entities;

#pragma warning disable 8618

public class Action
{
    [JsonProperty("link", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("link")]
    public string Link { get; set; }

    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("name")]
    public string Name { get; set; }
}

public class Asset
{
    [JsonProperty("appid", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("appid")]
    public int? AppID { get; set; }

    [JsonProperty("contextid", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("contextid")]
    public string ContextID { get; set; }

    [JsonProperty("assetid", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("assetid")]
    public string AssetID { get; set; }

    [JsonProperty("classid", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("classid")]
    public string ClassID { get; set; }

    [JsonProperty("instanceid", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("instanceid")]
    public string InstanceID { get; set; }

    [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("amount")]
    public string Amount { get; set; }
}

public class ItemDescription
{
    [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("value")]
    public string Value { get; set; }

    [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("color")]
    public string? Color { get; set; }
}

public class Description
{
    [JsonProperty("appid", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("appid")]
    public int? AppID { get; set; }

    [JsonProperty("classid", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("classid")]
    public string ClassID { get; set; }

    [JsonProperty("instanceid", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("instanceid")]
    public string InstanceID { get; set; }

    [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("currency")]
    public int? Currency { get; set; }

    [JsonProperty("background_color", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("background_color")]
    public string BackgroundColor { get; set; }

    [JsonProperty("icon_url", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("icon_url")]
    public string IconUrl { get; set; }

    [JsonProperty("icon_url_large", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("icon_url_large")]
    public string IconUrlLarge { get; set; }

    [JsonProperty("descriptions", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("descriptions")]
    public List<ItemDescription> Descriptions { get; set; }

    [JsonProperty("tradable", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("tradable")]
    public int? Tradable { get; set; }

    [JsonProperty("actions", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("actions")]
    public List<Action> Actions { get; set; }

    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonProperty("name_color", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("name_color")]
    public string NameColor { get; set; }

    [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonProperty("market_name", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("market_name")]
    public string MarketName { get; set; }

    [JsonProperty("market_hash_name", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("market_hash_name")]
    public string MarketHashName { get; set; }

    [JsonProperty("market_actions", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("market_actions")]
    public List<MarketAction> MarketActions { get; set; }

    [JsonProperty("commodity", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("commodity")]
    public int? Commodity { get; set; }

    [JsonProperty("market_tradable_restriction", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("market_tradable_restriction")]
    public int? MarketTradableRestriction { get; set; }

    [JsonProperty("marketable", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("marketable")]
    public int? Marketable { get; set; }

    [JsonProperty("tags", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("tags")]
    public List<Tag> Tags { get; set; }

    [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("value")]
    public string Value { get; set; }

    [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("color")]
    public string Color { get; set; }
}

public class MarketAction
{
    [JsonProperty("link", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("link")]
    public string Link { get; set; }

    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("name")]
    public string Name { get; set; }
}

public class Inventory : IDeserialize
{
    [JsonProperty("assets", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("assets")]
    public List<Asset> Assets { get; set; }

    [JsonProperty("descriptions", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("descriptions")]
    public List<Description> Descriptions { get; set; }

    [JsonProperty("total_inventory_count", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("total_inventory_count")]
    public int? TotalInventoryCount { get; set; }

    [JsonProperty("success", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("success")]
    public int? Success { get; set; }

    [JsonProperty("rwgrsn", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("rwgrsn")]
    public int? Rwgrsn { get; set; }

    public static object? Deserialize(string json)
    {
        return JsonConvert.DeserializeObject<Inventory>(json);
    }
}

public class Tag
{
    [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("category")]
    public string Category { get; set; }

    [JsonProperty("internal_name", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("internal_name")]
    public string InternalName { get; set; }

    [JsonProperty("localized_category_name", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("localized_category_name")]
    public string LocalizedCategoryName { get; set; }

    [JsonProperty("localized_tag_name", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("localized_tag_name")]
    public string LocalizedTagName { get; set; }

    [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("color")]
    public string? Color { get; set; }
}

