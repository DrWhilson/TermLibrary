using static libraryNodes.NodeTypeInfo;

namespace libraryNodes
{
    public class HexNode : GraphNode<int>
    {
        public NodeType NodeType { get; set; }
        public int MaxNeighbors => MaxNeighbors(NodeType);
        public HexCoord Coord { get; set; }

        public HexNode(int id, NodeType type, HexCoord coord)
            : base(id, type.ToString())
        {
            NodeType = type;
            Coord = coord;
        }

        public bool CanAddNeighbor() => Neighbors.Count < MaxNeighbors;
    }
}
