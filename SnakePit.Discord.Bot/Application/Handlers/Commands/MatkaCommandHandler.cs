using Discord;
using MediatR;
using SnakePit.Discord.Bot.Application.Notifications;

namespace SnakePit.Discord.Bot.Application.Handlers.Commands;

/// <summary>
/// Handler for processing the matka command.
/// </summary>
public class MatkaCommandHandler : INotificationHandler<CommandReceivedNotification>
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

        // Check if the command is "!matka"
        if (commandText != "!matka")
        {
            return;
        }

        // Send a response message with an image
        await message.Channel.SendFileAsync(
            filePath: @"C:\Work\Apps\SnakePit\SnakePit.Discord\SnakePit.Discord.Bot\assets\matka.png",
            text: $"Looking good <@{Constants.MatkaUserId}>.",
            messageReference: new MessageReference(message.Id)
        );
    }
}