using System.Collections.Generic;
using cast.core.models.symbols;

namespace cast.core.models
{

    public class Node
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public StageType Stage { get; set; }

        public List<VariableSymbol> Inputs { get; set; } = new List<VariableSymbol>();
        public List<VariableSymbol> Outputs { get; set; } = new List<VariableSymbol>();
        public List<VariableSymbol> Textures { get; set; } = new List<VariableSymbol>();

        public List<Connection> Incoming { get; set; } = new List<Connection>();
        public List<Connection> Outgoing { get; set; } = new List<Connection>();

        public Node(string id, string name, string[] dependsOn, StageType stage)
        {
            Id = id;
            Name = name;
            Stage = stage;
        }
    }
}
