using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Microsoft.Win32;

namespace PassgenWPF.Helpers;

/// <summary>
/// Applies dark/light mode to WPF window title bars on Windows 10/11.
/// </summary>
public static class WindowHelper
{
    [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
    private static extern void DwmSetWindowAttribute(IntPtr hwnd, uint attr, ref int value, uint size);

    private const uint DWMWA_USE_IMMERSIVE_DARK_MODE_WIN11 = 20;
    private const uint DWMWA_USE_IMMERSIVE_DARK_MODE_WIN10 = 19;

    public static void ApplySystemTheme(Window window)
    {
        var hwnd = new WindowInteropHelper(window).Handle;
        if (hwnd == IntPtr.Zero) return;

        var darkMode = IsSystemDarkMode() ? 1 : 0;

        try
        {
            DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE_WIN11, ref darkMode, sizeof(int));
        }
        catch
        {
            try
            {
                DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE_WIN10, ref darkMode, sizeof(int));
            }
            catch
            {
                // Unsupported Windows version
            }
        }
    }

    private static bool IsSystemDarkMode()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            return key?.GetValue("AppsUseLightTheme") is int value && value == 0;
        }
        catch
        {
            return false;
        }
    }
}
