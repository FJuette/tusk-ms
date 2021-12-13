using MediatR;
using Microsoft.EntityFrameworkCore;
using Tusk.Api.Exceptions;
using Tusk.Api.Persistence;

namespace Tusk.Api.Stories.Commands;
public record ToggleDoneCommand(int StoryId, int TaskId) : IRequest<bool>;

public class ToggleDoneCommandHandler : IRequestHandler<ToggleDoneCommand, bool>
{
    private readonly TuskDbContext _context;

    public ToggleDoneCommandHandler(
        TuskDbContext context) =>
        _context = context;

    public async Task<bool> Handle(
        ToggleDoneCommand request,
        CancellationToken cancellationToken)
    {
        var story = await _context.Stories
            .Include(e => e.StoryTasks)
            .SingleOrDefaultAsync(e => e.Id == request.StoryId, cancellationToken);

        _ = story ?? throw new NotFoundException("UserStory", request.StoryId);

        var task = story.StoryTasks.SingleOrDefault(e => e.Id == request.TaskId);
        _ = task ?? throw new NotFoundException("StoryTask", request.TaskId);

        task.ToggleDone();
        _context.Attach(story);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}
