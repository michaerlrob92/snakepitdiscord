using Discord.WebSocket;
using MediatR;

namespace SnakePit.Discord.Bot.Application.Notifications;

/// <summary>
/// Notification that is published when a message author is identified.
/// </summary>
/// <param name="message">The received socket message.</param>
public class MessageAuthorNotification(SocketMessage message) : INotification
{
    /// <summary>
    /// The received socket message.
    /// </summary>
    public readonly SocketMessage Message = message;
}