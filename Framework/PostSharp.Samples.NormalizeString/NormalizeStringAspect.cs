using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Reflection;
using PostSharp.Serialization;

namespace PostSharp.Samples.NormalizeString
{
  [PSerializable]
  class NormalizeStringAttribute : LocationInterceptionAspect
  {
    public override bool CompileTimeValidate(LocationInfo locationInfo)
    {
      if (locationInfo.LocationType != typeof(string))
      {
        Message.Write(locationInfo, SeverityType.Error, "MY001", "[NormalizeString] cannot be applied to {0} because its type is not string.", locationInfo);
        return false;
      }

      return true;
    }
    public override void OnSetValue(LocationInterceptionArgs args)
    {
      if (args.Value != null)
      {
        args.Value = ((string) args.Value).Trim().ToLowerInvariant();
      }

      args.ProceedSetValue();
    }
  }
}
