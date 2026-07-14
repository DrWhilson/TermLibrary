using static libraryNodes.NodeTypeInfo;

namespace libraryNodes
{
    public class HexNode : GraphNode<int>
    {
        public NodeType NodeType { get; set; }
        public int MaxNeighbors => MaxNeighbors(NodeType);

        public HexNode(int id, NodeType type)
            : base(id, type.ToString())
        {
            NodeType = type;
        }

        public bool CanAddNeighbor() => Neighbors.Count < MaxNeighbors;
    }
}
