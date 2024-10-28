using Discord;
using Discord.WebSocket;
using MediatR;
using SnakePit.Discord.Bot.Application.Notifications;

namespace SnakePit.Discord.Bot.Application.Handlers.Authors;

/// <summary>
/// Handler for processing messages from a specific author.
/// </summary>
public class FlameAuthorHandler : INotificationHandler<MessageAuthorNotification>
{
    /// <summary>
    /// Keeps track of the timestamp of the last message sent.
    /// </summary>
    private static DateTimeOffset _lastMessage = DateTimeOffset.MinValue;

    /// <summary>
    /// Random number generator for selecting responses.
    /// </summary>
    private static readonly Random Random = new();

    /// <summary>
    /// List of predefined responses to send.
    /// </summary>
    private static readonly List<string> Responses =
    [
        "Be humble <@{0}>.",
        "Stay grounded <@{0}>.",
        "Remember to be modest <@{0}>.",
        "Humility is key <@{0}>.",
        "Keep it humble <@{0}>."
    ];

    /// <summary>
    /// Handles the notification when a message is received.
    /// </summary>
    /// <param name="notification">The notification containing the message.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Handle(MessageAuthorNotification notification, CancellationToken cancellationToken)
    {
        var message = notification.Message;

        // Check if the message is from the specific user and if enough time has passed since the last message
        if (message.Author.Id != Constants.FlameUserId || _lastMessage > message.Timestamp.AddMinutes(-10))
        {
            return;
        }

        // Ignore messages that are too short
        if (message.Content.Trim().Length < 5)
        {
            return;
        }

        // Update the timestamp of the last message
        _lastMessage = message.Timestamp;

        // Send the flame message
        await SendFlameMessage(message);
    }

    /// <summary>
    /// Sends the flame message to the channel.
    /// </summary>
    /// <param name="message">The original message to respond to.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async Task SendFlameMessage(SocketMessage message)
    {
        // Select a random response from the list
        var response = string.Format(Responses[Random.Next(Responses.Count)], Constants.FlameUserId);

        // Send the response along with an image
        await message.Channel.SendFileAsync(
            filePath: @"C:\Work\Apps\SnakePit\SnakePit.Discord\SnakePit.Discord.Bot\assets\flame.png",
            text: response,
            messageReference: new MessageReference(message.Id)
        );
    }
}