using System.Text.Json;
using cast.cli.builder;
using cast.core.models;
using cast.core.parser;
using cast.core.parser.programs;
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

        DirectoryInfo? composeDir = new FileInfo(composeFile).Directory;
        if (composeDir == null)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] Could not determine directory for compose file: {composeFile}");
            return 1;
        }
        List<Node> nodes = new List<Node>();
        foreach (Stage stage in pipelineManifest.Stages)
        {
            string vertex   = stage.Entry + ".vsh";
            string fragment = stage.Entry + ".fsh";

            string vertexPath   = Path.Combine(composeDir.FullName, "shaders", vertex);
            string fragmentPath = Path.Combine(composeDir.FullName, "shaders", fragment);

            if (!File.Exists(vertexPath))
            {
                AnsiConsole.MarkupLine($"[red]Error:[/] Vertex shader not found: {vertexPath}");
                continue;
            }
            if (!File.Exists(fragmentPath))
            {
                AnsiConsole.MarkupLine($"[red]Error:[/] Fragment shader not found: {fragmentPath}");
                continue;
            }

            string vertexShader   = File.ReadAllText(vertexPath);
            string fragmentShader = File.ReadAllText(fragmentPath);

            GlslParser parser = new GlslParser();
            GlslShaderProgram vertexProgram   = parser.Parse(vertexShader);
            if (parser.GetLogs().Any())
            {
                AnsiConsole.MarkupLine($"[yellow]Warnings/Errors for {vertex}:[/]");
                foreach (var log in parser.GetLogs())
                    AnsiConsole.MarkupLine($"  {log}");
            }

            Node vertexNode = new Node(stage.Id, vertex, stage.DependsOn, stage.Type);
            vertexNode.Inputs = vertexProgram.Inputs;
            vertexNode.Outputs = vertexProgram.Outputs;

            parser = new GlslParser();
            GlslShaderProgram fragmentProgram = parser.Parse(fragmentShader);
            if (parser.GetLogs().Any())
            {
                AnsiConsole.MarkupLine($"[yellow]Warnings/Errors for {fragment}:[/]");
                foreach (var log in parser.GetLogs())
                    AnsiConsole.MarkupLine($"  {log}");
            }

            Node fragmentNode = new Node(stage.Id, fragment, stage.DependsOn, stage.Type);
            fragmentNode.Inputs = fragmentProgram.Inputs;
            fragmentNode.Outputs = fragmentProgram.Outputs;

            string output = Path.Combine(composeDir.FullName, "output");
            if (!Directory.Exists(output)) Directory.CreateDirectory(output);

            File.WriteAllText(Path.Combine(output, vertex), vertexProgram.GetShaderCode());
            File.WriteAllText(Path.Combine(output, fragment), fragmentProgram.GetShaderCode());

            nodes.Add(vertexNode);
            nodes.Add(fragmentNode);
        }

        if (settings.VerifyGraph)
        {
            var errors = GraphBuilder.Wire(nodes);
            foreach (var err in errors)
                AnsiConsole.MarkupLine($"[red]Error:[/] {err}");

            if (errors.Count == 0)
            {
                AnsiConsole.MarkupLine("[green]Graph validation passed.[/]");
                string graphJson = GraphBuilder.AsJson(nodes);
                string graphJsonPath = Path.Combine(composeDir.FullName, "output", "graph.json");
                File.WriteAllText(graphJsonPath, graphJson);

                if (settings.OutputFormat == "mermaid" || settings.OutputFormat == null)
                    Console.WriteLine(GraphBuilder.AsMermaidGraph(nodes));

                if (settings.OutputFormat == "json")
                    Console.WriteLine(graphJson);
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]{errors.Count} connection error(s) found.[/]");
                return 1;
            }
        }
        
        return 0;
    }
}