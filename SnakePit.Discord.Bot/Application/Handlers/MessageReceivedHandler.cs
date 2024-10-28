using MediatR;
using SnakePit.Discord.Bot.Application.Notifications;

namespace SnakePit.Discord.Bot.Application.Handlers;

/// <summary>
/// Handler for processing received messages.
/// </summary>
public class MessageReceivedHandler(IMediator mediator) : INotificationHandler<MessageReceivedNotification>
{
    /// <summary>
    /// Handles the notification when a message is received.
    /// </summary>
    /// <param name="notification">The notification containing the received message.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Handle(MessageReceivedNotification notification, CancellationToken cancellationToken)
    {
        var message = notification.Message;

        // Ignore messages from bots
        if (message.Author.IsBot)
        {
            return;
        }

        var messageContent = message.Content.Trim().ToLower();

        // Check if the message starts with '!' indicating a command
        if (messageContent.StartsWith('!'))
        {
            await mediator.Publish(new CommandReceivedNotification(message), cancellationToken);
            return;
        }

        // Publish a notification for messages from specific authors
        await mediator.Publish(new MessageAuthorNotification(message), cancellationToken);
    }
}