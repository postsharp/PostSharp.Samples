using PostSharp.Serialization;
using System;

namespace PostSharp.Samples.Authorization.Framework
{
  /// <summary>
  ///   Aspect that, when applied to a field, property or method, requires a permission before the field or property can be
  ///   read or written, or before
  ///   the method is executed. This is a specific implementation of the abstract
  ///   <see cref="RequiresPermissionBaseAttribute" /> class,
  ///   which is limited to standard permissions or simple named permsissions.
  /// </summary>
  [PSerializable]
  public class RequiresPermissionAttribute : RequiresPermissionBaseAttribute
  {
    private string _defaultPermission;
    private string _writePermission;

    /// <summary>
    ///   Initializes a new <see cref="RequiresPermissionAttribute" /> with a single <see cref="StandardPermission" />. This
    ///   constructor
    ///   is meant to be used on methods. If it is used on fields or properties, the same permission will be required both for
    ///   reading and writing.
    /// </summary>
    /// <param name="permission">
    ///   The permission required for executing the target method or reading or writing the target field
    ///   or property.
    /// </param>
    public RequiresPermissionAttribute(StandardPermission permission)
    {
      _defaultPermission = permission.ToString();
    }

    /// <summary>
    ///   Initializes a new <see cref="RequiresPermissionAttribute" /> with a single named permission. This constructor
    ///   is meant to be used on methods. If it is used on fields or properties, the same permission will be required both for
    ///   reading and writing.
    /// </summary>
    /// <param name="permission">
    ///   The permission required for executing the target method or reading or writing the target field
    ///   or property.
    /// </param>
    public RequiresPermissionAttribute(string permission)
    {
      _defaultPermission = permission;
    }

    /// <summary>
    ///   Initializes a new <see cref="RequiresPermissionAttribute" /> and specify a different
    ///   <see cref="StandardPermission" /> for reading or writing a field or property. This constructor
    ///   is meant to be used on fields and properties. If it is used on methods, the value passed to the
    ///   <paramref name="readPermission" /> parameter will be required upon execution.
    /// </summary>
    /// <param name="readPermission">Permission required for reading the field or property.</param>
    /// <param name="writePermission">Permission required for reading the field or property.</param>
    public RequiresPermissionAttribute(StandardPermission readPermission, StandardPermission writePermission)
    {
      _defaultPermission = readPermission.ToString();
      _writePermission = writePermission.ToString();
    }

    /// <summary>
    ///   Initializes a new <see cref="RequiresPermissionAttribute" /> and specify a different named permission for reading or
    ///   writing a field or property. This constructor
    ///   is meant to be used on fields and properties. If it is used on methods, the value passed to the
    ///   <paramref name="readPermission" /> parameter will be required upon execution.
    /// </summary>
    /// <param name="readPermission">Permission required for reading the field or property.</param>
    /// <param name="writePermission">Permission required for reading the field or property.</param>
    public RequiresPermissionAttribute(string readPermission, string writePermission)
    {
      _defaultPermission = readPermission;
      _writePermission = writePermission;
    }

    /// <inheritdoc />
    public override IPermission CreatePermission(OperationSemantic semantic)
    {
      switch (semantic)
      {
        case OperationSemantic.Default:
        case OperationSemantic.Read:
          return new Permission(_defaultPermission);

        case OperationSemantic.Write:
          return new Permission(_writePermission ?? _defaultPermission);

        default:
          throw new ArgumentOutOfRangeException(nameof(semantic), semantic, null);
      }
    }
  }
}