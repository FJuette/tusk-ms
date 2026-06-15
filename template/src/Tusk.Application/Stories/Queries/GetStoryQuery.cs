using DispatchR.Abstractions.Send;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Tusk.Application.Exceptions;
using Tusk.Application.Persistence;
using Tusk.Domain;

namespace Tusk.Application.Stories.Queries;

public record GetStoryQuery(int Id) : IRequest<GetStoryQuery, ValueTask<UserStoryViewModel>>;

public record UserStoryViewModel(UserStoryDto Story);

public class GetStoryQueryHandler(
    ITuskDbContext context,
    TypeAdapterConfig mapConfig) : IRequestHandler<GetStoryQuery, ValueTask<UserStoryViewModel>>
{
    public async ValueTask<UserStoryViewModel> Handle(
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
            .ProjectToType<UserStoryDto>(mapConfig)
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

// Mapster register for this Dto
public class UserStoryRegister : IRegister
{
    public void Register(TypeAdapterConfig config) =>
        config.NewConfig<UserStory, UserStoryDto>()
            .Map(d => d.Priority, c => c.Priority.Value)
            .Map(d => d.BusinessValue, c => c.BusinessValue.Name)
            .Map(d => d.Tasks, c => c.StoryTasks.Select(e => $"[{(e.IsDone ? 'x' : ' ')}] {e.Description}"));
}
