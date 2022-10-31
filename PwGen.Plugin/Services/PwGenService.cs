using PwGen.Plugin.Models;

namespace PwGen.Plugin.Services;

public static class PwGenService
{
    private const int DefaultLength = 25;
    private static readonly Random Random = new();
    private static readonly string Alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private static readonly string Numbers = "1234567890";
    private static readonly string SpecialCharacters = "!@#$%&*";

    static PwGenService()
    {
    }

    public static string Generate(GeneratorOptions options)
    {
        var passwordBuilder = string.Empty;

        if (options.UseLowerCaseLetters)
        {
            passwordBuilder += Alpha.ToLower();
        }

        if (options.UseUpperCaseLetters)
        {
            passwordBuilder += Alpha;
        }

        if (options.UseNumbers)
        {
            passwordBuilder += Numbers;
        }

        if (options.UseSpecialCharacters)
        {
            passwordBuilder += SpecialCharacters;
        }

        return new string(Enumerable.Repeat(passwordBuilder, options.Length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
}