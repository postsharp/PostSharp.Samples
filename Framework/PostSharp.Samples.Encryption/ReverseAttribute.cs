using PostSharp.Aspects;
using PostSharp.Serialization;
using System.Text;

namespace PostSharp.Samples.Encryption
{
  [PSerializable]
  [LinesOfCodeAvoided(2)]
  public sealed class ReverseAttribute : FilterAttribute
  {
    public override object ApplyFilter(object value)
    {
      if (value == null)
      {
        return null;
      }

      var s = (string) value;

      var stringBuilder = new StringBuilder(s.Length);
      for (var i = s.Length - 1; i >= 0; i--)
      {
        stringBuilder.Append(s[i]);
      }

      return stringBuilder.ToString();
    }
  }
}