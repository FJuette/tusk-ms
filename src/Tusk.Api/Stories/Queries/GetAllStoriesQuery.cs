using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serilog;
using Tusk.Api.Models;
using Tusk.Api.Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Tusk.Api.Stories.Queries
{
    public class GetAllStoriesQuery : IRequest<UserStoriesViewModel>
    {
    }

    public class GetAllStoriesQueryHandler : IRequestHandler<GetAllStoriesQuery, UserStoriesViewModel>
    {
        private readonly TuskDbContext _ctx;
        private readonly IMapper _mapper;

        public GetAllStoriesQueryHandler(TuskDbContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<UserStoriesViewModel> Handle(GetAllStoriesQuery request, CancellationToken cancellationToken)
        {
            // Use async calls if possible
            // Example logging call
            Log.Information("Get all Stories called");
            // Using the ProjectTo<T> from automapper to optimise the resulting sql query
            var stories = await _ctx.Stories
                .Include(e => e.StoryTasks) // Include if needed
                .ProjectTo<UserStoriesDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync(cancellationToken: cancellationToken);
            return new UserStoriesViewModel(stories);

        }
    }

    // Example Dto
    public class UserStoriesDto
    {
        public UserStoriesDto(int id, string title)
        {
            Id = id;
            Title = title;
        }

        public int Id { get; }
        public string Title { get; }
        // Not a clean design with setter, but actually I am unable to tell automapper to use my defined mapping in the constructor
        public int Priority { get; set; }
    }

    // Automapper Profile for this Dto
    public class UserStoriesProfile : Profile
    {
        public UserStoriesProfile()
        {
            CreateMap<UserStory, UserStoriesDto>()
                .ForMember(d => d.Priority, opt => opt.MapFrom(c => c.Priority.Value));
        }
    }

    public class UserStoriesViewModel
    {
        public UserStoriesViewModel(List<UserStoriesDto> stories)
        {
            Stories = stories;
        }

        public IEnumerable<UserStoriesDto> Stories { get; }
    }
}
