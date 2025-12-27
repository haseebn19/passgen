using System.Windows;
using PassgenWPF.Helpers;

namespace PassgenWPF.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += (_, _) => WindowHelper.ApplySystemTheme(this);
    }
}
