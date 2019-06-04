using JetBrains.Annotations;
using System;

namespace PostSharp.Samples.ResharperAnnotations
{
  internal interface IProgram
  {
    [NotNull]
    string FooFoo { get; set; }

    [NotNull]
    string Foo([NotNull] string bar);
  }

  internal class Program : IProgram
  {
    public string Foo(string bar)
    {
      return null;
    }

    public string FooFoo { get; set; }

    private static void Main(string[] args)
    {
      IProgram p = new Program();

      try
      {
        p.Foo(null);
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }

      try
      {
        p.Foo("");
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }

      try
      {
        p.FooFoo = null;
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }

      try
      {
        p.FooFoo = "";
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }
    }
  }
}