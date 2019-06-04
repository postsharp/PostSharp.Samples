using PostSharp.Samples.Authorization.RoleBased;
using System;

namespace PostSharp.Samples.Authorization.BusinessObjects
{
  public class Role : IRole
  {
    public static readonly Role Owner = new Role(new Guid("B45D4A7B-A68B-40A9-B521-9EC777112164"), "Owner");

    public static readonly Role SalesManager =
      new Role(new Guid("B45D4A7B-A68B-40A9-B521-9EC777112165"), "Sales Manager");

    public static readonly Role Everyone = new Role(new Guid("B45D4A7B-A68B-40A9-B521-9EC777112166"), "Everyone");

    public static readonly Role Administrator =
      new Role(new Guid("B45D4A7B-A68B-40A9-B521-9EC777112167"), "Administrator");

    public Role(Guid id, string name)
    {
      Id = id;
      Name = name;
    }

    public Guid Id { get; }

    public string Name { get; }

    bool IEquatable<IRole>.Equals(IRole other)
    {
      return Equals(other);
    }

    public bool Equals(Role other)
    {
      return Id.Equals(other.Id);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
      {
        return false;
      }

      if (ReferenceEquals(this, obj))
      {
        return true;
      }

      if (obj.GetType() != GetType())
      {
        return false;
      }

      return Equals((Role) obj);
    }

    public override int GetHashCode()
    {
      return Id.GetHashCode();
    }

    public override string ToString()
    {
      return Name;
    }
  }
}