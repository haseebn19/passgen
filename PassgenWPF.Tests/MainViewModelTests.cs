using PassgenWPF.ViewModels;
using Xunit;

namespace PassgenWPF.Tests;

public class MainViewModelTests
{
    private const string WeakPassword = "test";
    private const string StrongPassword = "q7#Vb!2xLm$9TzR@4wKe";

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
        Assert.Equal(string.Empty, vm.StrengthText);
        Assert.Equal(0, vm.StrengthValue);
        Assert.Equal("Copy", vm.CopyButtonText);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(16)]
    [InlineData(128)]
    public void GeneratePassword_UsesTheChosenLength(int length)
    {
        var vm = new MainViewModel { PasswordLength = length };

        vm.GeneratePasswordCommand.Execute(null);

        Assert.Equal(length, vm.GeneratedPassword.Length);
    }

    [Fact]
    public void GeneratePassword_IsDisabledWithNoCharacterTypes()
    {
        var vm = new MainViewModel
        {
            IncludeLowercase = false,
            IncludeUppercase = false,
            IncludeNumbers = false,
            IncludeSymbols = false
        };

        Assert.False(vm.GeneratePasswordCommand.CanExecute(null));

        vm.IncludeNumbers = true;
        Assert.True(vm.GeneratePasswordCommand.CanExecute(null));
    }

    [Fact]
    public void CopyToClipboard_IsDisabledUntilThereIsAPassword()
    {
        var vm = new MainViewModel();
        Assert.False(vm.CopyToClipboardCommand.CanExecute(null));

        vm.GeneratePasswordCommand.Execute(null);

        Assert.True(vm.CopyToClipboardCommand.CanExecute(null));
    }

    [Fact]
    public void MaxUniqueChars_IsLimitedByTheCharacterTypes()
    {
        var vm = new MainViewModel
        {
            PasswordLength = 64,
            IncludeUppercase = false,
            IncludeSymbols = false
        };

        Assert.Equal(36, vm.MaxUniqueChars);  // 26 lowercase + 10 numbers
    }

    [Fact]
    public void UniqueChars_IsClampedWhenTheLengthShrinks()
    {
        var vm = new MainViewModel { UniqueChars = 15 };

        vm.PasswordLength = 10;

        Assert.Equal(10, vm.UniqueChars);
    }

    [Fact]
    public void UniqueChars_IsClampedWhenCharacterTypesAreRemoved()
    {
        var vm = new MainViewModel { PasswordLength = 64, UniqueChars = 50 };

        vm.IncludeUppercase = false;
        vm.IncludeSymbols = false;

        Assert.Equal(36, vm.UniqueChars);
    }

    [Fact]
    public async Task Strength_RatesThePassword()
    {
        var vm = new MainViewModel { GeneratedPassword = WeakPassword };
        await vm.StrengthUpdate;

        Assert.Equal("Very Weak", vm.StrengthText);
        Assert.Equal(0, vm.StrengthValue);

        vm.GeneratedPassword = StrongPassword;
        await vm.StrengthUpdate;

        Assert.Equal("Strong", vm.StrengthText);
        Assert.Equal(100, vm.StrengthValue);
    }

    [Fact]
    public async Task Strength_ShowsTheNewestPasswordWhenEditedQuickly()
    {
        var vm = new MainViewModel { GeneratedPassword = StrongPassword };
        var olderUpdate = vm.StrengthUpdate;

        vm.GeneratedPassword = WeakPassword;
        await Task.WhenAll(olderUpdate, vm.StrengthUpdate);

        Assert.Equal("Very Weak", vm.StrengthText);
    }

    [Fact]
    public async Task Strength_ClearsWhenThePasswordIsEmptied()
    {
        var vm = new MainViewModel { GeneratedPassword = StrongPassword };
        await vm.StrengthUpdate;

        vm.GeneratedPassword = string.Empty;
        await vm.StrengthUpdate;

        Assert.Equal(string.Empty, vm.StrengthText);
        Assert.Equal(0, vm.StrengthValue);
    }
}
