using PwGen.Plugin.Helpers;
using PwGen.Plugin.Services;
using Wox.Plugin;

namespace PwGen.Plugin;

public class Main : IPlugin
{
    private List<Result> _results = new();
    private PluginInitContext? Context { get; set; }
    public string Name => "Password Generator";
    public string Description => "Generate passwords";

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
        var password = PwGenService.Generate(int.Parse(length));
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
        var password = PwGenService.Generate();
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
}