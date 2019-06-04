using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;
using System.IO;
using System.Threading;

namespace PostSharp.Samples.Xaml
{
  public class CustomerModel : ModelBase
  {
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    public string Phone { get; set; }
    public string Mobile { get; set; }
    public string Email { get; set; }

    [Child]
    public AdvisableCollection<AddressModel> Addresses { get; set; }

    [Reference]
    public AddressModel PrincipalAddress { get; set; }

    [Reader]
    public void Save(string path)
    {
      using (var stringWriter = new StreamWriter(path))
      {
        // We need to make sure the object graph is not being modified when we save,
        // and this is ensured by [ReaderWriterSynchronized] in ModelBase.

        stringWriter.WriteLine($"FirstName: {FirstName}");
        Thread.Sleep(1000);

        stringWriter.WriteLine($"LastName: {LastName}");
        Thread.Sleep(1000);

        stringWriter.WriteLine($"Phone: {Phone}");
        Thread.Sleep(1000);

        stringWriter.WriteLine($"Mobile: {Mobile}");
        Thread.Sleep(1000);

        stringWriter.WriteLine($"Email: {Email}");
        Thread.Sleep(1000);

        foreach (var address in Addresses)
        {
          Thread.Sleep(1000);

          if (address == PrincipalAddress)
          {
            stringWriter.WriteLine($"Principal address: {address}");
          }
          else
          {
            stringWriter.WriteLine($"Secondary address: {address}");
          }
        }
      }
    }
  }
}