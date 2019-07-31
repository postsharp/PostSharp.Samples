using PostSharp.Samples.Authorization.Framework;

namespace PostSharp.Samples.Authorization.RoleBased
{
  /// <summary>
  ///   Represents an object that can be secured with the role-based security policy.
  ///   An <see cref="IRoleBasedSecurable" /> must have the ability to determine whether a given subject is a part of a given
  ///   role in the current object.
  /// </summary>
  public interface IRoleBasedSecurable
  {
    /// <summary>
    ///   Gets the object from which roles are inherited.
    /// </summary>
    IRoleBasedSecurable SecurityParent { get; }

    /// <summary>
    ///   Determines whether a given subject is a part of a given role in the current object.
    /// </summary>
    /// <param name="subject">A subject.</param>
    /// <param name="role">A role.</param>
    /// <returns>
    ///   <c>true</c> if <paramref name="subject" /> is a part of <paramref name="role" /> in the current object,
    ///   otherwise <c>null</c>.
    /// </returns>
    bool HasRole(ISubject subject, IRole role);
  }
}