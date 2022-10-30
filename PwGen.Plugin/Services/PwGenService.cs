using PwGen.Plugin.Models;

namespace PwGen.Plugin.Services;

public static class PwGenService
{
    private const int DefaultLength = 25;
    private static readonly Random Random = new();
    private static readonly string Alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private static readonly string DefaultCharacters;

    static PwGenService()
    {
        DefaultCharacters = $"{Alpha}{Alpha.ToLower()}";
    }

    public static string Generate(int length = DefaultLength)
    {
        var options = new GeneratorOptions
        {
            Length = length,
            Characters = DefaultCharacters
        };
        return new string(Enumerable.Repeat(options.Characters, options.Length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
}