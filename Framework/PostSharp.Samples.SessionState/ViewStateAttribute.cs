using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Extensibility;
using PostSharp.Reflection;
using PostSharp.Serialization;
using System.Web.UI;

namespace PostSharp.Samples.SessionState
{
  /// <summary>
  ///   Aspect that, when applied to an instance field or property of a <see cref="Page" /> or <see cref="Control" />,
  ///   causes this field or property to be persisted in the view state state.
  /// </summary>
  [PSerializable]
  [LinesOfCodeAvoided(3)]
  [MulticastAttributeUsage(TargetTypeAttributes = MulticastAttributes.Instance)]
  public sealed class ViewStateAttribute : LocationInterceptionAspect, IInstanceScopedAspect
  {
    private string name;

    // At run-time, will be set to a delegate giving access to the 'ViewState' protected method.
    [ImportMember("ViewState")] public Property<StateBag> ViewState;

    /// <summary>
    ///   Creates an instance of the aspect for a specific instance of the <see cref="Control" /> or <see cref="Page" />.
    /// </summary>
    object IInstanceScopedAspect.CreateInstance(AdviceArgs adviceArgs)
    {
      return MemberwiseClone();
    }

    void IInstanceScopedAspect.RuntimeInitializeInstance()
    {
    }

    /// <summary>
    ///   Method invoked at build-time to validate that the aspect has been applied to a valid field or property.
    /// </summary>
    /// <param name="locationInfo">Field or property to which the aspect has been applied.</param>
    /// <returns>
    ///   <c>true</c> if <paramref name="locationInfo" /> is a valid target for the current aspect, otherwise
    ///   <c>false</c>.
    /// </returns>
    public override bool CompileTimeValidate(LocationInfo locationInfo)
    {
      if (!typeof(Control).IsAssignableFrom(locationInfo.DeclaringType))
      {
        Message.Write(locationInfo, SeverityType.Error, "VS01",
          "Cannot apply [ViewState] to {0} because the declaring type is not a Control or Page.", locationInfo);
        return false;
      }

      return true;
    }

    /// <summary>
    ///   Method invoked at build-time to initialize the current aspect.
    /// </summary>
    /// <param name="targetLocation">Field or property to which the aspect has been applied.</param>
    public override void CompileTimeInitialize(LocationInfo targetLocation, AspectInfo aspectInfo)
    {
      // Set the name of the page view.
      name = targetLocation.DeclaringType.FullName + "." + targetLocation.Name;
    }

    /// <summary>
    ///   Method invoked when (instead of) the target field or property is retrieved.
    /// </summary>
    /// <param name="args">Context information.</param>
    public override void OnGetValue(LocationInterceptionArgs args)
    {
      var value = ViewState.Get()[name];

      if (value != null)
        args.Value = value;
    }

    /// <summary>
    ///   Method invoked when (instead of) the target field or property is set to a new value.
    /// </summary>
    /// <param name="args">Context information.</param>
    public override void OnSetValue(LocationInterceptionArgs args)
    {
      ViewState.Get()[name] = args.Value;
    }
  }
}