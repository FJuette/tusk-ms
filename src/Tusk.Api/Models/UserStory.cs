using System.Collections.Generic;
using System.Linq;

namespace Tusk.Api.Models
{
    public class UserStory : EntityBase
    {
        // Enum example with usage in ef
        // Can replaced by EnumerationPattern
        public enum Relevance
        {
            MustHave,
            ShouldHave,
            CouldHave
        }

        protected UserStory()
        {

        }

        public UserStory(
            string title,
            Priority priority,
            string text,
            string acceptanceCriteria,
            Relevance relevance = Relevance.CouldHave) : this()
        {
            Title = title;
            Priority = priority;
            Text = text;
            AcceptanceCriteria = acceptanceCriteria;
            Importance = relevance;
        }

        public Priority Priority { get; }
        public string Title { get; }
        public string Text { get; }
        public string AcceptanceCriteria { get; }
        public Relevance Importance { get; }

        private readonly List<StoryTask> _storyTasks = new List<StoryTask>();
        public IReadOnlyList<StoryTask> StoryTasks => _storyTasks.ToList();

        // Some dummy examples to work with the relational data
        public void AddTask(StoryTask storyTask)
        {
            // Work with _storyTasks
            // ...
        }

        public void RemoveTask(StoryTask storyTask)
        {
            // Work with _storyTasks
            // ...
        }

        public void SetTaskDone(StoryTask storyTask)
        {
            // Work with _storyTasks
            // ...
        }
    }

}
