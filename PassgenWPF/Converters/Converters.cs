using System.Globalization;
using System.Windows.Data;

namespace PassgenWPF.Converters;

/// <summary>
/// Turns a strength from 0 to 100 and the width of the meter's track into the width of its fill.
/// </summary>
public class StrengthToWidthConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length != 2 || values[0] is not int strength || values[1] is not double trackWidth)
            return 0.0;

        return trackWidth * Math.Clamp(strength, 0, 100) / 100.0;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
