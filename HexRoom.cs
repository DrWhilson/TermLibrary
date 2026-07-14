using static libraryNodes.NodeTypeInfo;

namespace libraryNodes
{
    public class HexNode : GraphNode<int>
    {
        public NodeType NodeType { get; set; }
        public int MaxNeighbors => MaxNeighbors(NodeType);
        public string? BookText { get; set; }
        public int BookPageNumber { get; set; }

        public HexNode(int id, NodeType type) : base(id, type.ToString())
        {
            NodeType = type;

            if (type == NodeType.HexRoom)
            {
                var rng = Random.Shared;
                const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var buf = new char[1100];
                for (int i = 0; i < 1100; i++)
                    buf[i] = chars[rng.Next(chars.Length)];
                BookText = new string(buf);
                BookPageNumber = rng.Next(1, 999);
            }
        }

        public bool CanAddNeighbor() => Neighbors.Count < MaxNeighbors;
    }
}
