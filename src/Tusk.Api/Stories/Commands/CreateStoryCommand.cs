using System;
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
        public string Title { get; set; } = "";
        public string Text { get; set; } = "";
        public UserStory.Relevance Importance { get; set; }
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
            var story = new UserStory
            {
                Title = request.Title,
                Text = request.Text,
                Importance = request.Importance
            };

            var result = await _context.Stories.AddAsync(story, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return result.Entity.Id;
        }
    }

    public class CreateStoryValidator : AbstractValidator<CreateStoryCommand>
    {
        public CreateStoryValidator()
        {
            RuleFor(x => x.Importance).IsInEnum()
                .WithMessage("Provide a valid importance value (e.g. 0, 1, 2)");
        }
    }
}
