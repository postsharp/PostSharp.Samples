using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Design;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Extensibility;
using PostSharp.Reflection;
using PostSharp.Serialization;

namespace PostSharp.Samples.DependencyResolution.InstanceScoped
{
    public interface ILogger
    {
        void Log(string message);
    }


    [PSerializable]
    public class LogAspect : OnMethodBoundaryAspect, IInstanceScopedAspect
    {
        [IntroduceMember(Visibility = Visibility.Family, OverrideAction = MemberOverrideAction.Ignore)]
        [CopyCustomAttributes(typeof(ImportAttribute))]
        [Import(typeof(ILogger))]
        public ILogger Logger { get; set; }

        [ImportMember("Logger", IsRequired = true)]
        public Property<ILogger> LoggerProperty;

        public override void OnEntry(MethodExecutionArgs args)
        {
            this.LoggerProperty.Get().Log("OnEntry");
        }

        object IInstanceScopedAspect.CreateInstance(AdviceArgs adviceArgs)
        {
            return this.MemberwiseClone();
        }

        void IInstanceScopedAspect.RuntimeInitializeInstance()
        {
        }
    }


    [Export(typeof(MyServiceImpl))]
    internal class MyServiceImpl
    {
        [LogAspect]
        public void LoggedMethod()
        {
            Console.WriteLine("Hello, world.");
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            AssemblyCatalog catalog = new AssemblyCatalog(typeof(Program).Assembly);
            CompositionContainer container = new CompositionContainer(catalog);
            MyServiceImpl service = container.GetExport<MyServiceImpl>().Value;
            service.LoggedMethod();

            Console.ReadLine();
        }
    }


    [Export(typeof(ILogger))]
    internal class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}