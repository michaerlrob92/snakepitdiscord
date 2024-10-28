using Discord.WebSocket;
using MediatR;

namespace SnakePit.Discord.Bot.Application.Notifications;

public class CommandReceivedNotification(SocketMessage message) : INotification
{
    public readonly SocketMessage Message = message;
}