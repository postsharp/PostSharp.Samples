using PostSharp.Samples.Authorization.BusinessObjects;
using PostSharp.Samples.Authorization.Framework;
using PostSharp.Samples.Authorization.RoleBased;
using System;
using System.Security;
using SecurityContext = PostSharp.Samples.Authorization.Framework.SecurityContext;

namespace PostSharp.Samples.Authorization
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      var securityPolicy = new RoleBasedSecurityPolicy();

      // Set up role-permission assignments. Depending on your application, this can be hardcoded or stored in a database.

      // By default, everybody can read an entity but only the owner can write it.
      securityPolicy.AddRolePermissionAssignment(typeof(object), Permission.Read, Role.Everyone,
        PermissionAction.Grant);
      securityPolicy.AddRolePermissionAssignment(typeof(object), Permission.Write, Role.Owner, PermissionAction.Grant);
      securityPolicy.AddRolePermissionAssignment(typeof(object), Permission.Assign, Role.Owner, PermissionAction.Grant);

      // Sales managers have Write and Assign rights to invoices in their business unit.
      securityPolicy.AddRolePermissionAssignment(typeof(Invoice), Permission.Write, Role.SalesManager,
        PermissionAction.Grant);
      securityPolicy.AddRolePermissionAssignment(typeof(Invoice), Permission.Assign, Role.SalesManager,
        PermissionAction.Grant);

      // Administrators have the right to assign roles.
      securityPolicy.AddRolePermissionAssignment(typeof(object), Permission.ManageRoles, Role.Administrator,
        PermissionAction.Grant);


      // Set up an object graph. This would typically be stored in a database.
      // Note that security is disabled at this point because SecurityContext.Current is null.
      var company = new BusinessUnit { Name = "Contoso s.r.o." };

      var mikki = new User(Guid.NewGuid()) { Name = "Mikki Grisham" };
      company.UserRoleAssignments.Add(mikki, Role.Everyone);


      var silva = new User(Guid.NewGuid()) { Name = "Silva Pollard" };
      company.UserRoleAssignments.Add(silva, Role.Everyone);

      var admin = new User(Guid.NewGuid()) { Name = "Administrator" };
      company.UserRoleAssignments.Add(admin, Role.Everyone);
      company.UserRoleAssignments.Add(admin, Role.Administrator);


      var department = new BusinessUnit { ParentUnit = company, Name = "Trolls & gnomes wholesale" };
      var invoice = new Invoice
      {
        Owner = mikki,
        BusinessUnit = department,
        Amount = 50,
        Description = "Kuroji trolls XXL"
      };


      // Now enable security.
      var context = new SimpleSecurityContext
      {
        Policy = securityPolicy
      };

      SecurityContext.Current = context;


      // Test some operations.
      context.Subject = mikki;
      ShouldNotThrow(() => invoice.Amount = 53, "Changing the invoice amount as mikki");

      context.Subject = silva;
      ShouldThrow(() => invoice.Amount = 53, "Changing the invoice amount as silva");

      context.Subject = mikki;
      ShouldThrow(() => department.UserRoleAssignments.Add(silva, Role.SalesManager), "Changing roles as mikki");

      context.Subject = admin;
      ShouldNotThrow(() => department.UserRoleAssignments.Add(silva, Role.SalesManager), "Changing roles as admin");


      context.Subject = silva;
      ShouldNotThrow(() => invoice.Amount = 53, "Changing the invoice amount as silva");
    }

    public static void ShouldThrow(Action action, string description)
    {
      try
      {
        action();

        Console.WriteLine($"BAD. The operation '{description}' has succeeded but should have failed.");
      }
      catch (SecurityException e)
      {
        Console.WriteLine($"GOOD. The operation '{description}' has failed as expected: {e.Message}");
      }
    }

    public static void ShouldNotThrow(Action action, string description)
    {
      try
      {
        action();

        Console.WriteLine($"GOOD. The operation '{description}' has succeeded as expected.");
      }
      catch (SecurityException e)
      {
        Console.WriteLine($"BAD. The operation '{description}' has failed: {e.Message}");
      }
    }


    private class SimpleSecurityContext : ISecurityContext
    {
      public ISubject Subject { get; set; }
      public ISecurityPolicy Policy { get; set; }
      public ISecurityExceptionHandler ExceptionHandler { get; set; }
    }
  }
}