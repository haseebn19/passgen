using Xunit;
using PassgenWPF.ViewModels;

namespace PassgenWPF.Tests;

public class MainViewModelTests
{
    [Fact]
    public void Constructor_SetsDefaultValues()
    {
        var vm = new MainViewModel();

        Assert.True(vm.IncludeLowercase);
        Assert.True(vm.IncludeUppercase);
        Assert.True(vm.IncludeNumbers);
        Assert.True(vm.IncludeSymbols);
        Assert.Equal(16, vm.PasswordLength);
        Assert.Equal(12, vm.UniqueChars);
        Assert.Equal(string.Empty, vm.GeneratedPassword);
    }

    [Fact]
    public void CopyButtonText_DefaultValue()
    {
        var vm = new MainViewModel();

        Assert.Equal("Copy", vm.CopyButtonText);
    }

    [Theory]
    [InlineData(true, false, false, false)]
    [InlineData(false, true, false, false)]
    [InlineData(false, false, true, false)]
    [InlineData(false, false, false, true)]
    [InlineData(true, true, true, true)]
    public void CharacterSetOptions_CanBeToggled(bool lower, bool upper, bool numbers, bool symbols)
    {
        var vm = new MainViewModel
        {
            IncludeLowercase = lower,
            IncludeUppercase = upper,
            IncludeNumbers = numbers,
            IncludeSymbols = symbols
        };

        Assert.Equal(lower, vm.IncludeLowercase);
        Assert.Equal(upper, vm.IncludeUppercase);
        Assert.Equal(numbers, vm.IncludeNumbers);
        Assert.Equal(symbols, vm.IncludeSymbols);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(16)]
    [InlineData(32)]
    [InlineData(128)]
    public void PasswordLength_CanBeSet(int length)
    {
        var vm = new MainViewModel { PasswordLength = length };

        Assert.Equal(length, vm.PasswordLength);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(8)]
    [InlineData(16)]
    public void UniqueChars_CanBeSet(int unique)
    {
        var vm = new MainViewModel { UniqueChars = unique };

        Assert.Equal(unique, vm.UniqueChars);
    }

    [Fact]
    public void GeneratedPassword_UpdatesStrength()
    {
        var vm = new MainViewModel { GeneratedPassword = "test" };

        // Allow async strength evaluation to complete
        Thread.Sleep(100);

        Assert.NotEqual(string.Empty, vm.StrengthText);
    }

    [Fact]
    public void IsGenerating_DefaultsFalse()
    {
        var vm = new MainViewModel();

        Assert.False(vm.IsGenerating);
    }

    [Fact]
    public void IsEvaluating_DefaultsFalse()
    {
        var vm = new MainViewModel();

        Assert.False(vm.IsEvaluating);
    }

    [Fact]
    public void StrengthValue_DefaultsToZero()
    {
        var vm = new MainViewModel();

        Assert.Equal(0, vm.StrengthValue);
    }

    [Fact]
    public void GeneratePasswordCommand_Exists()
    {
        var vm = new MainViewModel();

        Assert.NotNull(vm.GeneratePasswordCommand);
    }

    [Fact]
    public void CopyToClipboardCommand_Exists()
    {
        var vm = new MainViewModel();

        Assert.NotNull(vm.CopyToClipboardCommand);
    }
}
