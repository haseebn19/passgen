using System.Windows;
using Microsoft.Win32;
using PassgenWPF.Helpers;

namespace PassgenWPF;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ThemeManager.ApplySystemTheme();
        SystemEvents.UserPreferenceChanged += OnUserPreferenceChanged;
    }

    protected override void OnExit(ExitEventArgs e)
    {
        SystemEvents.UserPreferenceChanged -= OnUserPreferenceChanged;
        base.OnExit(e);
    }

    // Switching Windows between light and dark mode raises a General preference change,
    // on a system thread rather than the UI thread.
    private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
    {
        if (e.Category != UserPreferenceCategory.General)
            return;

        Dispatcher.Invoke(() =>
        {
            if (!ThemeManager.ApplySystemTheme())
                return;
            foreach (Window window in Windows)
                WindowHelper.ApplySystemTheme(window);
        });
    }
}
