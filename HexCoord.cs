namespace libraryNodes
{
    public readonly struct HexCoord : IEquatable<HexCoord>
    {
        public readonly int Q;
        public readonly int R;

        public HexCoord(int q, int r)
        {
            Q = q;
            R = r;
        }

        private static readonly HexCoord[] DirOffsets =
        [
            new(1, 0),
            new(1, -1),
            new(0, -1),
            new(-1, 0),
            new(-1, 1),
            new(0, 1),
        ];

        public HexCoord Neighbor(int index) =>
            new(Q + DirOffsets[index].Q, R + DirOffsets[index].R);

        public bool Equals(HexCoord other) => Q == other.Q && R == other.R;

        public override bool Equals(object? obj) => obj is HexCoord other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Q, R);

        public override string ToString() => $"({Q},{R})";

        public static bool operator ==(HexCoord a, HexCoord b) => a.Equals(b);

        public static bool operator !=(HexCoord a, HexCoord b) => !a.Equals(b);
    }
}
