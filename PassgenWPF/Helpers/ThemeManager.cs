using System.Windows;
using Microsoft.Win32;

namespace PassgenWPF.Helpers;

public static class ThemeManager
{
    private static readonly Uri DarkThemeUri = new("/Themes/DarkColors.xaml", UriKind.Relative);
    private static readonly Uri LightThemeUri = new("/Themes/LightColors.xaml", UriKind.Relative);

    public static void ApplySystemTheme()
    {
        var isDark = IsSystemDarkMode();
        ApplyTheme(isDark);
    }

    public static void ApplyTheme(bool isDark)
    {
        var themeUri = isDark ? DarkThemeUri : LightThemeUri;
        var themeDictionary = new ResourceDictionary { Source = themeUri };

        // Find and replace existing theme dictionary
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

    public static bool IsSystemDarkMode()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            return key?.GetValue("AppsUseLightTheme") is int value && value == 0;
        }
        catch
        {
            return true; // Default to dark
        }
    }
}
