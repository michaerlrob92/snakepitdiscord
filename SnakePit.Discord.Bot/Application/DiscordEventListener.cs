using Discord.WebSocket;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SnakePit.Discord.Bot.Application.Notifications;

namespace SnakePit.Discord.Bot.Application;

public class DiscordEventListener(DiscordSocketClient client, IServiceScopeFactory serviceScope)
{
    private readonly CancellationToken cancellationToken = new();
    
    private IMediator Mediator => serviceScope.CreateScope().ServiceProvider.GetRequiredService<IMediator>();
    
    public async Task StartAsync()
    {
        client.MessageReceived += OnMessageReceivedAsync;

        await Task.CompletedTask;
    }

    private Task OnMessageReceivedAsync(SocketMessage message)
    {
        return Mediator.Publish(new MessageReceivedNotification(message), cancellationToken);
    }
}