using FluentValidation;
using MediatR;
using Tusk.Application.Persistence;
using Tusk.Application.Stories.Events;
using Tusk.Domain;

namespace Tusk.Application.Stories.Commands;

public record CreateStoryCommand : IRequest<int>
{
    public required string Title { get; init; }
    public required string Text { get; init; }
    public UserStory.Relevance Importance { get; init; }
    public int BusinessValue { get; init; }
}

public class CreateStoryCommandHandler(
    ITuskDbContext context,
    IMediator mediator,
    IValidator<CreateStoryCommand> validator) : IRequestHandler<CreateStoryCommand, int>
{
    public async Task<int> Handle(
        CreateStoryCommand request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var story = new UserStory(
            request.Title,
            Priority.Create(1).Value,
            request.Text,
            "",
            BusinessValue.BV1000);

        // Prefer attach over add/update
        var result = context.Stories.Attach(story);
        await context.SaveChangesAsync(cancellationToken);

        // Example for raising an event ...
        await mediator.Publish(new UserStoryAddedEvent(story.Title), cancellationToken);

        return result.Entity.Id;
    }
}

public class CreateStoryValidator : AbstractValidator<CreateStoryCommand>
{
    public CreateStoryValidator()
    {
        RuleFor(x => x.Title).NotEmpty()
            .WithMessage("Title is required");
        RuleFor(x => x.Importance).IsInEnum()
            .WithMessage("Provide a valid importance value (e.g. 0, 1, 2)");
        RuleFor(x => x.BusinessValue)
            .Must(v => BusinessValue.AllBusinessValues.Select(e => e.Id).Contains(v))
            .WithMessage("Provide a valid business value id (e.g. 0, 1)");
    }
}
