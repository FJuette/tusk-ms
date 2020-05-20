using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serilog;
using Tusk.Api.Models;
using Tusk.Api.Persistence;
using Tusk.Api.Exceptions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
        private readonly IMapper _mapper;

        public GetStoryQueryHandler(TuskDbContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<UserStoryViewModel> Handle(GetStoryQuery request, CancellationToken cancellationToken)
        {
            // Example logging call
            Log.Information("Get single story called");
            // Use async calls if possible
            var story = await _ctx.Stories
                .Where(e => e.Id == request.Id)
                .Include(e => e.StoryTasks) // Include if needed
                .ProjectTo<UserStoryDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .SingleOrDefaultAsync(cancellationToken);
            if (story is null)
            {
                throw new NotFoundException("Story", request.Id);
            }
            return new UserStoryViewModel(story);

        }
    }

    // Example Dto
    public class UserStoryDto
    {
        public UserStoryDto(int id, string title, string text, string acceptanceCriteria)
        {
            Id = id;
            Title = title;
            Text = text;
            AcceptanceCriteria = acceptanceCriteria;
        }

        public int Id { get; }
        public string Title { get; }
        public string Text { get; }
        public string AcceptanceCriteria { get; }
        // Not a clean design with setter, but actually I am unable to tell automapper to use my defined mapping in the constructor
        public int Priority { get; set; }
    }

    // Automapper Profile for this Dto
    public class UserStoryProfile : Profile
    {
        public UserStoryProfile()
        {
            CreateMap<UserStory, UserStoryDto>()
                .ForMember(d => d.Priority, opt => opt.MapFrom(c => c.Priority.Value));
        }
    }

    public class UserStoryViewModel
    {
        public UserStoryViewModel(UserStoryDto story)
        {
            Story = story;
        }

        public UserStoryDto Story { get; }
    }
}
