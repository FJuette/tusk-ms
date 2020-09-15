using System.Collections.Generic;
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
                .Include(e => e.StoryTasks)
                .Include(e => e.BusinessValue)
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
        public UserStoryDto(int id, string title, string text, string acceptanceCriteria, int priority, string businessValue)
        {
            Id = id;
            Title = title;
            Text = text;
            AcceptanceCriteria = acceptanceCriteria;
            Priority = priority;
            BusinessValue = businessValue;
        }

        public int Id { get; }
        public string Title { get; }
        public string Text { get; }
        public string AcceptanceCriteria { get; }
        public int Priority { get; }
        public string BusinessValue { get; }
        public List<string>? Tasks { get; private set; }
    }

    // Automapper Profile for this Dto
    public class UserStoryProfile : Profile
    {
        public UserStoryProfile()
        {
            CreateMap<UserStory, UserStoryDto>()
                .ForCtorParam(
                    "priority",
                    opt => opt.MapFrom(
                        c => c.Priority.Value))
                .ForCtorParam(
                    "businessValue",
                    opt => opt.MapFrom(
                        c => c.BusinessValue.Name))
                .ForMember(d => d.Tasks,
                    opt => opt.MapFrom(
                        c => c.StoryTasks.Select(e => $"[{(e.IsDone ? 'x' : ' ')}] {e.Description}" )));
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
