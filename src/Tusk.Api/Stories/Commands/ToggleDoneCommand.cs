using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tusk.Api.Exceptions;
using Tusk.Api.Persistence;

namespace Tusk.Api.Stories.Commands
{
    public class ToggleDoneCommand : IRequest<bool>
    {
        public ToggleDoneCommand(
            int storyId,
            int taskId)
        {
            StoryId = storyId;
            TaskId = taskId;
        }

        public int StoryId { get; }
        public int TaskId { get; }
    }

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
            if (story is null)
            {
                throw new NotFoundException("UserStory", request.StoryId);
            }

            var task = story.StoryTasks.SingleOrDefault(e => e.Id == request.TaskId);
            if (task is null)
            {
                throw new NotFoundException("StoryTask", request.TaskId);
            }

            task.ToggleDone();
            _context.Attach(story);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
