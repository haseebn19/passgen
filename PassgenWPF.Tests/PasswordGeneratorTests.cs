using PassgenWPF.Services;
using Xunit;

namespace PassgenWPF.Tests;

public class PasswordGeneratorTests
{
    private readonly PasswordGenerator _generator = new();

    [Fact]
    public void Generate_DefaultOptions_ReturnsPassword()
    {
        var options = new PasswordOptions();

        var result = _generator.Generate(options);

        Assert.True(result.Success);
        Assert.Equal(16, result.Password.Length);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(8)]
    [InlineData(16)]
    [InlineData(32)]
    [InlineData(128)]
    public void Generate_VariousLengths_ReturnsCorrectLength(int length)
    {
        var options = new PasswordOptions { Length = length, UniqueChars = 0 };

        var result = _generator.Generate(options);

        Assert.True(result.Success);
        Assert.Equal(length, result.Password.Length);
    }

    [Theory]
    [InlineData(true, false, false, false, CharacterSets.Lowercase)]
    [InlineData(false, true, false, false, CharacterSets.Uppercase)]
    [InlineData(false, false, true, false, CharacterSets.Numbers)]
    [InlineData(false, false, false, true, CharacterSets.Symbols)]
    public void Generate_OneCharacterType_UsesOnlyThatType(bool lower, bool upper, bool numbers, bool symbols, string allowed)
    {
        var options = new PasswordOptions
        {
            IncludeLowercase = lower,
            IncludeUppercase = upper,
            IncludeNumbers = numbers,
            IncludeSymbols = symbols,
            Length = 20,
            UniqueChars = 0
        };

        var result = _generator.Generate(options);

        Assert.True(result.Success);
        Assert.All(result.Password, c => Assert.Contains(c, allowed));
    }

    [Fact]
    public void Generate_AllCharacterSets_MixedCharacters()
    {
        var options = new PasswordOptions { Length = 100, UniqueChars = 0 };

        var result = _generator.Generate(options);

        Assert.True(result.Success);
        Assert.Contains(result.Password, c => char.IsLower(c));
        Assert.Contains(result.Password, c => char.IsUpper(c));
        Assert.Contains(result.Password, c => char.IsDigit(c));
    }

    [Fact]
    public void Generate_UniqueChars_HasRequiredUniqueCount()
    {
        var options = new PasswordOptions { Length = 20, UniqueChars = 15 };

        var result = _generator.Generate(options);

        Assert.True(result.Success);
        var uniqueCount = result.Password.Distinct().Count();
        Assert.True(uniqueCount >= 15);
    }

    [Fact]
    public void Generate_ZeroUniqueChars_Succeeds()
    {
        var options = new PasswordOptions { Length = 10, UniqueChars = 0 };

        var result = _generator.Generate(options);

        Assert.True(result.Success);
        Assert.Equal(10, result.Password.Length);
    }

    [Fact]
    public void Generate_MaxUniqueCharsEqualLength_AllUnique()
    {
        var options = new PasswordOptions { Length = 10, UniqueChars = 10 };

        var result = _generator.Generate(options);

        Assert.True(result.Success);
        Assert.Equal(10, result.Password.Distinct().Count());
    }

    [Fact]
    public void Generate_NoCharacterSets_ReturnsError()
    {
        var options = new PasswordOptions
        {
            IncludeLowercase = false,
            IncludeUppercase = false,
            IncludeNumbers = false,
            IncludeSymbols = false
        };

        var result = _generator.Generate(options);

        Assert.False(result.Success);
        Assert.Contains("character set", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Generate_UniqueCharsExceedsLength_ReturnsError()
    {
        var options = new PasswordOptions { Length = 10, UniqueChars = 15 };

        var result = _generator.Generate(options);

        Assert.False(result.Success);
        Assert.Contains("exceed", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Generate_UniqueCharsExceedsAvailable_ReturnsError()
    {
        var options = new PasswordOptions
        {
            IncludeLowercase = false,
            IncludeUppercase = false,
            IncludeNumbers = true,
            IncludeSymbols = false,
            Length = 20,
            UniqueChars = 15
        };

        var result = _generator.Generate(options);

        Assert.False(result.Success);
        Assert.Contains("10", result.ErrorMessage);
    }

    [Fact]
    public void Generate_MultipleCallsProduceDifferentResults()
    {
        var options = new PasswordOptions { Length = 20, UniqueChars = 0 };

        var result1 = _generator.Generate(options);
        var result2 = _generator.Generate(options);

        Assert.NotEqual(result1.Password, result2.Password);
    }

    [Fact]
    public void Generate_MinimumLength_Succeeds()
    {
        var options = new PasswordOptions { Length = 1, UniqueChars = 0 };

        var result = _generator.Generate(options);

        Assert.True(result.Success);
        Assert.Single(result.Password);
    }

    [Fact]
    public void Generate_LargePassword_Succeeds()
    {
        var options = new PasswordOptions { Length = 1000, UniqueChars = 0 };

        var result = _generator.Generate(options);

        Assert.True(result.Success);
        Assert.Equal(1000, result.Password.Length);
    }

    [Theory]
    [InlineData(true, true, true, true, 94)]
    [InlineData(true, false, false, false, 26)]
    [InlineData(false, false, true, true, 42)]
    [InlineData(false, false, false, false, 0)]
    public void AvailableCharacters_CountsTheSelectedTypes(bool lower, bool upper, bool numbers, bool symbols, int expected)
    {
        var options = new PasswordOptions
        {
            IncludeLowercase = lower,
            IncludeUppercase = upper,
            IncludeNumbers = numbers,
            IncludeSymbols = symbols
        };

        Assert.Equal(expected, options.AvailableCharacters);
    }
}
