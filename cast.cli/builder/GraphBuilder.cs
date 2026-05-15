using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using cast.core.models;
using cast.core.models.symbols;

namespace cast.cli.builder;

public static class GraphBuilder
{
    public static string AsMermaidGraph(List<Node> nodes)
    {
        StringBuilder graph = new StringBuilder();
    
        graph.AppendLine("graph LR;"); 
        graph.AppendLine();

        foreach (var node in nodes)
        {
            graph.AppendLine($"    subgraph {node.Id} [\"{node.Id} ({node.Stage})\"]");
            graph.AppendLine($"        direction TB");
        
            foreach(var input in node.Inputs)
            {
                string portId = $"{node.Id}_in_{input.Name}";
                graph.AppendLine($"        {portId}([in {input.Type} {input.Name}]);");
            }

            foreach(var output in node.Outputs)
            {
                string portId = $"{node.Id}_out_{output.Name}";
                graph.AppendLine($"        {portId}([out {output.Type} {output.Name}]);");
            }
        
            graph.AppendLine("    end");
            graph.AppendLine();
        }

        foreach (var node in nodes)
        {
            foreach (var conn in node.Outgoing) 
            {
                string sourcePortId = $"{conn.SourceNode.Id}_out_{conn.SourcePort.Name}";
                string targetPortId = $"{conn.TargetNode.Id}_in_{conn.TargetPort.Name}";
            
                graph.AppendLine($"    {sourcePortId} ==> {targetPortId};");
            }
        }
        
    
        return graph.ToString();
    }

    public static string AsJson(List<Node> nodes)
    {
        var result = new
        {
            nodes = nodes.Select(n => new
            {
                id = n.Id,
                name = n.Name,
                stage = n.Stage.ToString(),
                inputs = n.Inputs.Select(i => new
                {
                    name = i.Name,
                    type = i.Type.ToString()
                }),
                outputs = n.Outputs.Select(o => new
                {
                    name = o.Name,
                    type = o.Type.ToString()
                })
            }),
            edges = nodes.SelectMany(n => n.Outgoing).Select(c => new
            {
                sourceNode = c.SourceNode.Id,
                sourcePort = c.SourcePort.Name,
                targetNode = c.TargetNode.Id,
                targetPort = c.TargetPort.Name
            })
        };

        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }
    
    public static List<string> Wire(List<Node> nodes)
    {
        List<string> errors = new List<string>();

        for (int i = 0; i < nodes.Count; i++)
        {
            Node target = nodes[i];
            
            foreach (VariableSymbol input in target.Inputs)
            {
                string? matchSource = FindMatchingOutput(nodes, i, input);
                if (matchSource == null)
                {
                    errors.Add($"Stage '{target.Id}': input '{input.Type}' '{input.Name}' has no matching output in any preceding stage");
                }
            }
        }

        return errors;
    }

    private static string? FindMatchingOutput(List<Node> nodes, int targetIndex, VariableSymbol input)
    {
        for (int j = 0; j < nodes.Count; j++)
        {
            if (j == targetIndex) continue;
            Node source = nodes[j];
            VariableSymbol? match = source.Outputs.FirstOrDefault(o =>
                o.Name == input.Name && Equals(o.Type, input.Type));
            if (match != null)
            {
                Connection connection = new Connection(source, nodes[targetIndex], match, input);
                source.Outgoing.Add(connection);
                nodes[targetIndex].Incoming.Add(connection);
                return source.Id;
            }
        }

        return null;
    }
}