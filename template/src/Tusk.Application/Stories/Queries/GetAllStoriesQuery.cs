using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Tusk.Application.Persistence;
using Tusk.Domain;

namespace Tusk.Application.Stories.Queries;

public record GetAllStoriesQuery : IRequest<UserStoriesViewModel>;

public record UserStoriesViewModel(IEnumerable<UserStoriesDto> Data);

public class GetAllStoriesQueryHandler(
    ITuskDbContext context,
    IMapper mapper,
    IDateTime dateTime) : IRequestHandler<GetAllStoriesQuery, UserStoriesViewModel>
{
    public async Task<UserStoriesViewModel> Handle(
        GetAllStoriesQuery request,
        CancellationToken cancellationToken)
    {
        // Use async calls if possible
        // Example logging call
        Log.Information("Get all Stories called at {Now}", dateTime.Now);
        // Using the ProjectTo<T> from automapper to optimise the resulting sql query
        var stories = await context.Stories
            .Include(e => e.StoryTasks) // Add Includes if needed (eager loading)
            .ProjectTo<UserStoriesDto>(mapper.ConfigurationProvider)
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

// Automapper Profile for this Dto
public class UserStoriesProfile : Profile
{
    public UserStoriesProfile() =>
        CreateMap<UserStory, UserStoriesDto>()
            .ForMember(d => d.Priority,
                opt =>
                    opt.MapFrom(c => c.Priority.Value));
}
