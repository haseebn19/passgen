using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PassgenWPF.Services;
using Zxcvbn;

namespace PassgenWPF.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IPasswordGenerator _generator;
    private CancellationTokenSource _strengthCts = new();

    public MainViewModel() : this(new PasswordGenerator()) { }

    public MainViewModel(IPasswordGenerator generator)
    {
        _generator = generator;
        IncludeLowercase = true;
        IncludeUppercase = true;
        IncludeNumbers = true;
        IncludeSymbols = true;
        PasswordLength = 16;
        UniqueChars = 12;
        GeneratedPassword = string.Empty;
        StrengthText = string.Empty;
    }

    [ObservableProperty]
    private bool _includeLowercase;

    [ObservableProperty]
    private bool _includeUppercase;

    [ObservableProperty]
    private bool _includeNumbers;

    [ObservableProperty]
    private bool _includeSymbols;

    [ObservableProperty]
    private int _passwordLength;

    [ObservableProperty]
    private int _uniqueChars;

    [ObservableProperty]
    private string _generatedPassword;

    partial void OnGeneratedPasswordChanged(string value) => EvaluateStrengthAsync(value);

    [ObservableProperty]
    private string _strengthText;

    [ObservableProperty]
    private int _strengthValue;

    [ObservableProperty]
    private Brush _strengthBrush = new SolidColorBrush(Colors.Gray);

    [ObservableProperty]
    private bool _isEvaluating;

    [ObservableProperty]
    private bool _isGenerating;

    [ObservableProperty]
    private string _copyButtonText = "📋 Copy";

    [RelayCommand]
    private async Task GeneratePasswordAsync()
    {
        IsGenerating = true;
        try
        {
            var options = new PasswordOptions
            {
                IncludeLowercase = IncludeLowercase,
                IncludeUppercase = IncludeUppercase,
                IncludeNumbers = IncludeNumbers,
                IncludeSymbols = IncludeSymbols,
                Length = PasswordLength,
                UniqueChars = UniqueChars
            };

            var result = await Task.Run(() => _generator.Generate(options));

            if (result.Success)
            {
                GeneratedPassword = result.Password;
            }
            else
            {
                ShowWarning(result.ErrorMessage!, "Invalid Configuration");
            }
        }
        finally
        {
            IsGenerating = false;
        }
    }

    private static void ShowWarning(string message, string title)
    {
        Application.Current.Dispatcher.Invoke(() =>
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning));
    }

    [RelayCommand]
    private async Task CopyToClipboardAsync()
    {
        if (string.IsNullOrEmpty(GeneratedPassword))
        {
            MessageBox.Show("No password to copy.", "Nothing to Copy", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        Clipboard.SetText(GeneratedPassword);
        CopyButtonText = "✓ Copied!";
        await Task.Delay(2000);
        CopyButtonText = "📋 Copy";
    }

    private async void EvaluateStrengthAsync(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            StrengthValue = 0;
            StrengthText = string.Empty;
            StrengthBrush = new SolidColorBrush(Colors.Gray);
            return;
        }

        _strengthCts.Cancel();
        _strengthCts = new CancellationTokenSource();

        IsEvaluating = true;
        StrengthText = "Evaluating...";

        try
        {
            var result = await Task.Run(() => Core.EvaluatePassword(password), _strengthCts.Token);

            StrengthValue = result.Score * 25;
            (StrengthText, StrengthBrush) = result.Score switch
            {
                0 => ("Very Weak", new SolidColorBrush(Color.FromRgb(239, 68, 68))),
                1 => ("Weak", new SolidColorBrush(Color.FromRgb(249, 115, 22))),
                2 => ("Fair", new SolidColorBrush(Color.FromRgb(234, 179, 8))),
                3 => ("Good", new SolidColorBrush(Color.FromRgb(132, 204, 22))),
                4 => ("Strong", new SolidColorBrush(Color.FromRgb(34, 197, 94))),
                _ => (string.Empty, new SolidColorBrush(Colors.Gray))
            };
        }
        catch (OperationCanceledException)
        {
            // Cancelled by newer evaluation
        }
        finally
        {
            IsEvaluating = false;
        }
    }
}
