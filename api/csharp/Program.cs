using System.Text.Json;
using Newtonsoft.Json;
using SteamReactProject.SteamAPI;

var port = Environment.GetEnvironmentVariable("SERVERPORT") ?? "3001";
var key = Environment.GetEnvironmentVariable("STEAMAPIKEY") ?? "";

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddSimpleConsole(options =>
{
    // options.IncludeScopes = true;
    options.SingleLine = false;
    options.TimestampFormat = "[HH:mm:ss] ";
});

builder.Services.AddSingleton(sp => 
{
    var logger = sp.GetRequiredService<ILogger<SteamClient>>();
    return new SteamClient(key, logger);
});

var app = builder.Build();

app.MapGet("/steam/users", async (ctx) =>
{
    // var steamId = ctx.Request.RouteValues["steamid"]?.ToString();
    var steamIds = ctx.Request.Query["steamids"].ToString();

    if (string.IsNullOrEmpty(steamIds))
    {
        ctx.Response.StatusCode = 400;
        await ctx.Response.WriteAsync("SteamIDs not inserted");
        return;
    }

    var steamIdsList = steamIds.Split(',');
    var client = app.Services.GetRequiredService<SteamClient>();
    var player = await client.GetPlayersSummariesAsync(steamIdsList);

    if (player is null)
    {
        ctx.Response.StatusCode = 404;
        await ctx.Response.WriteAsync("SteamID not found");
        return;
    }

    var serialized = JsonConvert.SerializeObject(player);
    ctx.Response.ContentType = "application/json";
    await ctx.Response.WriteAsync(serialized);

    // await ctx.Response.WriteAsJsonAsync(player, new JsonSerializerOptions() 
    // {
    //     PropertyNamingPolicy = new JsonNamingPolicy()
    // });
});

app.Run($"http://localhost:{port}");