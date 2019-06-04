using PostSharp.Samples.Authorization.Framework;
using PostSharp.Samples.Authorization.RoleBased;
using System.Collections.ObjectModel;

namespace PostSharp.Samples.Authorization.BusinessObjects
{
  public class UserRoleAssignmentCollection : Collection<SubjectRoleAssignment>, IRoleBasedSecurable
  {
    public UserRoleAssignmentCollection(Entity parent)
    {
      Parent = parent;
    }

    public Entity Parent { get; }

    IRoleBasedSecurable IRoleBasedSecurable.SecurityParent => Parent;

    public bool HasRole(ISubject subject, IRole role)
    {
      return Parent.HasRole(subject, role);
    }

    [RequiresPermission(StandardPermission.ManageRoles)]
    protected override void ClearItems()
    {
      base.ClearItems();
    }

    [RequiresPermission(StandardPermission.ManageRoles)]
    protected override void InsertItem(int index, SubjectRoleAssignment item)
    {
      base.InsertItem(index, item);
    }

    [RequiresPermission(StandardPermission.ManageRoles)]
    protected override void RemoveItem(int index)
    {
      base.RemoveItem(index);
    }

    [RequiresPermission(StandardPermission.ManageRoles)]
    protected override void SetItem(int index, SubjectRoleAssignment item)
    {
      base.SetItem(index, item);
    }

    public void Add(User user, Role role)
    {
      Add(new SubjectRoleAssignment(user, role));
    }
  }
}