using System.Windows;
using PassgenWPF.Helpers;

namespace PassgenWPF;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ThemeManager.ApplySystemTheme();
    }
}
