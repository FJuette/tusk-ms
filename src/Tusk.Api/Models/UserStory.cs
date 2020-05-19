namespace Tusk.Api.Models
{
    public class UserStory : EntityBase
    {
        // Enum example with usage in ef
        public enum Relevance
        {
            MustHave,
            ShouldHave,
            CouldHave
        }

        // TODO here
        public UserStory()
        {

        }

        public UserStory(string title)
        {
            Title = title;
        }

        public int? Priority { get; }
        public int? BusinessValue { get; }
        public string Title { get; }
        public string? Text { get; }
        public string? AcceptanceCriteria { get; }
        public Relevance Importance { get; } = Relevance.CouldHave;
    }

}
