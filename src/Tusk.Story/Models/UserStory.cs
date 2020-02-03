namespace Tusk.Story.Models
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

        public int Priority { get; set; }
        public int BusinessValue { get; set; }
        public string Title { get; set; } = "";
        public string Text { get; set; } = "";
        public string AcceptanceCriteria { get; set; } = "";
        public Relevance Importance { get; set; }
    }

}
