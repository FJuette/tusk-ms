using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Tusk.Api.Models
{
    public sealed class Priority : ValueObject
    {
        private Priority(
            int value)
        {
            Value = value;
        }
        public int Value { get; }

        public static Result<Priority> Create(
            int priority)
        {
            // Make validation checks and return
            return priority < 0 ?
                Result.Failure<Priority>("Must be a positive number")
                : Result.Success(new Priority(priority));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        // Implicit cast from Priority back to int
        public static implicit operator int(
            Priority priority)
        {
            return priority.Value;
        }
    }
}
