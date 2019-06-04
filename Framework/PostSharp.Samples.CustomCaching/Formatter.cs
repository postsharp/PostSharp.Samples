using PostSharp.Aspects;
using System;
using System.Text;

namespace PostSharp.Samples.CustomCaching
{
  /// <summary>
  ///   Helps creating a string out of a method call context.
  /// </summary>
  internal static class Formatter
  {
    public static void AppendTypeName(StringBuilder stringBuilder, Type declaringType)
    {
      stringBuilder.Append(declaringType.FullName);
      if (declaringType.IsGenericType)
      {
        var genericArguments = declaringType.GetGenericArguments();
        AppendGenericArguments(stringBuilder, genericArguments);
      }
    }

    public static void AppendGenericArguments(StringBuilder stringBuilder, Type[] genericArguments)
    {
      stringBuilder.Append('<');
      for (var i = 0; i < genericArguments.Length; i++)
      {
        if (i > 0)
          stringBuilder.Append(", ");

        stringBuilder.Append(genericArguments[i].Name);
      }
      stringBuilder.Append('>');
    }

    public static void AppendArguments(StringBuilder stringBuilder, Arguments arguments)
    {
      stringBuilder.Append('(');
      for (var i = 0; i < arguments.Count; i++)
      {
        if (i > 0)
          stringBuilder.Append(", ");

        stringBuilder.Append(arguments[i]);
      }
      stringBuilder.Append(')');
    }
  }
}