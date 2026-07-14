namespace libraryNodes
{
    public class HexNode
    {
        public int Id { get; set; }
        public NodeType NodeType { get; set; }
        public HexCoord Coord { get; set; }
        public List<HexNode> Neighbors { get; set; }

        public int MaxNeighbors => NodeType.MaxNeighbors();

        public HexNode(int id, NodeType type, HexCoord coord)
        {
            Id = id;
            NodeType = type;
            Coord = coord;
            Neighbors = new List<HexNode>();
        }

        public bool CanAddNeighbor() => Neighbors.Count < MaxNeighbors;
    }
}
