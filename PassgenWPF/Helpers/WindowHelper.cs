using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace PassgenWPF.Helpers;

/// <summary>
/// Matches a window's title bar to the app theme on Windows 10 and 11.
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
        if (hwnd == IntPtr.Zero)
            return;

        var darkMode = ThemeManager.IsSystemDarkMode() ? 1 : 0;

        try
        {
            DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE_WIN11, ref darkMode, sizeof(int));
        }
        catch (COMException)
        {
            // Windows 10 builds before 20H1 use an older attribute number. Earlier versions have
            // no dark title bar at all, so a second failure leaves the title bar as it is.
            try
            {
                DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE_WIN10, ref darkMode, sizeof(int));
            }
            catch (COMException)
            {
            }
        }
    }
}
