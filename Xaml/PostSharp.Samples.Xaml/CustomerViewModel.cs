using PostSharp.Patterns.Model;

namespace PostSharp.Samples.Xaml
{
  [NotifyPropertyChanged]
  public class CustomerViewModel
  {
    public CustomerModel Customer { get; set; }

    public string FullName
    {
      get
      {
        if (Customer == null)
        {
          return "(No Data)";
        }

        return string.Format("{0} {1} from {2}",
          Customer.FirstName,
          Customer.LastName,
          Customer.PrincipalAddress != null ? Customer.PrincipalAddress.FullAddress : "?");
      }
    }
  }
}