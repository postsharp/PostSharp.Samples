using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Serialization;

namespace PostSharp.Samples.DependencyResolution.Contextual
{
    public interface ILogger
    {
        void Log(string message);
    }

    public static class AspectServiceLocator
    {
        private static CompositionContainer container;
        private static HashSet<object> rules = new HashSet<object>();

        public static void Initialize(ComposablePartCatalog catalog)
        {
            container = new CompositionContainer(catalog);
        }

        public static Lazy<T> GetService<T>(Type aspectType, MemberInfo targetElement) where T : class
        {
            return new Lazy<T>(() => GetServiceImpl<T>(aspectType, targetElement));
        }

        private static T GetServiceImpl<T>(Type aspectType, MemberInfo targetElement) where T : class
        {
            // The rule implementation is naive but this is for testing purpose only.
            foreach (object rule in rules)
            {
                DependencyRule<T> typedRule = rule as DependencyRule<T>;
                if (typedRule == null) continue;

                T service = typedRule.Rule(aspectType, targetElement);
                if (service != null) return service;
            }

            if (container == null)
                throw new InvalidOperationException();

            // Fallback to the container, which should be the default and production behavior.
            return container.GetExport<T>().Value;
        }

        public static IDisposable AddRule<T>(Func<Type, MemberInfo, T> rule)
        {
            DependencyRule<T> dependencyRule = new DependencyRule<T>(rule);
            rules.Add(dependencyRule);
            return dependencyRule;
        }

        private class DependencyRule<T> : IDisposable
        {
            public DependencyRule(Func<Type, MemberInfo, T> rule)
            {
                this.Rule = rule;
            }

            public Func<Type, MemberInfo, T> Rule { get; private set; }

            public void Dispose()
            {
                rules.Remove(this);
            }
        }
    }

    [PSerializable]
    public class LogAspect : OnMethodBoundaryAspect
    {
        private Lazy<ILogger> logger;


        public override void RuntimeInitialize(MethodBase method)
        {
            logger = AspectServiceLocator.GetService<ILogger>(this.GetType(), method);
        }


        public override void OnEntry(MethodExecutionArgs args)
        {
            logger.Value.Log("OnEntry");
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            AspectServiceLocator.Initialize(new TypeCatalog(typeof(ConsoleLogger)));

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