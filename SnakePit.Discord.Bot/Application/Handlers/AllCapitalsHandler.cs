using Discord;
using MediatR;
using SnakePit.Discord.Bot.Application.Notifications;
using System.Collections.Concurrent;

namespace SnakePit.Discord.Bot.Application.Handlers;

/// <summary>
/// Handles messages received in Discord and responds to messages that are in all capitals.
/// </summary>
public class AllCapitalsHandler : INotificationHandler<MessageReceivedNotification>
{
    /// <summary>
    /// DateTimeOffset of the last message for each user.
    /// </summary>
    private static readonly ConcurrentDictionary<ulong, DateTimeOffset> LastMessageByUserId = new();

    /// <summary>
    /// List of responses to reply with.
    /// </summary>
    private static readonly List<string> Responses =
    [
        "Are you trying to shout or just proving you don’t know how to use caps lock? <@{0}>",
        "Wow, I didn't know we were in a yelling contest! <@{0}>",
        "Looks like someone really wants to be heard—too bad it’s just a text! <@{0}>",
        "Did you forget how to type or is this your way of expressing excitement? <@{0}>",
        "Are you typing from the top of a mountain? Because it feels like you’re yelling! <@{0}>",
        "I see your message is in all caps—does that mean it’s extra important or just extra loud? <@{0}>",
        "Nice to see your keyboard is working hard! Too bad it’s all caps. <@{0}>",
        "I didn't realize I was reading a headline - do you have news to share? <@{0}>",
        "If you're trying to be dramatic, mission accomplished! <@{0}>",
        "Did you lose your shift key, or are you just very passionate about this? <@{0}>",
        "Why are we shouting? <@{0}>",
        "Is your caps lock key stuck? <@{0}>",
        "I can hear you loud and clear, no need to shout! <@{0}>",
        "Using all caps doesn't make your point stronger, <@{0}>.",
        "Calm down, <@{0}>. No need to yell.",
        "Please, lower your voice, <@{0}>.",
        "All caps? Really, <@{0}>?",
        "Take it easy, <@{0}>. We can hear you just fine.",
        "No need to scream, <@{0}>.",
        "Easy there, <@{0}>. We get it."
    ];

    /// <summary>
    /// Random number generator for selecting responses.
    /// </summary>
    private static readonly Random Random = new();

    /// <summary>
    /// The minimum length of a message to be considered for a response.
    /// </summary>
    private const int MinMessageLength = 4;

    /// <summary>
    /// The time interval in minutes to wait before responding to the same user again.
    /// </summary>
    private const int ResponseCooldownMinutes = 5;

    /// <summary>
    /// Handles the message received notification.
    /// </summary>
    /// <param name="notification">The notification containing the message.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Handle(MessageReceivedNotification notification, CancellationToken cancellationToken)
    {
        var message = notification.Message;

        // Check if the message contains any letters and is in all capitals.
        var messageLetterLength = message.Content.Count(char.IsLetter);
        if (messageLetterLength > MinMessageLength && message.Content.Any(char.IsLetter) && message.Content.ToUpper().Equals(message.Content))
        {
            var authorId = message.Author.Id;
            var lastMessageTime = LastMessageByUserId.GetOrAdd(authorId, DateTimeOffset.MinValue);

            // If the last message was sent more than the cooldown period ago.
            if (lastMessageTime <= message.Timestamp.AddMinutes(-ResponseCooldownMinutes))
            {
                // Select a random response and send it.
                var response = string.Format(Responses[Random.Next(Responses.Count)], message.Author.Id);
                await message.Channel.SendMessageAsync(
                    text: response,
                    messageReference: new MessageReference(message.Id)
                );

                // Update the last message time for the user.
                LastMessageByUserId[authorId] = message.Timestamp;
            }
        }
    }
}