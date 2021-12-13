using MediatR;
using Serilog;

namespace Tusk.Api.Stories.Events;
public record UserStoryAddedEvent(string Title) : INotification;

public class UserStoryAddedLoggerHandler : INotificationHandler<UserStoryAddedEvent>
{
    public Task Handle(UserStoryAddedEvent notification, CancellationToken cancellationToken)
    {
        Log.Information($"Story '{notification.Title}' created");
        return Task.CompletedTask;
    }
}
