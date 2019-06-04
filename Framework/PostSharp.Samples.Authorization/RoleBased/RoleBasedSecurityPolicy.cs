using PostSharp.Samples.Authorization.Framework;
using System;
using System.Collections.Generic;

namespace PostSharp.Samples.Authorization.RoleBased
{
  /// <summary>
  ///   Implementation of <see cref="ISecurityPolicy" /> for the role-based model.
  /// </summary>
  public class RoleBasedSecurityPolicy : ISecurityPolicy
  {
    private readonly Dictionary<Type, List<Assignment>> rolePermissionAssignments =
      new Dictionary<Type, List<Assignment>>();

    /// <inheritdoc />
    public bool Evaluate(ISubject subject, IPermission permission, object securable)
    {
      var securableAncestor = (IRoleBasedSecurable) securable;
      var granted = false;
      for (var type = securableAncestor.GetType(); type != null; type = type.BaseType)
      {
        List<Assignment> assignments;
        if (rolePermissionAssignments.TryGetValue(type, out assignments))
          foreach (var assignment in assignments)
            if (assignment.Permission.Equals(permission) && securableAncestor.HasRole(subject, assignment.Role))
              switch (assignment.Action)
              {
                case PermissionAction.Grant:
                  granted = true;
                  break;

                case PermissionAction.Revoke:
                  // If only one permission evaluates to Revoke, the permission is refused.
                  return false;
              }
      }

      return granted;
    }


    /// <summary>
    ///   Grants or revokes a permission to or from members of a role for a given type of entity.
    /// </summary>
    /// <param name="entitytype">
    ///   Base type of entities for which the permission is granted. To assign the permission to all
    ///   entity types, pass <c>typeof(object)</c>.
    /// </param>
    /// <param name="permission">The permission being granted or revoked.</param>
    /// <param name="role">The role for which the permissio is being granted or revoked.</param>
    /// <param name="action">Whether the permission is being granted or revoked.</param>
    public void AddRolePermissionAssignment(Type entitytype, IPermission permission, IRole role,
      PermissionAction action)
    {
      var assignment = new Assignment(permission, role, action);
      List<Assignment> list;
      if (!rolePermissionAssignments.TryGetValue(entitytype, out list))
      {
        list = new List<Assignment>();
        rolePermissionAssignments.Add(entitytype, list);
      }

      list.Add(assignment);
    }

    private class Assignment
    {
      internal Assignment(IPermission permission, IRole role, PermissionAction action)
      {
        Action = action;
        Permission = permission;
        Role = role;
      }

      public PermissionAction Action { get; }

      public IPermission Permission { get; }

      public IRole Role { get; }

      public override string ToString()
      {
        return $"{Action} {Permission} to {Role}";
      }
    }
  }
}