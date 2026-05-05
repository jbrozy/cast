using cast.core.models.symbols;

namespace cast.core.models
{
    public class Connection
    {
        public Node SourceNode { get; set; }
        public VariableSymbol SourcePort { get; set; }

        public Node TargetNode { get; set; }
        public VariableSymbol TargetPort { get; set; }

        public Connection(Node from, Node to, VariableSymbol fromPort, VariableSymbol toPort)
        {
            SourceNode = from;
            TargetNode = to;
            
            SourcePort = fromPort;
            TargetPort = toPort;
        }
    }
}