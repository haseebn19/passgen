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

    private readonly Random _random;

    public PasswordGenerator() : this(new Random()) { }

    public PasswordGenerator(Random random) => _random = random;

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

    private string BuildPassword(List<string> charSets, int length, int uniqueCount)
    {
        var chars = new char[length];
        var pool = charSets.SelectMany(s => s.ToCharArray()).ToList();

        // Unique characters first
        for (var i = 0; i < uniqueCount; i++)
        {
            var idx = _random.Next(pool.Count);
            chars[i] = pool[idx];
            pool.RemoveAt(idx);
        }

        // Fill remaining
        for (var i = uniqueCount; i < length; i++)
        {
            var set = charSets[_random.Next(charSets.Count)];
            chars[i] = set[_random.Next(set.Length)];
        }

        return new string(chars);
    }
}
