using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PassgenWPF.Helpers;
using PassgenWPF.Services;
using Zxcvbn;

namespace PassgenWPF.ViewModels;

public partial class MainViewModel : ObservableObject
{
    // Label and color for each zxcvbn score, from 0 (very weak) to 4 (strong)
    private static readonly (string Text, Brush Brush)[] StrengthLevels =
    [
        ("Very Weak", FrozenBrush(239, 68, 68)),
        ("Weak", FrozenBrush(249, 115, 22)),
        ("Fair", FrozenBrush(234, 179, 8)),
        ("Good", FrozenBrush(132, 204, 22)),
        ("Strong", FrozenBrush(34, 197, 94)),
    ];

    private static readonly Brush NeutralBrush = FrozenBrush(128, 128, 128);

    private readonly IPasswordGenerator _generator;
    private CancellationTokenSource _strengthCts = new();

    public MainViewModel() : this(new PasswordGenerator()) { }

    public MainViewModel(IPasswordGenerator generator)
    {
        _generator = generator;
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(MaxUniqueChars))]
    [NotifyCanExecuteChangedFor(nameof(GeneratePasswordCommand))]
    private bool _includeLowercase = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(MaxUniqueChars))]
    [NotifyCanExecuteChangedFor(nameof(GeneratePasswordCommand))]
    private bool _includeUppercase = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(MaxUniqueChars))]
    [NotifyCanExecuteChangedFor(nameof(GeneratePasswordCommand))]
    private bool _includeNumbers = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(MaxUniqueChars))]
    [NotifyCanExecuteChangedFor(nameof(GeneratePasswordCommand))]
    private bool _includeSymbols = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(MaxUniqueChars))]
    private int _passwordLength = 16;

    partial void OnIncludeLowercaseChanged(bool value) => ClampUniqueChars();
    partial void OnIncludeUppercaseChanged(bool value) => ClampUniqueChars();
    partial void OnIncludeNumbersChanged(bool value) => ClampUniqueChars();
    partial void OnIncludeSymbolsChanged(bool value) => ClampUniqueChars();
    partial void OnPasswordLengthChanged(int value) => ClampUniqueChars();

    private void ClampUniqueChars()
    {
        if (UniqueChars > MaxUniqueChars)
            UniqueChars = MaxUniqueChars;
    }

    /// <summary>
    /// The most unique characters possible with the selected types and length.
    /// </summary>
    public int MaxUniqueChars => Math.Min(CurrentOptions().AvailableCharacters, PasswordLength);

    [ObservableProperty]
    private int _uniqueChars = 12;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CopyToClipboardCommand))]
    private string _generatedPassword = string.Empty;

    // The password box is editable, so typed or pasted passwords are rated too
    partial void OnGeneratedPasswordChanged(string value) => StrengthUpdate = UpdateStrengthAsync(value);

    /// <summary>
    /// The strength rating in progress for the current password.
    /// </summary>
    public Task StrengthUpdate { get; private set; } = Task.CompletedTask;

    [ObservableProperty]
    private string _strengthText = string.Empty;

    [ObservableProperty]
    private int _strengthValue;

    [ObservableProperty]
    private Brush _strengthBrush = NeutralBrush;

    [ObservableProperty]
    private string _copyButtonText = "Copy";

    private PasswordOptions CurrentOptions() => new()
    {
        IncludeLowercase = IncludeLowercase,
        IncludeUppercase = IncludeUppercase,
        IncludeNumbers = IncludeNumbers,
        IncludeSymbols = IncludeSymbols,
        Length = PasswordLength,
        UniqueChars = UniqueChars
    };

    private bool CanGeneratePassword() => IncludeLowercase || IncludeUppercase || IncludeNumbers || IncludeSymbols;

    [RelayCommand(CanExecute = nameof(CanGeneratePassword))]
    private void GeneratePassword()
    {
        var result = _generator.Generate(CurrentOptions());
        if (result.Success)
            GeneratedPassword = result.Password;
        else
            MessageBox.Show(result.ErrorMessage, "Invalid Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    private bool HasPassword() => !string.IsNullOrEmpty(GeneratedPassword);

    [RelayCommand(CanExecute = nameof(HasPassword))]
    private async Task CopyToClipboardAsync()
    {
        CopyButtonText = ClipboardHelper.TryCopyPrivately(GeneratedPassword) ? "Copied!" : "Try Again";
        await Task.Delay(2000);
        CopyButtonText = "Copy";
    }

    private async Task UpdateStrengthAsync(string password)
    {
        // Only the newest password's rating may update the meter
        _strengthCts.Cancel();
        _strengthCts.Dispose();
        _strengthCts = new CancellationTokenSource();
        var token = _strengthCts.Token;

        if (string.IsNullOrEmpty(password))
        {
            ShowStrength(0, string.Empty, NeutralBrush);
            return;
        }

        ShowStrength(0, "Evaluating...", NeutralBrush);
        try
        {
            var result = await Task.Run(() => Core.EvaluatePassword(password), token);
            if (token.IsCancellationRequested)
                return;

            var (text, brush) = StrengthLevels[Math.Clamp(result.Score, 0, StrengthLevels.Length - 1)];
            ShowStrength(result.Score * 25, text, brush);
        }
        catch (OperationCanceledException)
        {
            // A newer password is being rated
        }
    }

    private void ShowStrength(int value, string text, Brush brush)
    {
        StrengthValue = value;
        StrengthText = text;
        StrengthBrush = brush;
    }

    private static SolidColorBrush FrozenBrush(byte r, byte g, byte b)
    {
        var brush = new SolidColorBrush(Color.FromRgb(r, g, b));
        brush.Freeze();
        return brush;
    }
}
