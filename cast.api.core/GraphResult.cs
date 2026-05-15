using System.Collections.Generic;

namespace cast.api.core
{
    public class GraphPort
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class GraphNode
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Stage { get; set; }
        public List<GraphPort> Inputs { get; set; }
        public List<GraphPort> Outputs { get; set; }
    }

    public class GraphEdge
    {
        public string SourceNode { get; set; }
        public string SourcePort { get; set; }
        public string TargetNode { get; set; }
        public string TargetPort { get; set; }
        public string Status { get; set; } // "matched" or "typeMismatch"
    }

    public class GraphResult
    {
        public List<GraphNode> Nodes { get; set; }
        public List<GraphEdge> Edges { get; set; }
        public List<string> Errors { get; set; }
    }

    public class ShaderFile
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public List<ShaderFile> Children { get; set; }
    }

    public class GraphRequest
    {
        public Dictionary<string, List<ShaderFile>> Files { get; set; }
    }
}