namespace Tusk.Domain;

#nullable disable
// Aggregate root entity
public class UserStory : EntityBase
{
    // Enum example with usage in ef core
    // Can optionally replaced by EnumerationPattern
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
        BusinessValue businessValue,
        Relevance relevance = Relevance.CouldHave) : this()
    {
        Title = title;
        Priority = priority;
        Text = text;
        AcceptanceCriteria = acceptanceCriteria;
        BusinessValue = businessValue;
        Importance = relevance;
    }

    public Priority Priority { get; }
    public string Title { get; }
    public string Text { get; }
    public string AcceptanceCriteria { get; }
    public Relevance Importance { get; }
    public BusinessValue BusinessValue { get; }

    private readonly List<StoryTask> _storyTasks = new List<StoryTask>();
    public IReadOnlyList<StoryTask> StoryTasks => _storyTasks.ToList();

    #nullable enable
    // Some dummy examples to work with the relational data
    public void AddTask(
        StoryTask storyTask) =>
        // Work with _storyTasks
        _storyTasks.Add(storyTask);

    public void RemoveTask(
        StoryTask storyTask) =>
        // Work with _storyTasks
        _storyTasks.Remove(storyTask);
}
