using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serilog;
using Tusk.Api.Models;
using Tusk.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Tusk.Api.Stories.Queries
{
    public class GetAllStoriesQuery : IRequest<UserStoriesViewModel>
    {
    }

    public class GetAllStoriesQueryHandler : IRequestHandler<GetAllStoriesQuery, UserStoriesViewModel>
    {
        private readonly TuskDbContext _ctx;

        public GetAllStoriesQueryHandler(TuskDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<UserStoriesViewModel> Handle(GetAllStoriesQuery request, CancellationToken cancellationToken)
        {
            // Example logging call
            Log.Information("Get all Stories called");
            // Use async calls if possible
            var stories = await _ctx.Stories.ToListAsync(cancellationToken: cancellationToken);
            // TODO use automapper projection, e.g.:
            // return context.OrderLines.Where(ol => ol.OrderId == orderId)
            // .ProjectTo<OrderLineDTO>().ToList();
            return new UserStoriesViewModel(stories);

        }
    }

    public class UserStoriesViewModel
    {
        public UserStoriesViewModel(List<UserStory> stories)
        {
            Stories = stories;
        }

        public IEnumerable<UserStory> Stories { get; }
    }
}
