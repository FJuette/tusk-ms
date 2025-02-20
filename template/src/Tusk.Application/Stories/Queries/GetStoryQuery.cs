using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Tusk.Application.Exceptions;
using Tusk.Application.Persistence;
using Tusk.Domain;

namespace Tusk.Application.Stories.Queries;

public record GetStoryQuery(int Id) : IRequest<UserStoryViewModel>;
public record UserStoryViewModel(UserStoryDto Story);

public class GetStoryQueryHandler(
    ITuskDbContext context,
    IMapper mapper) : IRequestHandler<GetStoryQuery, UserStoryViewModel>
{

    public async Task<UserStoryViewModel> Handle(
        GetStoryQuery request,
        CancellationToken cancellationToken)
    {
        // Example logging call
        Log.Information("Get single story called");
        // Use async calls if possible
        var story = await context.Stories
            .Where(e => e.Id == request.Id)
            .Include(e => e.StoryTasks)
            .Include(e => e.BusinessValue)
            .ProjectTo<UserStoryDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);

        _ = story ?? throw new NotFoundException("Story", request.Id);

        return new UserStoryViewModel(story);
    }
}

public record UserStoryDto
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public required string Text { get; init; }
    public required string AcceptanceCriteria { get; init; }
    public int Priority { get; init; }
    public required string BusinessValue { get; init; }
    public IReadOnlyList<string> Tasks { get; init; } = [];
}

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
