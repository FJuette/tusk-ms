using Tusk.Api.Models;
using Tusk.Api.Persistence;

namespace Tusk.Api.Extensions;
public class SampleDataSeeder
{
    private readonly TuskDbContext _context;

    public SampleDataSeeder(
        TuskDbContext context) =>
        _context = context;

    public bool SeedAll()
    {
        if (_context.Stories.Any())
            return false;

        var story = new UserStory(
            "My first story",
            Priority.Create(1).Value,
            "This is my content",
            "This must be fulfilled",
            BusinessValue.BV900,
            UserStory.Relevance.ShouldHave);

        story.AddTask(new StoryTask("Do this for me now!"));
        story.AddTask(new StoryTask("And now this!"));

        _context.Stories.Add(story);

        _context.Stories.Add(
            new UserStory(
                "Second Story",
                Priority.Create(2).Value,
                "As administrator i want ...",
                "Working CI/CD pipeline",
                BusinessValue.BV1000,
                UserStory.Relevance.MustHave
            ));
        return _context.SaveChanges() > 0;
    }
}
