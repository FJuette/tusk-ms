using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serilog;
using Tusk.Story.Models;
using Tusk.Story.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Tusk.Story.Stories.Queries
{
    public class GetAllStoriesQuery : IRequest<UserStoryViewModel>
    {
    }

    public class GetAllStoriesQueryHandler : IRequestHandler<GetAllStoriesQuery, UserStoryViewModel>
    {
        private readonly TuskDbContext _ctx;

        public GetAllStoriesQueryHandler(TuskDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<UserStoryViewModel> Handle(GetAllStoriesQuery request, CancellationToken cancellationToken)
        {
            // Example logging call
            Log.Information("Get all Stories called");
            // Use async calls if possible
            var stories = await _ctx.Stories.ToListAsync(cancellationToken: cancellationToken);
            return new UserStoryViewModel(stories);

        }
    }

    public class UserStoryViewModel
    {
        public UserStoryViewModel(List<UserStory> stories)
        {
            Stories = stories;
        }

        public IEnumerable<UserStory> Stories { get; }
    }
}
