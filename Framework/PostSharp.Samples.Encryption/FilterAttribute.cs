using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Reflection;
using PostSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PostSharp.Samples.Encryption
{
  [PSerializable]
  [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property)]
  public abstract class FilterAttribute : Attribute, IAspectProvider
  {
    public IEnumerable<AspectInstance> ProvideAspects(object targetElement)
    {
      var parameter = targetElement as ParameterInfo;

      if (parameter != null)
      {
        // When the attribute is applied on a parameter, we have to apply the filter when the method is invoked.

        var method = (MethodBase) parameter.Member;

        // Add an aspect to the method. Make sure we have a single aspect instance even if many parameters need filtering.
        var filterMethodArgumentsAspect = GetAspect<FilterMethodArgumentsAspect>(method);


        if (filterMethodArgumentsAspect == null)
        {
          filterMethodArgumentsAspect = new FilterMethodArgumentsAspect(method);
          yield return new AspectInstance(method, filterMethodArgumentsAspect);
        }

        filterMethodArgumentsAspect.SetFilter(parameter, this);
      }
      else
      {
        // When the attribute is applied on a field or property, we will apply the filter when asked implicitly.

        var locationInfo = LocationInfo.ToLocationInfo(targetElement);

        if (locationInfo.IsStatic)
        {
          Message.Write(locationInfo, SeverityType.Error, "MY02", "Cannot apply [{0}] to {1} because it is static.",
            GetType().Name, locationInfo);
          yield break;
        }

        var type = locationInfo.DeclaringType;

        if (type.IsValueType)
        {
          Message.Write(locationInfo, SeverityType.Error, "MY03",
            "Cannot apply [{0}] to {1} because the declaring type is a struct.", GetType().Name, locationInfo);
          yield break;
        }

        var filterTypePropertiesAspect = GetAspect<FilterTypePropertiesAspect>(type);

        if (filterTypePropertiesAspect == null)
        {
          filterTypePropertiesAspect = new FilterTypePropertiesAspect();
          yield return new AspectInstance(type, filterTypePropertiesAspect);
        }

        filterTypePropertiesAspect.SetFilter(locationInfo, this);
      }
    }

    public abstract object ApplyFilter(object value);

    private static T GetAspect<T>(object target)
    {
      return PostSharpEnvironment.CurrentProject.GetService<IAspectRepositoryService>()
        .GetAspectInstances(target)
        .Select(aspectInstance => aspectInstance.Aspect)
        .OfType<T>()
        .SingleOrDefault();
    }
  }
}