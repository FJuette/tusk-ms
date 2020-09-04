using System;

namespace Tusk.Api.Models
{
    public interface IOwnedBy
    {
        // This holds the ID of the person/institution who own the entity
        string OwnedBy { get; }
        void SetOwnedBy(string protectKey);
    }

    public abstract class EntityBase : IOwnedBy
    {
        public int Id { get; set; }

        public string OwnedBy { get; private set; } = "Admin"; // Dummy default value

        protected EntityBase()
        {
        }

        protected EntityBase(int id)
            : this()
        {
            Id = id;
        }

        public void SetOwnedBy(string protectKey)
        {
            OwnedBy = protectKey;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EntityBase other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetRealType() != other.GetRealType())
                return false;

            if (Id == 0 || other.Id == 0)
                return false;

            return Id == other.Id;
        }

        public static bool operator ==(EntityBase a, EntityBase b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(EntityBase a, EntityBase b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetRealType().ToString() + Id).GetHashCode();
        }

        private Type GetRealType()
        {
            Type type = GetType();

            if (type.ToString().Contains("Castle.Proxies.") && type.BaseType != null)
                return type.BaseType;

            return type;
        }
    }
}
