using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serilog;
using Tusk.Api.Models;
using Tusk.Api.Persistence;
using Tusk.Api.Exceptions;

namespace Tusk.Api.Stories.Queries
{
    public class GetStoryQuery : IRequest<UserStoryViewModel>
    {
        public GetStoryQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }

    public class GetStoryQueryHandler : IRequestHandler<GetStoryQuery, UserStoryViewModel>
    {
        private readonly TuskDbContext _ctx;

        public GetStoryQueryHandler(TuskDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<UserStoryViewModel> Handle(GetStoryQuery request, CancellationToken cancellationToken)
        {
            // Example logging call
            Log.Information("Get all Stories called");
            // Use async calls if possible
            var story = await _ctx.Stories.FindAsync(request.Id);
            if (story == null)
            {
                throw new NotFoundException("Story", request.Id);
            }
            return new UserStoryViewModel(story);

        }
    }

    public class UserStoryViewModel
    {
        public UserStoryViewModel(UserStory story)
        {
            Story = story;
        }

        public UserStory Story { get; }
    }
}
