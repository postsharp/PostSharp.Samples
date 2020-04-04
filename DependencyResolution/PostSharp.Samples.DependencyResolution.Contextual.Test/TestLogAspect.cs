using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PostSharp.Samples.DependencyResolution.Contextual.Test
{
    [TestClass]
    public class TestLogAspect
    {
        [TestMethod]
        public void TestMethod()
        {
            // The ServiceLocator can be initialized for each test.
            using (
                AspectServiceLocator.AddRule<ILogger>(
                    (type, member) =>
                    type == typeof(LogAspect) && member.Name == "TargetMethod" ? new TestLogger() : null)
                )
            {
                TestLogger.Clear();
                TargetMethod();
                Assert.AreEqual("OnEntry" + Environment.NewLine, TestLogger.GetLog());
            }
        }

        [LogAspect]
        public void TargetMethod()
        {
        }
    }

    internal class TestLogger : ILogger
    {
        public static readonly StringBuilder stringBuilder = new StringBuilder();

        public void Log(string message)
        {
            stringBuilder.AppendLine(message);
        }

        public static string GetLog()
        {
            return stringBuilder.ToString();
        }

        public static void Clear()
        {
            stringBuilder.Clear();
        }
    }
}