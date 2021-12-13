using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Tusk.Api.Exceptions;
using Tusk.Api.Models;
using Tusk.Api.Persistence;

namespace Tusk.Api.Stories.Queries;

public record GetStoryQuery(int Id) : IRequest<UserStoryViewModel>;
public record UserStoryViewModel(UserStoryDto Story);

public class GetStoryQueryHandler : IRequestHandler<GetStoryQuery, UserStoryViewModel>
{
    private readonly TuskDbContext _ctx;
    private readonly IMapper _mapper;

    public GetStoryQueryHandler(
        TuskDbContext ctx,
        IMapper mapper)
    {
        _ctx = ctx;
        _mapper = mapper;
    }

    public async Task<UserStoryViewModel> Handle(
        GetStoryQuery request,
        CancellationToken cancellationToken)
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

        _ = story ?? throw new NotFoundException("Story", request.Id);

        return new UserStoryViewModel(story);
    }
}

#nullable disable
public record UserStoryDto
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string Text { get; init; }
    public string AcceptanceCriteria { get; init; }
    public int Priority { get; init; }
    public string BusinessValue { get; init; }
    public IReadOnlyList<string> Tasks { get; init; }
}
#nullable enable

// Automapper Profile for this Dto
public class UserStoryProfile : Profile
{
    public UserStoryProfile() =>
        CreateMap<UserStory, UserStoryDto>()
            .ForMember(d => d.Priority,
                opt => opt.MapFrom(
                    c => c.Priority.Value))
            .ForMember(d => d.BusinessValue,
                opt => opt.MapFrom(
                    c => c.BusinessValue.Name))
            .ForMember(d => d.Tasks,
                opt => opt.MapFrom(
                    c => c.StoryTasks.Select(e => $"[{(e.IsDone ? 'x' : ' ')}] {e.Description}")));
}
