namespace Tusk.Api.Models
{
    // Dummy to show 1:n relation with UserStories
    public class StoryTask : EntityBase
    {
#nullable disable
        protected StoryTask()
        {
        }

        public StoryTask(string description)
        {
            Description = description;
        }
        
        public string Description { get; }
        public bool IsDone { get; private set; }

#nullable enable
        public StoryTask ToggleDone()
        {
            IsDone = !IsDone;
            return this;
        }
    }
}
