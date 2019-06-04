using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Recording.Operations;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace PostSharp.Samples.Xaml
{
  /// <summary>
  ///   Makes the operation names (in the drop-down list of the undo button) nicer.
  /// </summary>
  internal class MyOperationFormatter : OperationFormatter
  {
    public MyOperationFormatter(OperationFormatter next) : base(next)
    {
    }


    private static string SplitString(string input)
    {
      var output = new StringBuilder(input.Length + 8);

      for (var i = 0; i < input.Length; i++)
      {
        if (i > 0 && char.IsUpper(input[i]) && char.IsLower(input[i - 1]))
          output.Append(' ');

        output.Append(input[i]);
      }

      return output.ToString();
    }

    protected override string FormatOperationDescriptor(IOperationDescriptor operation)
    {
      if (operation.OperationKind != OperationKind.Method)
        return null;

      var descriptor = (MethodExecutionOperationDescriptor) operation;


      if (descriptor.Method.IsSpecialName && descriptor.Method.Name.StartsWith("set_"))
      {
        // We have a property setter.

        var property = descriptor.Method.DeclaringType.GetProperty(
          descriptor.Method.Name.Substring(4),
          BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        var attributes =
          (DisplayNameAttribute[]) property.GetCustomAttributes(typeof(DisplayNameAttribute), false);

        string displayName;

        if (attributes.Length > 0)
          displayName = attributes[0].DisplayName;
        else
          displayName = SplitString(descriptor.Method.Name.Substring(4));

        return string.Format("Set {0} to {1}", displayName, descriptor.Arguments[0] ?? "null");
      }
      else
      {
        // We have another method.

        var attributes = (DisplayNameAttribute[])
          descriptor.Method.GetCustomAttributes(typeof(DisplayNameAttribute), false);

        if (attributes.Length > 0)
          return attributes[0].DisplayName;
      }

      return null;
    }
  }
}