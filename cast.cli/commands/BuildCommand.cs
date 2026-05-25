using cast.core.parser;
using cast.core.parser.programs;
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
            List<string> files = Directory.GetFiles(settings.InputFile, "*", SearchOption.AllDirectories).ToList();

            foreach (var file in files)
            {
                AnsiConsole.MarkupLine($"[Green][/] Compiling file: {file}");
                try
                {

                    GlslParser parser = new GlslParser();
                    string fileContent = File.ReadAllText(file);
                    GlslShaderProgram result = parser.Parse(fileContent);

                    if (parser.GetLogs().Count > 0)
                    {
                        foreach (var log in parser.GetLogs())
                            AnsiConsole.MarkupLine($"[yellow]  {Markup.Escape(log)}[/]");
                    }

                    string newFileContent = result.GetShaderCode();

                    FileInfo fileInfo = new FileInfo(file);
                    string fileName = fileInfo.Name;
                    string outFileName = Path.Join(settings.OutputFile, fileName);
                    FileInfo newFileInfo = new FileInfo(outFileName);
                    newFileInfo.Directory?.Create();
                    File.WriteAllText(outFileName, newFileContent);

                    AnsiConsole.MarkupLine($"[Green][/] Compiled file: {newFileInfo.FullName}");
                }
                catch (Exception e)
                {
                    AnsiConsole.MarkupLine($"[red]Error:[/] {file}: {Markup.Escape(e.StackTrace)}");
                    return 1;
                }
            }
        }

        return 0;
    }
}
