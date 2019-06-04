using PostSharp.Patterns.Formatters;
using System;
using IFormattable = PostSharp.Patterns.Formatters.IFormattable;

namespace PostSharp.Samples.Logging
{
  internal class ExampleFormattable : IFormattable
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }

    void IFormattable.Format(UnsafeStringBuilder stringBuilder, FormattingRole role)
    {
      stringBuilder.Append("{ExampleFormattable FirstName=\"");
      stringBuilder.Append(FirstName);
      stringBuilder.Append("\", LastName=\"");
      stringBuilder.Append(LastName);
      stringBuilder.Append("}");
    }

    public override string ToString()
    {
      return $"{FirstName} {LastName}";
    }

    public static void Greet(ExampleFormattable formattable)
    {
      Console.WriteLine($"Hello, {formattable}.");
    }
  }
}