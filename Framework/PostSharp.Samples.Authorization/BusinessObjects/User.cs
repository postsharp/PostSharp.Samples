using PostSharp.Samples.Authorization.Framework;
using System;

namespace PostSharp.Samples.Authorization.BusinessObjects
{
  public class User : ISubject
  {
    public User(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }

    public string Name { get; set; }


    bool IEquatable<ISubject>.Equals(ISubject other)
    {
      return Equals(other);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      return Id.Equals(((User) obj).Id);
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