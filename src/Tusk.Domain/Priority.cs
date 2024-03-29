using CSharpFunctionalExtensions;

namespace Tusk.Domain;
public sealed class Priority : ValueObject
{
    private Priority(
        int value) =>
        Value = value;

    public int Value { get; }

    public static Result<Priority> Create(
        int priority) =>
        // Make validation checks and return
        priority < 0
            ? Result.Failure<Priority>("Must be a positive number")
            : Result.Success(new Priority(priority));

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }

    // Implicit cast from Priority back to int
    public static implicit operator int(
        Priority priority) =>
        priority.Value;
}
