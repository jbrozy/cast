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

        private static GraphNode BuildNode(string id, string name, string stage, GlslShaderProgram program)
        {
            return new GraphNode
            {
                Id = id,
                Name = name,
                Stage = stage,
                Inputs = program.Inputs.Select(i => new GraphPort { Name = i.Name, Type = i.Type.ToString() }).ToList(),
                Outputs = program.Outputs.Select(o => new GraphPort { Name = o.Name, Type = o.Type.ToString() }).ToList(),
                Textures = program.Textures.Select(t => new GraphPort { Name = t.Name, Type = t.Type.ToString() }).ToList()
            };
        }

        public GraphResult Graph(GraphRequest request)
        {
            var errors = new List<string>();
            var nodes = new List<GraphNode>();
            int nodeId = 0;

            if (request?.Files == null || request.Files.Count == 0)
                return new GraphResult { Nodes = nodes, Edges = new List<GraphEdge>(), Errors = errors };

            var stageOrder = request.StageOrder ?? new Dictionary<string, int>();
            var sortedStages = request.Files.Keys
                .OrderBy(s => stageOrder.GetValueOrDefault(s, 0))
                .ThenBy(s => s)
                .ToList();

            foreach (var stageName in sortedStages)
            {
                var files = request.Files[stageName];
                foreach (var file in files)
                {
                    var parser = new GlslParser();
                    var program = parser.Parse(file.Content ?? "");
                    errors.AddRange(parser.GetLogs().Select(l => $"[{file.Name}] {l}"));

                    var node = BuildNode($"node_{nodeId++}", file.Name, stageName, program);
                    nodes.Add(node);

                    if (file.Children != null)
                    {
                        foreach (var child in file.Children)
                        {
                            var childParser = new GlslParser();
                            var childProgram = childParser.Parse(child.Content ?? "");
                            errors.AddRange(childParser.GetLogs().Select(l => $"[{child.Name}] {l}"));

                            var childNode = BuildNode($"node_{nodeId++}", child.Name, stageName, childProgram);
                            nodes.Add(childNode);
                        }
                    }
                }
            }

            var edges = new List<GraphEdge>();

            // match vertex shader outputs -> fragment shader inputs (intra-stage)
            var vshNodes = nodes.Where(n => n.Name.EndsWith(".vsh", StringComparison.OrdinalIgnoreCase)).ToList();
            var fshNodes = nodes.Where(n => n.Name.EndsWith(".fsh", StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var target in fshNodes)
            {
                foreach (var input in target.Inputs)
                {
                    var source = vshNodes.FirstOrDefault(s =>
                        s.Stage == target.Stage &&
                        s.Outputs.Any(o => o.Name == input.Name && o.Type == input.Type));
                    if (source != null)
                    {
                        edges.Add(new GraphEdge
                        {
                            SourceNode = source.Id,
                            SourcePort = input.Name,
                            TargetNode = target.Id,
                            TargetPort = input.Name,
                            Status = "matched"
                        });
                        continue;
                    }

                    source = vshNodes.FirstOrDefault(s =>
                        s.Stage == target.Stage &&
                        s.Outputs.Any(o => o.Name == input.Name));
                    if (source != null)
                    {
                        edges.Add(new GraphEdge
                        {
                            SourceNode = source.Id,
                            SourcePort = input.Name,
                            TargetNode = target.Id,
                            TargetPort = input.Name,
                            Status = "typeMismatch"
                        });
                    }
                }
            }

            // match fragment shader outputs -> sampler uniforms in fragment shaders
            foreach (var target in fshNodes)
            {
                if (target.Textures == null || target.Textures.Count == 0) continue;

                foreach (var tex in target.Textures)
                {
                    foreach (var source in fshNodes)
                    {
                        if (source == target) continue;

                        var match = source.Outputs.FirstOrDefault(o => o.Name == tex.Name);
                        if (match == null) continue;

                        tex.Type = $"sampler2D<{match.Type}>";

                        var isSameStage = source.Stage == target.Stage;
                        edges.Add(new GraphEdge
                        {
                            SourceNode = source.Id,
                            SourcePort = match.Name,
                            TargetNode = target.Id,
                            TargetPort = tex.Name,
                            Status = isSameStage ? "matched" : "texture"
                        });
                        break;
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