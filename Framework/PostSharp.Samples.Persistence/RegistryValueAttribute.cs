using Microsoft.Win32;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Reflection;
using PostSharp.Serialization;
using System;

namespace PostSharp.Samples.Persistence
{
  [PSerializable]
  [LinesOfCodeAvoided(5)]
  [MulticastAttributeUsage(TargetMemberAttributes = MulticastAttributes.Static)]
  public sealed class RegistryValueAttribute : LocationInterceptionAspect
  {
    private bool isFetched;
    private string keyFullName;
    private string keyName;
    private string valueName;

    public RegistryValueAttribute(string keyName)
    {
      this.keyName = keyName;
    }


    public override void CompileTimeInitialize(LocationInfo targetLocation, AspectInfo aspectInfo)
    {
      keyFullName = "HKEY_CURRENT_USER\\Software\\" + keyName;
      valueName = targetLocation.Name;
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

        var stringValue = Registry.GetValue(keyFullName, valueName, null) as string;
        if (stringValue != null)
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
      args.ProceedSetValue();
      Registry.SetValue(keyFullName, valueName, Convert.ToString(args.Value));
      isFetched = true;
    }
  }
}