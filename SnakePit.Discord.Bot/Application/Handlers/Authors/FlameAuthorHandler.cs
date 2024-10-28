using Discord;
using MediatR;
using SnakePit.Discord.Bot.Application.Notifications;

namespace SnakePit.Discord.Bot.Application.Handlers.Authors;

public class FlameAuthorHandler : INotificationHandler<MessageAuthorNotification>
{
    private static DateTimeOffset _lastMessage = DateTimeOffset.MinValue;
    
    public async Task Handle(MessageAuthorNotification notification, CancellationToken cancellationToken)
    {
        var message = notification.Message;
        if (message.Author.Id != Constants.FlameUserId || _lastMessage > message.Timestamp.AddMinutes(-10))
        {
            return;
        }
        
        _lastMessage = message.Timestamp;
        
        await message.Channel.SendFileAsync(
            filePath: @"C:\Work\Apps\SnakePit\SnakePit.Discord\SnakePit.Discord.Bot\assets\flame.png", 
            text: $"Be humble <@{Constants.FlameUserId}>.",
            messageReference: new MessageReference(message.Id)
        );
    }
}