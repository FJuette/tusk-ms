namespace Tusk.Application;

public interface IDateTime
{
    DateTime Now { get; }
}

public class MachineDateTime : IDateTime
{
    public DateTime Now => DateTime.Now;
}

