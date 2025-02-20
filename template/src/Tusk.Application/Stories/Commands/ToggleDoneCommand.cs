using MediatR;
using Microsoft.EntityFrameworkCore;
using Tusk.Application.Exceptions;
using Tusk.Application.Persistence;

namespace Tusk.Application.Stories.Commands;
public record ToggleDoneCommand(int StoryId, int TaskId) : IRequest<bool>;

public class ToggleDoneCommandHandler(ITuskDbContext context) : IRequestHandler<ToggleDoneCommand, bool>
{
    public async Task<bool> Handle(
        ToggleDoneCommand request,
        CancellationToken cancellationToken)
    {
        var story = await context.Stories
            .Include(e => e.StoryTasks)
            .SingleOrDefaultAsync(e => e.Id == request.StoryId, cancellationToken);

        _ = story ?? throw new NotFoundException("UserStory", request.StoryId);

        var task = story.StoryTasks.SingleOrDefault(e => e.Id == request.TaskId);
        _ = task ?? throw new NotFoundException("StoryTask", request.TaskId);

        task.ToggleDone();
        context.Attach(story);
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}
