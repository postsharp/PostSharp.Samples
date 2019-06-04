using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostSharp.Samples.AutoRetry
{

  [TestClass]
  public class AutoRetryTest
  {
    int counter;


    // Success tests: the number of failures is SMALLER than the number of retries.
    [DataTestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(2)]
    public void SucceedingTest(int failures)
    {
      counter = 0;
      TestMethod(failures);
      
    }


    // Failure tests: the number of failures is SMALLER than the number of retries.
    [DataTestMethod]
    [DataRow(3)]
    [DataRow(4)]
    [ExpectedException(typeof(TestException))]

    public void FailingTest(int failures)
    {
      counter = 0;
      TestMethod(failures);

    }




    [AutoRetry(MaxRetries = 3, HandledExceptions = new Type[] { typeof(TestException) }, Delay = 0)]
    private void TestMethod(int failures)
    {
      this.counter++;

      if (this.counter - 1 <= failures)
      {
        throw new TestException();
      }

    }

  }


  public class TestException : Exception
  {
  }
}
