namespace libraryNodes
{
    public enum NodeType
    {
        HexRoom,
        Passage,
        DeadEnd,
    }

    public static class NodeTypeExtensions
    {
        public static int MaxNeighbors(this NodeType type) =>
            type switch
            {
                NodeType.HexRoom => 6,
                NodeType.Passage => 2,
                NodeType.DeadEnd => 1,
                _ => throw new ArgumentOutOfRangeException(nameof(type)),
            };
    }
}
