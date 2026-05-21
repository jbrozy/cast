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
        settings.InputFile =
            "C:\\Users\\jannik\\AppData\\Roaming\\ModrinthApp\\profiles\\Fabulously Optimized (1)\\shaderpacks\\ssao\\cast\\";
        settings.OutputFile =
            "C:\\Users\\jannik\\AppData\\Roaming\\ModrinthApp\\profiles\\Fabulously Optimized (1)\\shaderpacks\\ssao\\shaders\\";
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
                    Console.WriteLine(e.StackTrace);
                }
            }
        }
        
        try
        {
            string output = settings.OutputFile;
            return 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return 1;
        }
    }
}