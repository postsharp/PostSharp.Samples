using PostSharp.Extensibility;
using PostSharp.Samples.MiniProfiler;
using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("PostSharp.Samples.MiniProfiler")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("PostSharp.Samples.MiniProfiler")]
[assembly: AssemblyCopyright("Copyright ©  2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("8ed28d25-bd81-4995-9e83-3ff6fe82392f")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers
// by using the '*' as shown below:
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// Add the mini-profiler to all services.
[assembly:
  MiniProfilerStep(AttributeTargetTypes = "*Service", AttributeTargetMemberAttributes = MulticastAttributes.Public)]
[assembly:
  MiniProfilerStep(AttributeTargetTypes = "*Controller", AttributeTargetMemberAttributes = MulticastAttributes.Public)]