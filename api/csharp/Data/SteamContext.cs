using Microsoft.EntityFrameworkCore;
using SteamReactProject.SteamAPI.Entities;
using Newtonsoft.Json;

namespace SteamReactProject.Data;

public class SteamContext : DbContext
{
    #pragma warning disable 8618
    public SteamContext(DbContextOptions<SteamContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<SteamUser> SteamUsers { get; set; }
    public DbSet<CSGOInventory> CSGOUserInventories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new InventoryConfiguration());

        // modelBuilder.Entity<SteamUser>().ToTable("SteamUsers");
        // modelBuilder.Entity<CSGOInventory>().ToTable("CSGOInventories");
    }

    public async Task<SteamUser?> FindUserAsync(ulong id)
        => await SteamUsers.FindAsync(id);

    public async Task<CSGOInventory?> FindInventoryAsync(ulong id)
        => await CSGOUserInventories.FindAsync(id);

    public async Task<SteamUser> AddOrUpdateAsync(SteamUser entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        entity.LastUpdate = DateTime.UtcNow;

        var local = await SteamUsers.FindAsync(entity.SteamId);

        if (local == null)
        {
            local = (await SteamUsers.AddAsync(entity)).Entity;
        } 
        else 
        {
            local += entity;
        }

        await SaveChangesAsync();
        return local;
    }

    public async Task<CSGOInventory> AddOrUpdateAsync(Inventory entity, ulong steamId)
    {
        var local = await FindInventoryAsync(steamId);

        if (local == null)
        {
            var newEntity = new CSGOInventory()
            {
                SteamID = steamId,
                IsPublic = entity is not null,
                Inventory = entity,
                LastUpdate = DateTime.UtcNow
            };

            local = (await CSGOUserInventories.AddAsync(newEntity)).Entity;
        }
        else 
        {
            local.Inventory = entity;
            local.IsPublic = entity is not null;
            local.LastUpdate = DateTime.UtcNow;
        }

        await SaveChangesAsync();
        return local;
    }

    public async Task<IReadOnlyList<SteamUser>> AddOrUpdateAsync(IList<SteamUser> entities)
    {
        if (entities == null)
        {
            throw new ArgumentNullException(nameof(entities));
        }

        var now = DateTime.Now;
        foreach (var entity in entities)
        {
            entity.LastUpdate = now;
        }
        
        UpdateRange(entities);
        await SaveChangesAsync();

        return entities.AsReadOnly();
    }

    public async Task<IReadOnlyList<SteamUser>?> GetSteamUsers(
        IEnumerable<string> ids)
    {        
        var usersModels = SteamUsers
            .Where(u => ids.Contains(u.SteamId.ToString()));
        
        var users = await usersModels.ToListAsync();

        return users.AsReadOnly();
    }
}