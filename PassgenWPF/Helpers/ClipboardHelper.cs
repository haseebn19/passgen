using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

namespace PassgenWPF.Helpers;

/// <summary>
/// Copies passwords so Windows keeps them out of clipboard history and cloud clipboard sync.
/// </summary>
public static class ClipboardHelper
{
    // A value of 0 in these formats opts the clipboard content out of each feature:
    // https://learn.microsoft.com/windows/win32/dataxchg/clipboard-formats#cloud-clipboard-and-clipboard-history-formats
    private const string HistoryFormat = "CanIncludeInClipboardHistory";
    private const string CloudFormat = "CanUploadToCloudClipboard";

    /// <summary>
    /// Copy the text, returning false if another app is holding the clipboard open.
    /// </summary>
    public static bool TryCopyPrivately(string text)
    {
        var data = new DataObject();
        data.SetText(text, TextDataFormat.UnicodeText);
        data.SetData(HistoryFormat, new MemoryStream(BitConverter.GetBytes(0)));
        data.SetData(CloudFormat, new MemoryStream(BitConverter.GetBytes(0)));

        try
        {
            Clipboard.SetDataObject(data, copy: true);
            return true;
        }
        catch (ExternalException)
        {
            return false;
        }
    }
}
