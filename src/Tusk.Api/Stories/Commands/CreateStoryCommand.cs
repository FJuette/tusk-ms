using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Tusk.Api.Models;
using Tusk.Api.Persistence;

namespace Tusk.Api.Stories.Commands
{
    public class CreateStoryCommand : IRequest<int>
    {
        public CreateStoryCommand(string title, string text, UserStory.Relevance importance, int businessValue)
        {
            Title = title;
            Text = text;
            Importance = importance;
            BusinessValue = businessValue;
        }
        public string Title { get; }
        public string Text { get; }
        public UserStory.Relevance Importance { get; }
        public int BusinessValue { get; }
    }

    public class CreateStoryCommandHandler : IRequestHandler<CreateStoryCommand, int>
    {
        private readonly TuskDbContext _context;

        public CreateStoryCommandHandler(TuskDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateStoryCommand request, CancellationToken cancellationToken)
        {
            var story = new UserStory(request?.Title, Priority.Create(1).Value, request?.Text, "", BusinessValue.BV1000);

            // Prefer attach over add/update
            var result = _context.Stories.Attach(story);
            await _context.SaveChangesAsync(cancellationToken);
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
}
