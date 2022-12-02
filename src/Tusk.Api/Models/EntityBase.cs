namespace Tusk.Api.Models;
public interface IOwnedBy
{
    // This holds the ID of the person/institution who owns the entity
    string OwnedBy { get; }
    void SetOwnedBy(string protectKey);
}

public abstract class EntityBase : IOwnedBy
{
    protected EntityBase()
    {
    }

    protected EntityBase(
        int id)
        : this() =>
        Id = id;

    public int Id { get; set; }

    public string OwnedBy { get; private set; } = "Admin"; // TODO CHANGE (Dummy default value)

    public void SetOwnedBy(
        string protectKey) =>
        OwnedBy = protectKey;

    public override bool Equals(
        object? obj)
    {
        if (!(obj is EntityBase other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (GetRealType() != other.GetRealType())
        {
            return false;
        }

        if (Id == 0 || other.Id == 0)
        {
            return false;
        }

        return Id == other.Id;
    }

    public static bool operator ==(
        EntityBase? a,
        EntityBase? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(
        EntityBase a,
        EntityBase b) =>
        !(a == b);

    public override int GetHashCode() =>
        (GetRealType().ToString() + Id)
        .GetHashCode(StringComparison.InvariantCulture);

    private Type GetRealType()
    {
        Type type = GetType();

        if (type.ToString().Contains("Castle.Proxies.", StringComparison.InvariantCulture) && type.BaseType != null)
        {
            return type.BaseType;
        }

        return type;
    }
}
