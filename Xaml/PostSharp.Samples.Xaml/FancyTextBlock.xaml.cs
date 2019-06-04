using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Xaml;
using System.Windows.Controls;

namespace PostSharp.Samples.Xaml
{
  /// <summary>
  ///   Interaction logic for FancyTextBlock.xaml
  /// </summary>
  public partial class FancyTextBlock : UserControl
  {
    public FancyTextBlock()
    {
      InitializeComponent();
    }


    [DependencyProperty]
    [Required]
    public string Text { get; set; }

    private void OnTextChanged()
    {
      // do something.
    }
  }
}