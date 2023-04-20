using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SteamReactProject.Models;

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