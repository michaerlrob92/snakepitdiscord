using Discord;
using MediatR;
using SnakePit.Discord.Bot.Application.Notifications;

namespace SnakePit.Discord.Bot.Application.Handlers.Commands;

public class PraygCommandHandler : INotificationHandler<CommandReceivedNotification>
{
    public async Task Handle(CommandReceivedNotification notification, CancellationToken cancellationToken)
    {
        var message = notification.Message;
        var commandText = message.Content.Trim().ToLower();
        
        if (commandText != "!prayg")
        {
            return;
        }
        
        await message.Channel.SendFileAsync(
            filePath: @"C:\Work\Apps\SnakePit\SnakePit.Discord\SnakePit.Discord.Bot\assets\prayg.png", 
            text: $"Is that Channing Tatum? <@{Constants.PraygUserId}>",
            messageReference: new MessageReference(message.Id)
        );
    }
}