using System.Security;
using System.Windows;
using Microsoft.Win32;

namespace PassgenWPF.Helpers;

/// <summary>
/// Loads the dark or light color dictionary to match the Windows app theme.
/// </summary>
public static class ThemeManager
{
    // Component URIs name this assembly, so they resolve the same whichever app loads it
    private static readonly string AssemblyName = typeof(ThemeManager).Assembly.GetName().Name!;
    private static readonly Uri DarkThemeUri = new($"/{AssemblyName};component/Themes/DarkColors.xaml", UriKind.Relative);
    private static readonly Uri LightThemeUri = new($"/{AssemblyName};component/Themes/LightColors.xaml", UriKind.Relative);

    // The theme last applied, or null before the first call
    private static bool? _appliedDark;

    /// <summary>
    /// Apply the Windows app theme. Returns false if it is already applied.
    /// </summary>
    public static bool ApplySystemTheme() => ApplyTheme(IsSystemDarkMode());

    /// <summary>
    /// Apply the dark or light theme. Returns false if it is already applied.
    /// </summary>
    public static bool ApplyTheme(bool isDark)
    {
        if (_appliedDark == isDark)
            return false;
        _appliedDark = isDark;

        var themeUri = isDark ? DarkThemeUri : LightThemeUri;
        var themeDictionary = new ResourceDictionary { Source = themeUri };

        var existingTheme = FindThemeDictionary();
        if (existingTheme != null)
        {
            var index = Application.Current.Resources.MergedDictionaries.IndexOf(existingTheme);
            Application.Current.Resources.MergedDictionaries[index] = themeDictionary;
        }
        else
        {
            // Insert at beginning so styles can reference colors
            Application.Current.Resources.MergedDictionaries.Insert(0, themeDictionary);
        }
        return true;
    }

    private static ResourceDictionary? FindThemeDictionary()
    {
        foreach (var dict in Application.Current.Resources.MergedDictionaries)
        {
            if (dict.Source?.OriginalString.Contains("Colors.xaml") == true)
                return dict;
        }
        return null;
    }

    /// <summary>
    /// Whether Windows apps are set to dark mode. A missing setting means light, as it does in
    /// Windows, and the app stays dark if the registry can't be read.
    /// </summary>
    public static bool IsSystemDarkMode()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            return key?.GetValue("AppsUseLightTheme") is int value && value == 0;
        }
        catch (SecurityException)
        {
            return true;
        }
    }
}
