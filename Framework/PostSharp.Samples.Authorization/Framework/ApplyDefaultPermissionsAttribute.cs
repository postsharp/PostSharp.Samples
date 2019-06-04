using PostSharp.Aspects;
using PostSharp.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PostSharp.Samples.Authorization.Framework
{
  /// <summary>
  ///   Aspect that, when applied to a class, requires the <see cref="StandardPermission.Read" /> and
  ///   <see cref="StandardPermission.Write" />
  ///   on all public fields and properties of this class, unless this field or property already has an
  ///   <see cref="RequiresPermissionBaseAttribute" /> aspect.
  /// </summary>
  [MulticastAttributeUsage(MulticastTargets.Class, Inheritance = MulticastInheritance.Strict)]
  [AttributeUsage(AttributeTargets.Class)]
  public class ApplyDefaultPermissionsAttribute : MulticastAttribute, IAspectProvider
  {
    private static readonly RequiresPermissionAttribute permissionFactory =
      new RequiresPermissionAttribute(StandardPermission.Read, StandardPermission.Write);

    IEnumerable<AspectInstance> IAspectProvider.ProvideAspects(object targetElement)
    {
      var targetType = (Type) targetElement;

      foreach (var location in targetType.GetFields(BindingFlags.Instance | BindingFlags.Public).Union(
        targetType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Cast<MemberInfo>()))
      {
        if (location.IsDefined(typeof(RequiresPermissionBaseAttribute), true))
        {
          continue;
        }

        var aspect = new LocationAuthorizationAspect();
        aspect.AddPermission(0, permissionFactory);

        yield return new AspectInstance(location, aspect);
      }
    }
  }
}