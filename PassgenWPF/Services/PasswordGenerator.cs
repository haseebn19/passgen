using System.Security.Cryptography;

namespace PassgenWPF.Services;

public class PasswordOptions
{
    public bool IncludeLowercase { get; init; } = true;
    public bool IncludeUppercase { get; init; } = true;
    public bool IncludeNumbers { get; init; } = true;
    public bool IncludeSymbols { get; init; } = true;
    public int Length { get; init; } = 16;
    public int UniqueChars { get; init; } = 12;
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
    private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
    private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string NumberChars = "0123456789";
    private const string SymbolChars = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";

    public PasswordGenerationResult Generate(PasswordOptions options)
    {
        var charSets = GetCharacterSets(options);

        if (charSets.Count == 0)
            return PasswordGenerationResult.Error("Select at least one character set.");

        if (options.UniqueChars > options.Length)
            return PasswordGenerationResult.Error("Unique characters cannot exceed password length.");

        var totalAvailable = charSets.Sum(s => s.Length);
        if (options.UniqueChars > totalAvailable)
            return PasswordGenerationResult.Error($"Not enough unique characters. Maximum: {totalAvailable}");

        var password = BuildPassword(charSets, options.Length, options.UniqueChars);
        return PasswordGenerationResult.Ok(password);
    }

    private static List<string> GetCharacterSets(PasswordOptions options)
    {
        var sets = new List<string>();
        if (options.IncludeLowercase) sets.Add(LowercaseChars);
        if (options.IncludeUppercase) sets.Add(UppercaseChars);
        if (options.IncludeNumbers) sets.Add(NumberChars);
        if (options.IncludeSymbols) sets.Add(SymbolChars);
        return sets;
    }

    private static string BuildPassword(List<string> charSets, int length, int uniqueCount)
    {
        var chars = new char[length];
        var pool = charSets.SelectMany(s => s.ToCharArray()).ToList();

        // Pick unique characters first
        for (var i = 0; i < uniqueCount; i++)
        {
            var idx = RandomNumberGenerator.GetInt32(pool.Count);
            chars[i] = pool[idx];
            pool.RemoveAt(idx);
        }

        // Fill remaining positions
        for (var i = uniqueCount; i < length; i++)
        {
            var set = charSets[RandomNumberGenerator.GetInt32(charSets.Count)];
            chars[i] = set[RandomNumberGenerator.GetInt32(set.Length)];
        }

        // Shuffle to avoid predictable patterns
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
