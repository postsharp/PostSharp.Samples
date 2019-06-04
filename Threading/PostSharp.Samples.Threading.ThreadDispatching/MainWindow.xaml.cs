using PostSharp.Patterns.Threading;
using System;
using System.Windows;

namespace PostSharp.Samples.Threading.ThreadDispatching
{
  /// <summary>
  ///   Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void startButton_Click(object sender, RoutedEventArgs e)
    {
      EnableControls(false);
      DoStuff();
    }

    [Background]
    private void DoStuff()
    {
      var random = new Random();
      for (var i = 0; i <= 100; i++)
      {
        for (var j = 0; j < 1000000; j++)
        {
          Math.Sin(random.NextDouble());
        }

        SetProgress(i);
      }

      EnableControls(true);
    }

    [Dispatched(true)]
    private void SetProgress(int progress)
    {
      progressBar.Value = progress;
    }

    [Dispatched(true)]
    private void EnableControls(bool enabled)
    {
      startButton.IsEnabled = enabled;
    }
  }
}