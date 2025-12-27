using Xunit;
using PassgenWPF.Converters;
using System.Globalization;
using System.Windows;

namespace PassgenWPF.Tests;

public class ConverterTests
{
    [Theory]
    [InlineData(true, Visibility.Visible)]
    [InlineData(false, Visibility.Collapsed)]
    public void BoolToVisibilityConverter_ConvertsCorrectly(bool input, Visibility expected)
    {
        var converter = new BoolToVisibilityConverter();

        var result = converter.Convert(input, typeof(Visibility), null!, CultureInfo.InvariantCulture);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(true, Visibility.Collapsed)]
    [InlineData(false, Visibility.Visible)]
    public void InvertedBoolToVisibilityConverter_ConvertsCorrectly(bool input, Visibility expected)
    {
        var converter = new InvertedBoolToVisibilityConverter();

        var result = converter.Convert(input, typeof(Visibility), null!, CultureInfo.InvariantCulture);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public void InverseBoolConverter_InvertsValue(bool input, bool expected)
    {
        var converter = new InverseBoolConverter();

        var result = converter.Convert(input, typeof(bool), null!, CultureInfo.InvariantCulture);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void InverseBoolConverter_ProvideValue_ReturnsSelf()
    {
        var converter = new InverseBoolConverter();

        var result = converter.ProvideValue(null!);

        Assert.Same(converter, result);
    }

    [Theory]
    [InlineData(0, 200.0, 0.0)]
    [InlineData(50, 200.0, 59.0)]    // (200-82) * 0.5 = 59
    [InlineData(100, 200.0, 118.0)]  // (200-82) * 1.0 = 118
    public void StrengthToWidthConverter_CalculatesWidth(int strength, double containerWidth, double expected)
    {
        var converter = new StrengthToWidthConverter();
        var values = new object[] { strength, containerWidth };

        var result = converter.Convert(values, typeof(double), null!, CultureInfo.InvariantCulture);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void StrengthToWidthConverter_InvalidInput_ReturnsZero()
    {
        var converter = new StrengthToWidthConverter();
        var values = new object[] { "invalid", 200.0 };

        var result = converter.Convert(values, typeof(double), null!, CultureInfo.InvariantCulture);

        Assert.Equal(0.0, result);
    }
}
