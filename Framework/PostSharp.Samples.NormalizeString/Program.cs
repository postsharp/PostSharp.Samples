using System;

namespace PostSharp.Samples.NormalizeString
{
  class Program
  {
    [NormalizeString]
    static string myField;

    static void Main(string[] args)
    {

      myField = "   Hello, world.    ";

      Console.WriteLine("\"" + myField + "\"");

    }
  }
}
