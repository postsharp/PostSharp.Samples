using PostSharp.Samples.Authorization.Framework;
using PostSharp.Samples.Authorization.RoleBased;
using System.Linq;

namespace PostSharp.Samples.Authorization.BusinessObjects
{
  [ApplyDefaultPermissions]
  public abstract class Entity : IRoleBasedSecurable
  {
    protected Entity()
    {
      UserRoleAssignments = new UserRoleAssignmentCollection(this);
    }

    [RequiresPermission(StandardPermission.Read, StandardPermission.Assign)]
    public User Owner { get; set; }

    public UserRoleAssignmentCollection UserRoleAssignments { get; }


    public abstract IRoleBasedSecurable SecurityParent { get; }

    public virtual bool HasRole(ISubject subject, IRole role)
    {
      if (role.Equals(Role.Owner))
        return subject.Equals(Owner);
      if (UserRoleAssignments.Any(a => a.Role.Equals(role) && a.Subject.Equals(subject)))
        return true;
      if (SecurityParent != null)
        return SecurityParent.HasRole(subject, role);
      return false;
    }
  }
}