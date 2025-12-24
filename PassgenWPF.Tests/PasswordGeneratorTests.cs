using Xunit;
using PassgenWPF.Services;

namespace PassgenWPF.Tests;

public class PasswordGeneratorTests
{
    private readonly PasswordGenerator _generator = new();

    // ========================================================================
    // SUCCESS CASES
    // ========================================================================

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

    [Fact]
    public void Generate_LowercaseOnly_ContainsOnlyLowercase()
    {
        var options = new PasswordOptions
        {
            IncludeLowercase = true,
            IncludeUppercase = false,
            IncludeNumbers = false,
            IncludeSymbols = false,
            Length = 20,
            UniqueChars = 0
        };

        var result = _generator.Generate(options);

        Assert.True(result.Success);
        Assert.All(result.Password, c => Assert.True(char.IsLower(c)));
    }

    [Fact]
    public void Generate_UppercaseOnly_ContainsOnlyUppercase()
    {
        var options = new PasswordOptions
        {
            IncludeLowercase = false,
            IncludeUppercase = true,
            IncludeNumbers = false,
            IncludeSymbols = false,
            Length = 20,
            UniqueChars = 0
        };

        var result = _generator.Generate(options);

        Assert.True(result.Success);
        Assert.All(result.Password, c => Assert.True(char.IsUpper(c)));
    }

    [Fact]
    public void Generate_NumbersOnly_ContainsOnlyDigits()
    {
        var options = new PasswordOptions
        {
            IncludeLowercase = false,
            IncludeUppercase = false,
            IncludeNumbers = true,
            IncludeSymbols = false,
            Length = 20,
            UniqueChars = 0
        };

        var result = _generator.Generate(options);

        Assert.True(result.Success);
        Assert.All(result.Password, c => Assert.True(char.IsDigit(c)));
    }

    [Fact]
    public void Generate_SymbolsOnly_ContainsOnlySymbols()
    {
        var options = new PasswordOptions
        {
            IncludeLowercase = false,
            IncludeUppercase = false,
            IncludeNumbers = false,
            IncludeSymbols = true,
            Length = 20,
            UniqueChars = 0
        };

        var result = _generator.Generate(options);

        Assert.True(result.Success);
        Assert.All(result.Password, c => Assert.False(char.IsLetterOrDigit(c)));
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

    // ========================================================================
    // ERROR CASES
    // ========================================================================

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
            IncludeNumbers = true,  // Only 10 digits available
            IncludeSymbols = false,
            Length = 20,
            UniqueChars = 15        // Requesting 15 unique from 10 available
        };

        var result = _generator.Generate(options);

        Assert.False(result.Success);
        Assert.Contains("10", result.ErrorMessage);
    }

    // ========================================================================
    // DETERMINISTIC TESTS (with seeded random)
    // ========================================================================

    [Fact]
    public void Generate_WithSeed_ProducesDeterministicResult()
    {
        var gen1 = new PasswordGenerator(new Random(42));
        var gen2 = new PasswordGenerator(new Random(42));
        var options = new PasswordOptions { Length = 20, UniqueChars = 0 };

        var result1 = gen1.Generate(options);
        var result2 = gen2.Generate(options);

        Assert.Equal(result1.Password, result2.Password);
    }

    [Fact]
    public void Generate_DifferentSeeds_ProducesDifferentResults()
    {
        var gen1 = new PasswordGenerator(new Random(42));
        var gen2 = new PasswordGenerator(new Random(99));
        var options = new PasswordOptions { Length = 20, UniqueChars = 0 };

        var result1 = gen1.Generate(options);
        var result2 = gen2.Generate(options);

        Assert.NotEqual(result1.Password, result2.Password);
    }

    // ========================================================================
    // EDGE CASES
    // ========================================================================

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
}
