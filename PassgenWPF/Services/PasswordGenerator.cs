using System.Security.Cryptography;

namespace PassgenWPF.Services;

/// <summary>
/// The characters each character type can contribute to a password.
/// </summary>
public static class CharacterSets
{
    public const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
    public const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public const string Numbers = "0123456789";
    public const string Symbols = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";
}

public class PasswordOptions
{
    public bool IncludeLowercase { get; init; } = true;
    public bool IncludeUppercase { get; init; } = true;
    public bool IncludeNumbers { get; init; } = true;
    public bool IncludeSymbols { get; init; } = true;
    public int Length { get; init; } = 16;
    public int UniqueChars { get; init; } = 12;

    /// <summary>
    /// The character sets of the selected character types.
    /// </summary>
    public IReadOnlyList<string> SelectedSets()
    {
        var sets = new List<string>();
        if (IncludeLowercase)
            sets.Add(CharacterSets.Lowercase);
        if (IncludeUppercase)
            sets.Add(CharacterSets.Uppercase);
        if (IncludeNumbers)
            sets.Add(CharacterSets.Numbers);
        if (IncludeSymbols)
            sets.Add(CharacterSets.Symbols);
        return sets;
    }

    /// <summary>
    /// The number of different characters the selected types offer.
    /// </summary>
    public int AvailableCharacters => SelectedSets().Sum(set => set.Length);
}

public class PasswordGenerationResult
{
    public bool Success { get; init; }
    public string Password { get; init; } = string.Empty;
    public string? ErrorMessage { get; init; }

    public static PasswordGenerationResult Ok(string password) => new() { Success = true, Password = password };
    public static PasswordGenerationResult Error(string message) => new() { Success = false, ErrorMessage = message };
}

public interface IPasswordGenerator
{
    PasswordGenerationResult Generate(PasswordOptions options);
}

public class PasswordGenerator : IPasswordGenerator
{
    public PasswordGenerationResult Generate(PasswordOptions options)
    {
        var charSets = options.SelectedSets();

        if (charSets.Count == 0)
            return PasswordGenerationResult.Error("Select at least one character set.");

        if (options.UniqueChars > options.Length)
            return PasswordGenerationResult.Error("Unique characters cannot exceed password length.");

        if (options.UniqueChars > options.AvailableCharacters)
            return PasswordGenerationResult.Error($"Not enough unique characters. Maximum: {options.AvailableCharacters}");

        var password = BuildPassword(charSets, options.Length, options.UniqueChars);
        return PasswordGenerationResult.Ok(password);
    }

    private static string BuildPassword(IReadOnlyList<string> charSets, int length, int uniqueCount)
    {
        var chars = new char[length];
        var pool = charSets.SelectMany(s => s.ToCharArray()).ToList();

        // The first uniqueCount characters are drawn without replacement, so they all differ.
        // The rest can repeat.
        for (var i = 0; i < uniqueCount; i++)
        {
            var idx = RandomNumberGenerator.GetInt32(pool.Count);
            chars[i] = pool[idx];
            pool.RemoveAt(idx);
        }

        for (var i = uniqueCount; i < length; i++)
        {
            var set = charSets[RandomNumberGenerator.GetInt32(charSets.Count)];
            chars[i] = set[RandomNumberGenerator.GetInt32(set.Length)];
        }

        // Otherwise the unique characters would always come first
        Shuffle(chars);

        return new string(chars);
    }

    private static void Shuffle(char[] array)
    {
        for (var i = array.Length - 1; i > 0; i--)
        {
            var j = RandomNumberGenerator.GetInt32(i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
}
