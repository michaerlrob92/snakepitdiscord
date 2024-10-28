using System.Reflection;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SnakePit.Discord.Bot.Application;

var configuration = new ConfigurationBuilder()
    // Adds a JSON configuration file to the configuration builder.
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    // Adds environment variables to the configuration builder.
    .AddEnvironmentVariables()
    // Adds user secrets to the configuration builder.
    .AddUserSecrets(Assembly.GetExecutingAssembly())
    // Adds command line arguments to the configuration builder.
    .AddCommandLine(args)
    // Builds the configuration.
    .Build();

var services = new ServiceCollection()
    // Adds logging services to the service collection.
    .AddLogging(logger => logger.AddConsole())
    // Adds MediatR services to the service collection.
    .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
    // Adds a singleton service for DiscordSocketConfig.
    .AddSingleton(new DiscordSocketConfig
    {
        GatewayIntents = GatewayIntents.MessageContent | GatewayIntents.Guilds | GatewayIntents.GuildMessages
    })
    // Adds a singleton service for DiscordSocketClient.
    .AddSingleton<DiscordSocketClient>()
    // Adds a singleton service for DiscordEventListener.
    .AddSingleton<DiscordEventListener>()
    .AddSingleton<IConfiguration>(configuration)
    // Builds the service provider.
    .BuildServiceProvider();

var client = services.GetRequiredService<DiscordSocketClient>();
var logger = services.GetRequiredService<ILogger<DiscordSocketClient>>();
client.Log += msg =>
{
    // Logs a message from the Discord client.
    logger.LogInformation("{Message}", msg.ToString());
    return Task.CompletedTask;
};

var token = configuration["DiscordToken"];
// Logs in to the Discord client using the bot token.
await client.LoginAsync(TokenType.Bot, token);
// Starts the Discord client.
await client.StartAsync();

var listener = services.GetRequiredService<DiscordEventListener>();
// Starts the Discord event listener.
await listener.StartAsync();

// Keeps the application running indefinitely.
await Task.Delay(-1);