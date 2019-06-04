using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Reflection;
using PostSharp.Serialization;
using System;
using System.Configuration;

namespace PostSharp.Samples.Persistence
{
  [PSerializable]
  [LinesOfCodeAvoided(2)]
  [MulticastAttributeUsage(TargetMemberAttributes = MulticastAttributes.Static)]
  public sealed class AppSettingsValueAttribute : LocationInterceptionAspect
  {
    private bool isFetched;
    private string settingName;

    public AppSettingsValueAttribute()
    {
    }

    public AppSettingsValueAttribute(string settingName)
    {
      this.settingName = settingName;
    }

    public override void CompileTimeInitialize(LocationInfo targetLocation, AspectInfo aspectInfo)
    {
      if (settingName == null)
        settingName = targetLocation.Name;
    }

    public override bool CompileTimeValidate(LocationInfo locationInfo)
    {
      bool isReadOnly;
      switch (locationInfo.LocationKind)
      {
        case LocationKind.Field:
          isReadOnly = locationInfo.FieldInfo.IsInitOnly;
          break;

        case LocationKind.Property:
          isReadOnly = locationInfo.PropertyInfo.SetMethod == null;
          break;

        default:
          throw new Exception();
      }

      if (isReadOnly)
      {
        Message.Write(locationInfo, SeverityType.Error, "MY001",
          "Cannot apply AppSettingsValueAttribute to a read-only field or property {0}.", locationInfo);
        return false;
      }

      return true;
    }

    public override void OnGetValue(LocationInterceptionArgs args)
    {
      if (isFetched)
      {
        args.ProceedGetValue();
      }
      else
      {
        isFetched = true;

        var stringValue = ConfigurationManager.AppSettings[settingName];

        if (!string.IsNullOrEmpty(stringValue))
        {
          var value = Convert.ChangeType(stringValue, args.Location.LocationType);
          args.SetNewValue(value);
          args.Value = value;
        }
        else
        {
          args.ProceedGetValue();
        }
      }
    }

    public override void OnSetValue(LocationInterceptionArgs args)
    {
      if (isFetched)
        throw new InvalidOperationException(
          "The value of this field or property should not be changed after it has been read for the first time.");

      base.OnSetValue(args);
    }
  }
}