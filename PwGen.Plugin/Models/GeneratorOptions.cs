namespace PwGen.Plugin.Models;

public class GeneratorOptions
{
    public int Length { get; set; }
    public string Characters { get; }

    public bool UseLowerCaseLetters { get; set; }
    public bool UseUpperCaseLetters { get; set; }
    public bool UseNumbers { get; set; }
    public bool UseSpecialCharacters { get; set; }
}