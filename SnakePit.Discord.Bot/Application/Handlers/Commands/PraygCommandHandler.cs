using Discord;
using MediatR;
using SnakePit.Discord.Bot.Application.Notifications;

namespace SnakePit.Discord.Bot.Application.Handlers.Commands;

/// <summary>
/// Handler for processing the prayg command.
/// </summary>
public class PraygCommandHandler : INotificationHandler<CommandReceivedNotification>
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

        // Check if the command is "!prayg"
        if (commandText != "!prayg")
        {
            return;
        }

        // Send a response message with an image
        await message.Channel.SendFileAsync(
            filePath: @"C:\Users\mikew\RiderProjects\snakepitdiscord\SnakePit.Discord.Bot\assets\prayg.png",
            text: $"Is that Channing Tatum? <@{Constants.PraygUserId}>",
            messageReference: new MessageReference(message.Id)
        );
    }
}