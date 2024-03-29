using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Tusk.Application.Persistence;
using Tusk.Domain;

namespace Tusk.Application.Stories.Queries;
public record GetAllStoriesQuery() : IRequest<UserStoriesViewModel>;
public record UserStoriesViewModel(IEnumerable<UserStoriesDto> Data);

public class GetAllStoriesQueryHandler : IRequestHandler<GetAllStoriesQuery, UserStoriesViewModel>
{
    private readonly ITuskDbContext _ctx;
    private readonly IDateTime _dt;
    private readonly IMapper _mapper;

    public GetAllStoriesQueryHandler(
        ITuskDbContext ctx,
        IMapper mapper,
        IDateTime dt)
    {
        _ctx = ctx;
        _mapper = mapper;
        _dt = dt;
    }

    public async Task<UserStoriesViewModel> Handle(
        GetAllStoriesQuery request,
        CancellationToken cancellationToken)
    {
        // Use async calls if possible
        // Example logging call
        Log.Information($"Get all Stories called at {_dt.Now:dd.MM.yyyy H:mm:ss}");
        // Using the ProjectTo<T> from automapper to optimise the resulting sql query
        var stories = await _ctx.Stories
            .Include(e => e.StoryTasks) // Add Includes if needed (eager loading)
            .ProjectTo<UserStoriesDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new UserStoriesViewModel(stories);
    }
}

// Example Dto
#nullable disable
public record UserStoriesDto
{
    public int Id { get; init; }
    public string Title { get; init; }
    public int Priority { get; init; }
}
#nullable enable

// Automapper Profile for this Dto
public class UserStoriesProfile : Profile
{
    public UserStoriesProfile() =>
        CreateMap<UserStory, UserStoriesDto>()
            .ForMember(d => d.Priority,
                opt =>
                    opt.MapFrom(c => c.Priority.Value));
}
