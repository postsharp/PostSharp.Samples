using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Reflection;
using PostSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PostSharp.Samples.Encryption
{
  [PSerializable]
  [IntroduceInterface(typeof(IFilterable))]
  public class FilterTypePropertiesAspect : InstanceLevelAspect, IFilterable, IAdviceProvider
  {
    public List<ILocationBinding> bindings;

    private Dictionary<LocationInfo, FilterAttribute> filteredMembers =
      new Dictionary<LocationInfo, FilterAttribute>();

    [PNonSerialized] private bool frozen;

    IEnumerable<AdviceInstance> IAdviceProvider.ProvideAdvices(object targetElement)
    {
      frozen = true;

      // Ask PostSharp to populate the 'bindings' field at runtime.
      var importField = GetType().GetField(nameof(bindings));

      return filteredMembers.Select(filteredMember =>
        new ImportLocationAdviceInstance(importField, filteredMember.Key));
    }

    public void ApplyFilter()
    {
      foreach (var binding in bindings)
      {
        var filter = filteredMembers[binding.LocationInfo];
        binding.SetValue(Instance, filter.ApplyFilter(binding.GetValue(Instance)));
      }
    }

    internal void SetFilter(LocationInfo locationInfo, FilterAttribute filter)
    {
      if (frozen)
        throw new InvalidOperationException();

      filteredMembers.Add(locationInfo, filter);
    }
  }
}