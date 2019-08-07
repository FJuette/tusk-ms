namespace Tusk.Story.Models
{
    public class UserStory : EntityBase
    {

        public int Priority { get; set; }
        public int BusinessValue { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string AcceptanceCriteria { get; set; }
        public Muscow Importance { get; set; }
    }

    public enum Muscow
    {
        MustHave,
        ShouldHave,
        CouldHave
    }
}