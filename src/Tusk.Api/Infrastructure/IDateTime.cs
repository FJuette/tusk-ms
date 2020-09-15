using System;

namespace Tusk.Api.Infrastructure
{
    public interface IDateTime
    {
        DateTime Now { get; }
    }

    public class MachineDateTime : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
