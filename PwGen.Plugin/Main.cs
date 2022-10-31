using System.Windows.Controls;
using Microsoft.PowerToys.Settings.UI.Library;
using PwGen.Plugin.Helpers;
using PwGen.Plugin.Models;
using PwGen.Plugin.Services;
using Wox.Plugin;

namespace PwGen.Plugin;

public class Main : IPlugin, ISettingProvider
{
    private List<Result> _results = new();
    private IEnumerable<PluginAdditionalOption> _additionalOptions;
    private PluginInitContext? Context { get; set; }
    public string Name => "Password Generator";
    public string Description => "Generate passwords";

    private bool _optionLowerCaseLetters = true;
    private bool _optionUpperCaseLetters = true;
    private bool _optionNumbers = true;
    private bool _optionSpecialCharacters = true;

    public List<Result> Query(Query query)
    {
        if (string.IsNullOrEmpty(query.Search))
        {
            QueryWithoutParameters?.Invoke(this, EventArgs.Empty);
        }
        else if (!string.IsNullOrEmpty(query.Search))
        {
            QueryWithParameters?.Invoke(this, query);
        }

        return _results;
    }

    public void Init(PluginInitContext? context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));

        QueryWithoutParameters += OnQueryWithoutParameters;
        QueryWithParameters += OnQueryWithParameters;
    }

    public event EventHandler<Query>? QueryWithParameters;
    public event EventHandler? QueryWithoutParameters;

    private void OnQueryWithParameters(object? sender, Query query)
    {
        var parameters = query.Search.Split(" ");
        var length = parameters.FirstOrDefault();
        var password = PwGenService.Generate(new GeneratorOptions
        {
            Length = int.Parse(length),
            UseLowerCaseLetters = _optionLowerCaseLetters,
            UseUpperCaseLetters = _optionUpperCaseLetters,
            UseNumbers = _optionNumbers,
            UseSpecialCharacters = _optionSpecialCharacters
        });

        _results = new List<Result>
        {
            new()
            {
                Title = password,
                SubTitle = "Copy to clipboard",
                Action = _ =>
                {
                    ClipboardHelper.SetText(password);
                    return true;
                }
            }
        };
    }

    private void OnQueryWithoutParameters(object? sender, EventArgs e)
    {
        var password = PwGenService.Generate(new GeneratorOptions
        {
            Length = 25,
            UseLowerCaseLetters = _optionLowerCaseLetters,
            UseUpperCaseLetters = _optionUpperCaseLetters,
            UseNumbers = _optionNumbers,
            UseSpecialCharacters = _optionSpecialCharacters
        });

        _results = new List<Result>
        {
            new()
            {
                Title = password,
                SubTitle = "Copy to clipboard",
                Action = _ =>
                {
                    ClipboardHelper.SetText(password);
                    return true;
                }
            }
        };
    }

    public Control CreateSettingPanel()
    {
        throw new NotImplementedException();
    }

    public void UpdateSettings(PowerLauncherPluginSettings settings)
    {
        if (settings is not {AdditionalOptions: { }})
        {
            return;
        }

        _optionLowerCaseLetters = settings.AdditionalOptions.FirstOrDefault(x => x.Key == "LowerCaseLetters")?.Value ?? true;
        _optionUpperCaseLetters = settings.AdditionalOptions.FirstOrDefault(x => x.Key == "UpperCaseLetters")?.Value ?? true;
        _optionNumbers = settings.AdditionalOptions.FirstOrDefault(x => x.Key == "Numbers")?.Value ?? true;
        _optionSpecialCharacters = settings.AdditionalOptions.FirstOrDefault(x => x.Key == "SpecialCharacters")?.Value ?? true;
    }

    public IEnumerable<PluginAdditionalOption> AdditionalOptions =>
        new List<PluginAdditionalOption>()
        {
            new()
            {
                Key = "LowerCaseLetters",
                DisplayLabel = "Lowercase Letters",
                DisplayDescription = "Add letters to generated password (a-z)",
                Value = true,
            },
            new()
            {
                Key = "UpperCaseLetters",
                DisplayLabel = "Uppercase Letters",
                DisplayDescription = "Add letters to generated password (A-Z)",
                Value = true,
            },
            new()
            {
                Key = "Numbers",
                DisplayLabel = "Numbers",
                DisplayDescription = "Add numbers to generated password (0-9)",
                Value = true,
            },
            new()
            {
                Key = "SpecialCharacters",
                DisplayLabel = "Special characters",
                DisplayDescription = "Add special characters to generated password (!@#$%^&*)",
                Value = true,
            }
        };
}