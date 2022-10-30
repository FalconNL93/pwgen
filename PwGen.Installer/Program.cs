using System.Diagnostics;

namespace PwGen.Installer;

internal static class Program
{
    private static readonly List<string> RestartProcesses = new()
    {
        "PowerToys", "PowerToys.PowerLauncher"
    };

    private static readonly List<string> PluginFiles = new()
    {
        "plugin.json",
        "Wox.Plugin.dll",
        "PwGen.Plugin.dll"
    };

    private static readonly List<string> PluginFolders = new()
    {
        "Assets"
    };

    private static readonly string CurrentDirectory = Directory.GetCurrentDirectory();
    private static readonly string PluginDir = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)}\PowerToys\modules\launcher\Plugins\PwGen";

    private static void Main(string[] args)
    {
        InstallPlugin();
    }

    private static void InstallPlugin()
    {
        foreach (var process in RestartProcesses)
        {
            Process.GetProcessesByName(process).ToList().ForEach(proc =>
            {
                Console.WriteLine($"Terminating process: {proc.ProcessName}...");
                proc.Kill();
            });
        }

        Thread.Sleep(2000);
        if (!Directory.Exists(PluginDir))
        {
            Console.WriteLine($"Creating {PluginDir}...");
            Directory.CreateDirectory(PluginDir);
        }

        foreach (var file in PluginFiles)
        {
            if (!File.Exists(file))
            {
                Console.WriteLine($"Missing file {file}");
                Environment.Exit(1);
            }

            Console.WriteLine(@$"Installing {file} -> {PluginDir}\{file}");
            File.Copy(@$"{file}", $@"{PluginDir}\{file}", true);
        }

        foreach (var folder in PluginFolders)
        {
            var pluginFolder = $@"{PluginDir}\{folder}";
            if (!Directory.Exists(pluginFolder))
            {
                Console.WriteLine($"Creating {pluginFolder}...");
                Directory.CreateDirectory(pluginFolder);
            }

            foreach (var file in Directory.GetFiles(@$"{CurrentDirectory}\{folder}", "*.*", SearchOption.AllDirectories))
            {
                var fileName = Path.GetFileName(file);
                Console.WriteLine(@$"Installing {folder}\{fileName} -> {PluginDir}\{folder}\{fileName}");
                File.Copy(@$"{file}", $@"{PluginDir}\{folder}\{fileName}", true);
            }
        }

        using var powerToys = new Process();
        powerToys.StartInfo.FileName = @"C:\Program Files\PowerToys\PowerToys.exe";
        powerToys.StartInfo.WorkingDirectory = @"C:\Program Files\PowerToys";
        powerToys.Start();
    }
}