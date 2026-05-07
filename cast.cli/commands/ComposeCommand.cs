using System.Text.Json;
using System.Text.Json.Nodes;
using cast.cli.builder;
using cast.core.models;
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
        List<Node> nodes = new List<Node>();
        foreach (Stage stage in pipelineManifest.Stages)
        {
            string fragment = stage.Entry + ".fsh";
            FileInfo fragmentInfo = new FileInfo(fragment);
            string vertex   = stage.Entry + ".vsh";
            FileInfo vertexInfo = new FileInfo(vertex);

            string fragmentShader = File.ReadAllText($"{composePath}/shaders/{fragmentInfo.Name}");
            string vertexShader = File.ReadAllText($"{composePath}/shaders/{vertexInfo.Name}");

            try
            {
                
                GlslParser parser = new GlslParser();
                GlslShaderProgram vertexShaderProgram = parser.Parse(vertexShader);
                Node vertexNode = new Node(stage.Id, vertexInfo.Name, stage.DependsOn, stage.Type);
                vertexNode.Inputs = vertexShaderProgram.Inputs;
                vertexNode.Outputs = vertexShaderProgram.Outputs;
                parser = new GlslParser();
                GlslShaderProgram fragmentShaderProgram = parser.Parse(fragmentShader);
                Node fragmentNode = new Node(stage.Id, fragmentInfo.Name, stage.DependsOn, stage.Type);
                fragmentNode.Inputs = fragmentShaderProgram.Inputs;
                fragmentNode.Outputs = fragmentShaderProgram.Outputs;
            
                Console.WriteLine(vertexShaderProgram.GetShaderCode());
                Console.WriteLine(fragmentShaderProgram.GetShaderCode());

                string output = $"{composePath}/output/";
                if (!Directory.Exists(output)) Directory.CreateDirectory(output);
            
                File.WriteAllText(output + vertexInfo.Name, vertexShaderProgram.GetShaderCode());
                File.WriteAllText(output + fragmentInfo.Name, fragmentShaderProgram.GetShaderCode());
            
                nodes.Add(vertexNode);
                nodes.Add(fragmentNode);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        if (settings.VerifyGraph)
        {
            GraphBuilder.Wire(nodes);
            Console.WriteLine(GraphBuilder.AsMermaidGraph(nodes));
        }
        
        return 0;
    }
}