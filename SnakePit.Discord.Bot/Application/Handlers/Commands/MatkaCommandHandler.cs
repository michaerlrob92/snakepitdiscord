using Discord;
using MediatR;
using SnakePit.Discord.Bot.Application.Notifications;

namespace SnakePit.Discord.Bot.Application.Handlers.Commands;

public class MatkaCommandHandler : INotificationHandler<CommandReceivedNotification>
{
    public async Task Handle(CommandReceivedNotification notification, CancellationToken cancellationToken)
    {
        var message = notification.Message;
        var commandText = message.Content.Trim().ToLower();
        
        if (commandText != "!matka")
        {
            return;
        }
        
        await message.Channel.SendFileAsync(
            filePath: @"C:\Work\Apps\SnakePit\SnakePit.Discord\SnakePit.Discord.Bot\assets\matka.png", 
            text: $"Looking good <@{Constants.MatkaUserId}>.",
            messageReference: new MessageReference(message.Id)
        );
    }
}