using System;
using System.Threading;

namespace PostSharp.Samples.Logging.BusinessLogic
{
  public class RequestStorage
  {
    public static Request GetRequest(int id)
    {
      Thread.Sleep(4);
      return new Request(id);
    }

    public static User GetUser(int id)
    {
      if (id <= 0)
      {
        throw new ArgumentOutOfRangeException("id", "The user id must be greater than zero.");
      }

      if (id == 14)
      {
        Thread.Sleep(56);
      }

      return new User();
    }
  }
}