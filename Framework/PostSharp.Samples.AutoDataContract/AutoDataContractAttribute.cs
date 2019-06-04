using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace PostSharp.Samples.AutoDataContract
{
  // We set up multicast inheritance so  the aspect is automatically added to children types. This is optional.
  [MulticastAttributeUsage(Inheritance = MulticastInheritance.Strict)]

  // Since we want the aspect to be applied on types, we derive our class from TypeLevelAspect.
  // If you want to have a project-wide aspect provider, derive the class from AssemblyLevelAspect.
  public class AutoDataContractAttribute : TypeLevelAspect, IAspectProvider
  {
    // This method is called at build time and should just provide other aspects.

    public IEnumerable<AspectInstance> ProvideAspects(object targetElement)
    {
      var targetType = (Type) targetElement;

      var introduceDataContractAspect =
        new CustomAttributeIntroductionAspect(
          new ObjectConstruction(typeof(DataContractAttribute).GetConstructor(Type.EmptyTypes)));

      var introduceDataMemberAspect =
        new CustomAttributeIntroductionAspect(
          new ObjectConstruction(typeof(DataMemberAttribute).GetConstructor(Type.EmptyTypes)));


      // Add the DataContract attribute to the type.
      yield return new AspectInstance(targetType, introduceDataContractAspect);

      // Add a DataMember attribute to every relevant property.
      foreach (var property in
        targetType.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
        if (property.CanWrite && !property.IsDefined(typeof(NotDataMemberAttribute)))
          yield return new AspectInstance(property, introduceDataMemberAspect);
    }
  }
}