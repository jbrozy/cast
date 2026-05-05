using Spectre.Console;
using Spectre.Console.Cli;

namespace cast.cli.commands;

public class BuildCommand : Command<BuildSettings>
{
    protected override int Execute(CommandContext context, BuildSettings settings, CancellationToken cancellationToken)
    {
        bool directory = false;
        if (Directory.Exists(settings.InputFile))
        {
            AnsiConsole.MarkupLine($"[Green]Info:[/] Input {settings.InputFile} is a directory.");
            directory = true;
        }
        else if (!File.Exists(settings.InputFile))
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] File does not exist: {settings.InputFile}");
        }

        if (directory)
        {
            string input = File.ReadAllText(settings.InputFile);
        }
        
        try
        {
            string output = settings.OutputFile;
            return 0;
        }
        catch (Exception e)
        {
            return 1;
        }
    }
}