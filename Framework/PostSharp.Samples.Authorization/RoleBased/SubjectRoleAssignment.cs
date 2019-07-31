using PostSharp.Samples.Authorization.Framework;

namespace PostSharp.Samples.Authorization.RoleBased
{
  /// <summary>
  ///   Represents the fact that a subject is a part of a role.
  /// </summary>
  public class SubjectRoleAssignment
  {
    /// <summary>
    ///   Initializes a new <see cref="SubjectRoleAssignment" />.
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="role"></param>
    public SubjectRoleAssignment(ISubject subject, IRole role)
    {
      Role = role;
      Subject = subject;
    }

    public IRole Role { get; }
    public ISubject Subject { get; }
  }
}