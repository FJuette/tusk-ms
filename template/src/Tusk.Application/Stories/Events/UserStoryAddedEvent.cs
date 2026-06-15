using DispatchR.Abstractions.Notification;
using Serilog;

namespace Tusk.Application.Stories.Events;

public record UserStoryAddedEvent(string Title) : INotification;

public class UserStoryAddedLoggerHandler : INotificationHandler<UserStoryAddedEvent>
{
    public ValueTask Handle(UserStoryAddedEvent notification, CancellationToken cancellationToken)
    {
        Log.Information("Story '{Title}' created", notification.Title);
        return ValueTask.CompletedTask;
    }
}
