using Discord.WebSocket;
using MediatR;

namespace SnakePit.Discord.Bot.Application.Notifications;

public class MessageReceivedNotification(SocketMessage message) : INotification
{
    public readonly SocketMessage Message = message;
}