using cast.core.parser;
using cast.core.parser.programs;
using Spectre.Console;
using Spectre.Console.Cli;

namespace cast.cli.commands;

public class WatchCommand : Command<WatchSettings>
{
    protected override int Execute(CommandContext context, WatchSettings settings, CancellationToken cancellationToken)
    {
        string inputDir = settings.InputDirectory;
        string outputDir = settings.OutputDirectory ?? Path.Combine(inputDir, "output");

        if (!Directory.Exists(inputDir))
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] Directory does not exist: {inputDir}");
            return 1;
        }

        AnsiConsole.MarkupLine($"[green]Watching:[/] {inputDir}");
        AnsiConsole.MarkupLine($"[green]Output:[/]   {outputDir}");

        Directory.CreateDirectory(outputDir);

        BuildAll(inputDir, outputDir);

        using var watcher = new FileSystemWatcher(inputDir);

        watcher.IncludeSubdirectories = true;
        watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime;
        watcher.Filters.Add("*.vsh");
        watcher.Filters.Add("*.fsh");

        var debounceTimers = new Dictionary<string, DateTime>();
        var debounceLock = new object();

        void OnChanged(object sender, FileSystemEventArgs e)
        {
            lock (debounceLock)
            {
                if (debounceTimers.TryGetValue(e.FullPath, out var last) &&
                    (DateTime.Now - last).TotalMilliseconds < 500)
                {
                    debounceTimers[e.FullPath] = DateTime.Now;
                    return;
                }
                debounceTimers[e.FullPath] = DateTime.Now;
            }

            CompileFile(e.FullPath, inputDir, outputDir);
        }

        watcher.Created += OnChanged;
        watcher.Changed += OnChanged;
        watcher.Renamed += (_, e) =>
        {
            if (!string.IsNullOrEmpty(e.OldName) && Path.GetExtension(e.OldName) is ".vsh" or ".fsh")
            {
                var oldOut = GetOutputPath(e.OldFullPath, inputDir, outputDir);
                if (File.Exists(oldOut))
                {
                    File.Delete(oldOut);
                    AnsiConsole.MarkupLine($"[yellow]Removed:[/] {oldOut}");
                }
            }
            CompileFile(e.FullPath, inputDir, outputDir);
        };
        watcher.Deleted += (_, e) =>
        {
            var outPath = GetOutputPath(e.FullPath, inputDir, outputDir);
            if (File.Exists(outPath))
            {
                File.Delete(outPath);
                AnsiConsole.MarkupLine($"[yellow]Removed:[/] {outPath}");
            }
        };
        watcher.Error += (_, e) =>
        {
            AnsiConsole.MarkupLine($"[red]Watcher error:[/] {e.GetException().Message}");
        };

        watcher.EnableRaisingEvents = true;

        AnsiConsole.MarkupLine("[green]Press [bold]Ctrl+C[/] or [bold]q[/] then [bold]Enter[/] to stop watching.[/]");

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        try
        {
            while (!cts.Token.IsCancellationRequested)
            {
                try
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(intercept: true);
                        if (key.KeyChar is 'q' or 'Q')
                            break;
                    }
                }
                catch (InvalidOperationException)
                {
                    Thread.Sleep(1000);
                }
                Thread.Sleep(200);
            }
        }
        catch (OperationCanceledException)
        {
        }

        AnsiConsole.MarkupLine("[green]Watch stopped.[/]");
        return 0;
    }

    private static void BuildAll(string inputDir, string outputDir)
    {
        var files = Directory.GetFiles(inputDir, "*", SearchOption.AllDirectories);
        var shaderFiles = files.Where(f => Path.GetExtension(f) is ".vsh" or ".fsh");

        foreach (var file in shaderFiles)
        {
            CompileFile(file, inputDir, outputDir);
        }

        var outputs = Directory.GetFiles(outputDir, "*", SearchOption.AllDirectories);
        foreach (var outFile in outputs)
        {
            var relPath = Path.GetRelativePath(outputDir, outFile);
            var sourceFile = Path.Combine(inputDir, relPath);
            if (!File.Exists(sourceFile))
            {
                File.Delete(outFile);
                AnsiConsole.MarkupLine($"[yellow]Removed stale:[/] {outFile}");
            }
        }
    }

    private static string ReadFileWithRetry(string filePath, int maxRetries = 5)
    {
        for (var i = 0; i < maxRetries; i++)
        {
            try
            {
                return File.ReadAllText(filePath);
            }
            catch (IOException) when (i < maxRetries - 1)
            {
                Thread.Sleep(200 * (i + 1));
            }
        }

        return File.ReadAllText(filePath);
    }

    private static void CompileFile(string filePath, string inputDir, string outputDir)
    {
        if (Path.GetExtension(filePath) is not ".vsh" and not ".fsh") return;

        try
        {
            string fileContent = ReadFileWithRetry(filePath);

            var parser = new GlslParser();
            GlslShaderProgram result = parser.Parse(fileContent);

            if (parser.GetLogs().Count > 0)
            {
                foreach (var log in parser.GetLogs())
                    AnsiConsole.MarkupLine($"[yellow]  {Markup.Escape(log)}[/]");
            }

            string newContent = result.GetShaderCode();

            string outPath = GetOutputPath(filePath, inputDir, outputDir);
            Directory.CreateDirectory(Path.GetDirectoryName(outPath)!);
            File.WriteAllText(outPath, newContent);

            AnsiConsole.MarkupLine($"[green]Compiled:[/] {filePath} -> {outPath}");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] {filePath}: {ex.Message}");
        }
    }

    private static string GetOutputPath(string filePath, string inputDir, string outputDir)
    {
        string relative = Path.GetRelativePath(inputDir, filePath);
        return Path.Combine(outputDir, relative);
    }
}
