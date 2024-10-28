using Discord.WebSocket;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SnakePit.Discord.Bot.Application.Notifications;

namespace SnakePit.Discord.Bot.Application;

/// <summary>
/// Listens to Discord events and publishes notifications.
/// </summary>
public class DiscordEventListener(DiscordSocketClient client, IServiceScopeFactory serviceScope)
{
    /// <summary>
    /// Token to monitor for cancellation requests.
    /// </summary>
    private readonly CancellationToken _cancellationToken = new();

    /// <summary>
    /// Gets the IMediator instance from the service scope.
    /// </summary>
    private IMediator Mediator => serviceScope.CreateScope().ServiceProvider.GetRequiredService<IMediator>();

    /// <summary>
    /// Starts listening to Discord events.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task StartAsync()
    {
        client.MessageReceived += OnMessageReceivedAsync;

        await Task.CompletedTask;
    }

    /// <summary>
    /// Handles the MessageReceived event from the Discord client.
    /// </summary>
    /// <param name="message">The received message.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private Task OnMessageReceivedAsync(SocketMessage message)
    {
        return Mediator.Publish(new MessageReceivedNotification(message), _cancellationToken);
    }
}