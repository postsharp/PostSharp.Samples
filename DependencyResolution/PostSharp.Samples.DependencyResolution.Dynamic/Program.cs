using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Serialization;

namespace PostSharp.Samples.DependencyResolution.Dynamic
{
    public interface ILogger
    {
        void Log(string message);
    }

    public static class AspectServiceLocator
    {
        private static CompositionContainer container;
        private static bool isCacheable;

        public static void Initialize(ComposablePartCatalog catalog, bool isCacheable)
        {
            if (AspectServiceLocator.isCacheable && container != null)
                throw new InvalidOperationException();

            container = new CompositionContainer(catalog);
            AspectServiceLocator.isCacheable = isCacheable;
        }

        public static Func<T> GetService<T>() where T : class
        {
            if (isCacheable)
            {
                return () => new Lazy<T>(GetServiceImpl<T>).Value;
            }
            else
            {
                return GetServiceImpl<T>;
            }
        }

        private static T GetServiceImpl<T>()
        {
            if (container == null)
                throw new InvalidOperationException();

            return container.GetExport<T>().Value;
        }
    }

    [PSerializable]
    public class LogAspect : OnMethodBoundaryAspect
    {
        private static readonly Func<ILogger> logger;

        static LogAspect()
        {
            if (!PostSharpEnvironment.IsPostSharpRunning)
            {
                logger = AspectServiceLocator.GetService<ILogger>();
            }
        }


        public override void OnEntry(MethodExecutionArgs args)
        {
            logger().Log("OnEntry");
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            AspectServiceLocator.Initialize(new TypeCatalog(typeof(ConsoleLogger)), true);

            LoggedMethod();

            Console.ReadLine();
        }

        [LogAspect]
        public static void LoggedMethod()
        {
            Console.WriteLine("Hello, world.");
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