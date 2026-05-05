using System.Text.Json;
using System.Text.Json.Nodes;
using cast.cli.models;
using cast.core.parser;
using cast.core.parser.programs;
using cast.core.visitor.configuration;
using Spectre.Console;
using Spectre.Console.Cli;

namespace cast.cli.commands;

public class ComposeCommand : Command<ComposeSettings>
{
    protected override int Execute(CommandContext context, ComposeSettings settings, CancellationToken cancellationToken)
    {
        string composeFile = settings.ComposeFile;
        if (!File.Exists(composeFile))
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] File does not exist: {composeFile}");
            return 1;
        }
        
        PipelineManifest pipelineManifest = JsonSerializer.Deserialize<PipelineManifest>(File.ReadAllText(composeFile));

        var args= context.Arguments;
        DirectoryInfo? composePath = new FileInfo(args[1]).Directory;
        foreach (Stage stage in pipelineManifest.Stages)
        {
            string fragment = stage.Entry + ".fsh";
            FileInfo fragmentInfo = new FileInfo(fragment);
            string vertex   = stage.Entry + ".vsh";
            FileInfo vertexInfo = new FileInfo(vertex);

            string fragmentShader = File.ReadAllText($"{composePath}/shaders/{fragmentInfo.Name}");
            string vertexShader = File.ReadAllText($"{composePath}/shaders/{vertexInfo.Name}");
            
            GLSLParser parser = new GLSLParser();
            GLSLShaderProgram vertexShaderProgram = parser.Parse(vertexShader);
            parser = new GLSLParser();
            GLSLShaderProgram fragmentShaderProgram = parser.Parse(fragmentShader);
            
            Console.WriteLine(vertexShaderProgram.GetShaderCode());
            Console.WriteLine(fragmentShaderProgram.GetShaderCode());

            string output = $"{composePath}/output/";
            if (!Directory.Exists(output)) Directory.CreateDirectory(output);
            
            File.WriteAllText(output + vertexInfo.Name, vertexShaderProgram.GetShaderCode());
            File.WriteAllText(output + fragmentInfo.Name, fragmentShaderProgram.GetShaderCode());
        }
        
        return 0;
    }
}