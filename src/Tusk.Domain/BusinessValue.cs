namespace Tusk.Domain;

// Example for enumeration pattern class
#nullable disable
public class BusinessValue : EntityBase
{
    public static readonly BusinessValue BV1000 = new(1, "Business Value 1000");
    public static readonly BusinessValue BV900 = new(2, "Business Value 900");

    public static readonly BusinessValue[] AllBusinessValues = { BV1000, BV900 };

    protected BusinessValue()
    {
    }

    private BusinessValue(
        int id,
        string name)
        : base(id) =>
        Name = name;

    public string Name { get; }

    public static BusinessValue FromId(int id) =>
        AllBusinessValues
            .SingleOrDefault(c => c.Id == id);
}
