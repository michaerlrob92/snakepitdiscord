using MediatR;
using SnakePit.Discord.Bot.Application.Handlers.Authors;
using SnakePit.Discord.Bot.Application.Notifications;

namespace SnakePit.Discord.Bot.Application.Handlers.Commands;

/// <summary>
/// Handler for processing the flame command.
/// </summary>
public class FlameCommandHandler : INotificationHandler<CommandReceivedNotification>
{
    /// <summary>
    /// Handles the notification when a command is received.
    /// </summary>
    /// <param name="notification">The notification containing the command message.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Handle(CommandReceivedNotification notification, CancellationToken cancellationToken)
    {
        var message = notification.Message;
        var commandText = message.Content.Trim().ToLower();

        // Check if the command is "!flame"
        if (commandText != "!flame")
        {
            return;
        }

        // Send the flame message
        await FlameAuthorHandler.SendFlameMessage(message);
    }
}