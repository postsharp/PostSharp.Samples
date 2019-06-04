using PostSharp.Aspects;
using PostSharp.Reflection;
using PostSharp.Serialization;
using System.Web;

namespace PostSharp.Samples.SessionState
{
  /// <summary>
  ///   Aspect that, when applied to a field or property, causes this field or property to be persisted in the session
  ///   state.
  /// </summary>
  [PSerializable]
  [LinesOfCodeAvoided(3)]
  public sealed class SessionStateAttribute : LocationInterceptionAspect
  {
    private string name;

    /// <summary>
    ///   Method invoked at build time.
    /// </summary>
    /// <param name="targetLocation">Field or property to which the aspect has been applied.</param>
    /// <param name="aspectInfo">Ignored.</param>
    public override void CompileTimeInitialize(LocationInfo targetLocation, AspectInfo aspectInfo)
    {
      // Set the name of the session item.
      name = targetLocation.DeclaringType.FullName + "." + targetLocation.Name;
    }

    /// <summary>
    ///   Method invoked when (instead of) the target field or property is retrieved.
    /// </summary>
    /// <param name="args">Context information.</param>
    public override void OnGetValue(LocationInterceptionArgs args)
    {
      var value = HttpContext.Current.Session[name];
      if (value != null)
      {
        args.Value = value;
      }
    }

    /// <summary>
    ///   Method invoked when (instead of) the target field or property is set to a new value.
    /// </summary>
    /// <param name="args">Context information.</param>
    public override void OnSetValue(LocationInterceptionArgs args)
    {
      HttpContext.Current.Session[name] = args.Value;
    }
  }
}