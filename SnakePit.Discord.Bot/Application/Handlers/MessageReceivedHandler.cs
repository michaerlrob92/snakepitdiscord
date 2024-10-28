using MediatR;
using SnakePit.Discord.Bot.Application.Notifications;

namespace SnakePit.Discord.Bot.Application.Handlers;

public class MessageReceivedHandler(IMediator mediator) : INotificationHandler<MessageReceivedNotification>
{
    public async Task Handle(MessageReceivedNotification notification, CancellationToken cancellationToken)
    {
        var message = notification.Message;

        if (message.Author.IsBot)
        {
            return;
        }

        var messageContent = message.Content.Trim().ToLower();

        if (messageContent.StartsWith('!'))
        {
            await mediator.Publish(new CommandReceivedNotification(message), cancellationToken);
            return;
        }
        
        await mediator.Publish(new MessageAuthorNotification(message), cancellationToken);
    }
}