using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

var home = Environment.GetEnvironmentVariable("HOME");
var port = Environment.GetEnvironmentVariable("SERVERPORT") ?? "3001";
var key = Environment.GetEnvironmentVariable("STEAMAPIKEY") ?? "";
var connection = Environment.GetEnvironmentVariable("STEAM_DB") ?? "";

var appFolder = new DirectoryInfo(AppContext.BaseDirectory);
appFolder.CreateSubdirectory("data");

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = false;
    options.TimestampFormat = "[HH:mm:ss] ";
});

builder.Services.AddSingleton(sp => 
{
    var logger = sp.GetRequiredService<ILogger<SteamClient>>();
    return new SteamClient(key, logger);
});

builder.Services.AddDbContext<SteamContext>(options =>
{
    options.UseNpgsql(connection);
});

builder.Services.AddScoped<UsersRepository>();
builder.Services.AddScoped<ILogger<UsersRepository>, Logger<UsersRepository>>();

builder.Services.AddScoped<InventoriesRepository>();
builder.Services.AddScoped<ILogger<InventoriesRepository>, Logger<InventoriesRepository>>();

builder.Services.Configure<JsonSerializerOptions>(options =>
{
    options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.WriteIndented = true;
});

var options = new JsonSerializerOptions()
{
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    WriteIndented = true
};

var app = builder.Build();
var client = app.Services.GetRequiredService<SteamClient>();

app.MapGet("/steam/users", async ([FromServices] UsersRepository repo) =>
{
    return Results.Json(repo.GetAll(), options);
});

app.MapGet("/steam/users/{steamid}", async (HttpContext ctx, [FromServices] UsersRepository repo, string steamID) =>
{
    if (string.IsNullOrEmpty(steamID.ToString()))
    {
        return Results.BadRequest("SteamID not inserted");
    }

    object? result;
    if (!ulong.TryParse(steamID, out var id))
    {
        result = await repo.GetByNameId(steamID);
        return result is not null ? Results.Json(result, options) : Results.NotFound("Steam user not found");
    }

    if (steamID.ToString().Length != 17)
    {
        return Results.BadRequest("SteamID not valid");
    }

    result = await repo.GetById(id);
    return result is not null ? Results.Json(result, options) : Results.NotFound("Steam user not found");
});

app.MapGet("/steam/csgo/{steamid}", async (HttpContext ctx, [FromServices] InventoriesRepository repo, string steamID) =>
{
    if (string.IsNullOrEmpty(steamID.ToString()))
    {
        return Results.BadRequest("SteamID not inserted");
    }

    if (steamID.ToString().Length != 17)
    {
        return Results.BadRequest("SteamID not valid");
    }

    var id = ulong.Parse(steamID);
    
    try
    {
        var result = await repo.GetById(id);
        return result is not null ? Results.Json(result, options) : Results.NotFound("Steam user CSGO inventory not found");
    }
    catch (Exception e)
    {
        return Results.BadRequest(new
        {
            error = e.Message
        });
    }
});

app.MapGet("/steam/users/ban", async (ctx) =>
{
    var steamIds = ctx.Request.Query["steamids"].ToString();
    
    if (string.IsNullOrEmpty(steamIds))
    {
        // ctx.Response.StatusCode = 400;
        ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
        await ctx.Response.WriteAsync("SteamIDs not inserted");
        return;
    }

    var identifiers = steamIds.Split(',');
    var client = app.Services.GetRequiredService<SteamClient>();
    var status = await client.GetPlayersBansAsync(identifiers);

    if (status is null)
    {
        // ctx.Response.StatusCode = 404;
        ctx.Response.StatusCode = StatusCodes.Status404NotFound;
        await ctx.Response.WriteAsync("SteamID not found");
        return;
    }

    var serialized = JsonConvert.SerializeObject(status);
    ctx.Response.ContentType = "application/json";
    await ctx.Response.WriteAsync(serialized);
});

app.Run($"http://localhost:{port}");