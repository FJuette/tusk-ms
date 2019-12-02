using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serilog;
using Tusk.Story.Models;
using Tusk.Story.Persistance;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections;

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
            var stories = await _ctx.Stories.ToListAsync();
            stories.Add(new UserStory{
                AcceptanceCriteria = "bal",
                BusinessValue = 1000,
                Priority = 1,
                Text = "Info",
                Title = "my new story"
            });
            return new UserStoryViewModel
            {
                Stories = stories
            };

        }
    }

    public class UserStoryViewModel
    {
        public IEnumerable<UserStory> Stories { get; set; }
    }
}