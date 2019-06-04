using PostSharp.Samples.Transactions.Data;
using System;
using System.Diagnostics;
using System.IO;

namespace PostSharp.Samples.Transactions
{
  internal static class Program
  {
    private const string itDepartment = "00-001";
    private const string hrDepartment = "00-002";

    private static void Main(string[] args)
    {
      // Initialization.
      CheckDtcRunning();
      CreateDatabases();

      // Perform a first transaction. It will be successful.
      Console.WriteLine("Placing and paying one order...");
      Console.WriteLine();
      IntegrationService.PlaceOrderAndPay(hrDepartment, itDepartment, "Employee Benefit System", 450000);
      DumpDatabases();
      Console.WriteLine();
      Console.WriteLine();

      // Perform a second transaction. It will fail.
      Console.WriteLine("Placing and paying a second order...");
      try
      {
        IntegrationService.PlaceOrderAndPay(hrDepartment, itDepartment, "Employee Benefit System Rev 1", 75000);
      }
      catch (Exception e)
      {
        Console.WriteLine("ERROR! " + e.Message);
        Console.WriteLine(
          "The transaction should be rolled back and the database state identical as before the transaction.");
      }
      Console.WriteLine();
      DumpDatabases();
    }

    private static void CheckDtcRunning()
    {
      while (Process.GetProcessesByName("msdtc").Length == 0)
      {
        Console.WriteLine("Distributed Transaction Coordinator (MSDTC) service is not running.");
        Console.WriteLine(
          "Execute 'net start msdtc' from an administrative command prompt and press Enter.");
        Console.ReadLine();
      }
    }

    private static void DumpDatabases()
    {
      using (var financeDb = new FinanceDb())
      {
        foreach (var account in financeDb.Accounts)
        {
          Console.WriteLine("Account {0} ({1}) - Balance ${2}", account.Number, account.Name,
            account.Balance);
          Console.WriteLine(
            "------------------------------------------------------------------------------------------");
          foreach (var operation in account.Operations)
          {
            Console.WriteLine("{0} - {1} - ${2}", operation.Time, operation.Description, operation.Amount);
          }

          Console.WriteLine();
        }
      }

      using (var orderDb = new OrderDb())
      {
        Console.WriteLine("Orders");
        Console.WriteLine("----------------------------------------------------------------------------");
        foreach (var order in orderDb.Orders)
        {
          Console.WriteLine("{0} - {1}", order.Description, order.TotalAmount);
        }
      }
    }

    private static void CreateDatabases()
    {
      var dataDirectory = Path.Combine(Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]));
      AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectory);

      using (var financeDb = new FinanceDb())
      {
        if (!financeDb.Database.CreateIfNotExists())
        {
          financeDb.Operations.RemoveRange(financeDb.Operations);
          financeDb.Accounts.RemoveRange(financeDb.Accounts);
        }

        financeDb.Accounts.Add(new Account
        {
          Name = "IT Department",
          Number = itDepartment
        });

        financeDb.Accounts.Add(new Account
        {
          Name = "HR Department",
          Number = hrDepartment,
          Balance = 500000
        });
        financeDb.SaveChanges();
      }

      using (var orderDb = new OrderDb())
      {
        if (!orderDb.Database.CreateIfNotExists())
        {
          orderDb.Orders.RemoveRange(orderDb.Orders);
          orderDb.SaveChanges();
        }
      }
    }
  }
}