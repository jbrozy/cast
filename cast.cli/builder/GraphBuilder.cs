using System.Text;
using cast.core.models;
using cast.core.models.symbols;

namespace cast.cli.builder;

public abstract class GraphBuilder
{
    private void Sort(List<Node> nodes)
    {
        
    }
    
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
    
    public static void Wire(List<Node> nodes)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            Node target = nodes[i];
            
            foreach (VariableSymbol input in target.Inputs)
            {
                bool connectionFound = false;
                for (int j = i - 1; j >= 0; j--)
                {
                    Node source =  nodes[j];
                    VariableSymbol? match = source.Outputs.FirstOrDefault(o => Equals(o.Type, input.Type) && o.Name == input.Name);
                    if (match != null)
                    {
                        Connection connection = new Connection(source, target, match, input);
                        source.Outgoing.Add(connection);
                        target.Incoming.Add(connection);
                        connectionFound = true;
                        break;
                    }
                }

                if (!connectionFound)
                {
                    throw new Exception("No connection found");
                }
            }
        }
    }
}