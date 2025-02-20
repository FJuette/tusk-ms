using MediatR;
using Serilog;

namespace Tusk.Application.Stories.Events;

public record UserStoryAddedEvent(string Title) : INotification;

public class UserStoryAddedLoggerHandler : INotificationHandler<UserStoryAddedEvent>
{
    public Task Handle(UserStoryAddedEvent notification, CancellationToken cancellationToken)
    {
        Log.Information("Story '{Title}' created", notification.Title);
        return Task.CompletedTask;
    }
}
