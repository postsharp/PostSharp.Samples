using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Serialization;
using System.IO;


/// <summary>
/// Using a Global Composition Container
/// see code samples at <see href="https://doc.postsharp.net/global-service-container"/>
/// </summary>
namespace PostSharp.Samples.DependencyResolution.GlobalServiceContainer
{
    public interface ILogger
    {
        void Log(string message);
    }

    public static class AspectServiceInjector
    {
        /// <summary>
        /// A class instance of a Managed Extensibility Framework Container.
        /// The Managed Extensibility Framework or MEF is a library for creating 
        /// lightweight, and extensible applications. It allows application 
        /// developers to discover and use extensions with no configuration required.
        /// See the following link for more about MEF, 
        /// <see href="https://docs.microsoft.com/en-us/dotnet/framework/mef/"/>.
        /// </summary>
        private static CompositionContainer container; //

        public static void Initialize(ComposablePartCatalog catalog)
        {
            container = new CompositionContainer(catalog);
        }

        public static void BuildObject(object o)
        {
            if (container == null)
                throw new InvalidOperationException();

            container.SatisfyImportsOnce(o);
        }
    }

    [PSerializable]
    public class LogAspect : OnMethodBoundaryAspect
    {
        [Import] private ILogger logger;


        public override void RuntimeInitialize(MethodBase method)
        {
            AspectServiceInjector.BuildObject(this);
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            logger.Log("OnEntry");
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            logger.Log("OnExit");
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            logger.Log("OnSuccess");
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            bool SHOWCONSOLE = false;

            if (SHOWCONSOLE)
            {
                //Un-comment For Console Logging
                AspectServiceInjector.Initialize(new TypeCatalog(typeof(ConsoleLogger)));
            }
            else
            {
                //Un-comment For File Logging
                AspectServiceInjector.Initialize(new TypeCatalog(typeof(FileLogger)));
            }

            // The static constructor of LogAspect is called before the static constructor of the type
            // containing target methods. This is why we cannot use the aspect in the Program class.
            Foo.LoggedMethod();

            //Pauses the app for viewing.
            Console.ReadLine();
        }
    }

    internal class Foo
    {
        [LogAspect]
        public static void LoggedMethod()
        {
            Console.WriteLine("Hello, world." + Environment.NewLine);
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

    [Export(typeof(ILogger))]
    internal class FileLogger : ILogger
    {
        public void Log(string message)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "FileLoggerOutput.txt");
                File.AppendAllText(path, message + Environment.NewLine);
                Console.WriteLine($"Wrote: {message} to {path}.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n {ex.StackTrace}");
            }
        }
    }
}

