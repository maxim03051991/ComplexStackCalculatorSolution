using ComplexStackCalculator.ViewModels;
using System.Windows;

namespace ComplexStackCalculator.Wpf;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}