using Discord;
using MediatR;
using SnakePit.Discord.Bot.Application.Notifications;

namespace SnakePit.Discord.Bot.Application.Handlers.Commands;

public class FlameCommandHandler : INotificationHandler<CommandReceivedNotification>
{
    public async Task Handle(CommandReceivedNotification notification, CancellationToken cancellationToken)
    {
        var message = notification.Message;
        var commandText = message.Content.Trim().ToLower();
        
        if (commandText != "!flame")
        {
            return;
        }
        
        await message.Channel.SendFileAsync(
            filePath: @"C:\Work\Apps\SnakePit\SnakePit.Discord\SnakePit.Discord.Bot\assets\flame.png", 
            text: $"Be humble <@{Constants.FlameUserId}>.",
            messageReference: new MessageReference(message.Id)
        );
    }
}