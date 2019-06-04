using PostSharp.Samples.Transactions.Data;
using System;
using System.Linq;

namespace PostSharp.Samples.Transactions
{
  public static class FinanceService
  {
    [RequiresTransaction]
    public static void Transfer(string source, string destination, decimal amount, string description)
    {
      AddOperation(destination, amount, description);
      AddOperation(source, -amount, description);
    }

    [RequiresTransaction]
    private static void AddOperation(string accountNumber, decimal amount, string description)
    {
      using (var financeDb = new FinanceDb())
      {
        var account = financeDb.Accounts.Single(a => a.Number == accountNumber);
        account.Balance += amount;

        if (account.Balance < account.MinimalBalance)
        {
          throw new InvalidOperationException("Account balance insufficient.");
        }

        financeDb.Operations.Add(new Operation
        {
          Account = account,
          Amount = amount,
          Description = description,
          Time = DateTime.Now
        });
        financeDb.SaveChanges();
      }
    }
  }
}