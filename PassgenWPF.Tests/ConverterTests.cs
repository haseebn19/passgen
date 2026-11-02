using System.Globalization;
using PassgenWPF.Converters;
using Xunit;

namespace PassgenWPF.Tests;

public class ConverterTests
{
    [Theory]
    [InlineData(0, 200.0, 0.0)]
    [InlineData(50, 200.0, 100.0)]
    [InlineData(100, 200.0, 200.0)]
    [InlineData(150, 200.0, 200.0)]
    public void StrengthToWidthConverter_FillsItsShareOfTheTrack(int strength, double trackWidth, double expected)
    {
        var converter = new StrengthToWidthConverter();
        var values = new object[] { strength, trackWidth };

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
