using System;

namespace PostSharp.Samples.Encryption
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      Login(new LoginData { Login = "login", Password = "password" });
      Login("login", "password");
    }

    private static void Login([ApplyFilters] LoginData loginData)
    {
      Console.WriteLine($"Login={loginData.Login}, Password={loginData.Password}");
    }

    private static void Login(string login, [Reverse] string password)
    {
      Console.WriteLine($"Login={login}, Password={password}");
    }
  }

  internal class LoginData
  {
    public string Login;

    [Reverse] public string Password;
  }
}