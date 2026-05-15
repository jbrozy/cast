using System;
using System.Collections.Generic;
using System.Linq;
using cast.core.parser;
using cast.core.parser.programs;

namespace cast.api.core
{
    public class CompilationService
    {
        public CompilationResult Compile(string input)
        {
            if (string.IsNullOrEmpty(input)) return default(CompilationResult);
            GlslParser parser = new GlslParser();
            GlslShaderProgram result = parser.Parse(input);

            return new CompilationResult()
            {
                Errors = parser.GetLogs().ToList(),
                Result = result.GetShaderCode()
            };
        }

        public GraphResult Graph(GraphRequest request)
        {
            var errors = new List<string>();
            var nodes = new List<GraphNode>();
            int nodeId = 0;

            if (request?.Files == null || request.Files.Count == 0)
                return new GraphResult { Nodes = nodes, Edges = new List<GraphEdge>(), Errors = errors };

            foreach (var kvp in request.Files)
            {
                string stageName = kvp.Key;
                foreach (var file in kvp.Value)
                {
                    var parser = new GlslParser();
                    var program = parser.Parse(file.Content ?? "");
                    errors.AddRange(parser.GetLogs().Select(l => $"[{file.Name}] {l}"));

                    bool isFragment = file.Name.EndsWith(".fsh", StringComparison.OrdinalIgnoreCase);

                    var node = new GraphNode
                    {
                        Id = $"node_{nodeId++}",
                        Name = file.Name,
                        Stage = stageName,
                        Inputs = program.Inputs.Select(i => new GraphPort { Name = i.Name, Type = i.Type.ToString() }).ToList(),
                        Outputs = program.Outputs.Select(o => new GraphPort { Name = o.Name, Type = o.Type.ToString() }).ToList()
                    };
                    nodes.Add(node);

                    if (file.Children != null)
                    {
                        foreach (var child in file.Children)
                        {
                            var childParser = new GlslParser();
                            var childProgram = childParser.Parse(child.Content ?? "");
                            errors.AddRange(childParser.GetLogs().Select(l => $"[{child.Name}] {l}"));

                            var childNode = new GraphNode
                            {
                                Id = $"node_{nodeId++}",
                                Name = child.Name,
                                Stage = stageName,
                                Inputs = childProgram.Inputs.Select(i => new GraphPort { Name = i.Name, Type = i.Type.ToString() }).ToList(),
                                Outputs = childProgram.Outputs.Select(o => new GraphPort { Name = o.Name, Type = o.Type.ToString() }).ToList()
                            };
                            nodes.Add(childNode);
                        }
                    }
                }
            }

            var edges = new List<GraphEdge>();
            foreach (var target in nodes)
            {
                foreach (var input in target.Inputs)
                {
                    foreach (var source in nodes)
                    {
                        if (source == target) continue;

                        var exactMatch = source.Outputs.FirstOrDefault(o =>
                            o.Name == input.Name && o.Type == input.Type);
                        if (exactMatch != null)
                        {
                            edges.Add(new GraphEdge
                            {
                                SourceNode = source.Id,
                                SourcePort = exactMatch.Name,
                                TargetNode = target.Id,
                                TargetPort = input.Name,
                                Status = "matched"
                            });
                            break;
                        }

                        var nameMatch = source.Outputs.FirstOrDefault(o =>
                            o.Name == input.Name);
                        if (nameMatch != null)
                        {
                            edges.Add(new GraphEdge
                            {
                                SourceNode = source.Id,
                                SourcePort = nameMatch.Name,
                                TargetNode = target.Id,
                                TargetPort = input.Name,
                                Status = "typeMismatch"
                            });
                            break;
                        }
                    }
                }
            }

            return new GraphResult
            {
                Nodes = nodes,
                Edges = edges,
                Errors = errors
            };
        }
    }
}