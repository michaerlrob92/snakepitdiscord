using System.Reflection;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SnakePit.Discord.Bot.Application;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddUserSecrets(Assembly.GetExecutingAssembly())
    .AddCommandLine(args)
    .Build();

var services = new ServiceCollection()
    .AddLogging(logger => logger.AddConsole())
    .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
    .AddSingleton(new DiscordSocketConfig
    {
        GatewayIntents = GatewayIntents.MessageContent | GatewayIntents.Guilds | GatewayIntents.GuildMessages
    })
    .AddSingleton<DiscordSocketClient>()
    .AddSingleton<DiscordEventListener>()
    .BuildServiceProvider();

var client = services.GetRequiredService<DiscordSocketClient>();
var logger = services.GetRequiredService<ILogger<DiscordSocketClient>>();
client.Log += msg =>
{
    logger.LogInformation("{Message}", msg.ToString());
    return Task.CompletedTask;
};

var token = configuration["DiscordToken"];
await client.LoginAsync(TokenType.Bot, token);
await client.StartAsync();

var listener = services.GetRequiredService<DiscordEventListener>();
await listener.StartAsync();

await Task.Delay(-1);