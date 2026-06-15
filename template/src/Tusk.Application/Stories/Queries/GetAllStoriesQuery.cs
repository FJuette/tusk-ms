using DispatchR.Abstractions.Send;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Tusk.Application.Persistence;
using Tusk.Domain;

namespace Tusk.Application.Stories.Queries;

public record GetAllStoriesQuery : IRequest<GetAllStoriesQuery, ValueTask<UserStoriesViewModel>>;

public record UserStoriesViewModel(IEnumerable<UserStoriesDto> Data);

public class GetAllStoriesQueryHandler(
    ITuskDbContext context,
    TypeAdapterConfig mapConfig,
    IDateTime dateTime) : IRequestHandler<GetAllStoriesQuery, ValueTask<UserStoriesViewModel>>
{
    public async ValueTask<UserStoriesViewModel> Handle(
        GetAllStoriesQuery request,
        CancellationToken cancellationToken)
    {
        // Use async calls if possible
        // Example logging call
        Log.Information("Get all Stories called at {Now}", dateTime.Now);
        var stories = await context.Stories
            .Include(e => e.StoryTasks) // Add Includes if needed (eager loading)
            .ProjectToType<UserStoriesDto>(mapConfig)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new UserStoriesViewModel(stories);
    }
}

// Example Dto
public record UserStoriesDto
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public int Priority { get; init; }
}

// Mapster register for this Dto
public class UserStoriesRegister : IRegister
{
    public void Register(TypeAdapterConfig config) =>
        config.NewConfig<UserStory, UserStoriesDto>()
            .Map(d => d.Priority, c => c.Priority.Value);
}
