namespace libraryNodes
{
    public class GraphGenerator
    {
        private readonly Random _rng;
        private readonly int _maxNodes;
        private int _nextId;
        private readonly List<HexNode> _allNodes;
        private readonly Dictionary<HexCoord, HexNode> _coordToNode;

        private static readonly (NodeType type, double weight)[] TypeWeights =
        [
            (NodeType.HexRoom, 0.30),
            (NodeType.Passage, 0.50),
            (NodeType.DeadEnd, 0.20),
        ];

        public IReadOnlyList<HexNode> AllNodes => _allNodes;

        public GraphGenerator(int? seed = null, int maxNodes = 100)
        {
            _rng = seed.HasValue ? new Random(seed.Value) : Random.Shared;
            _maxNodes = maxNodes;
            _nextId = 1;
            _allNodes = new List<HexNode>();
            _coordToNode = new Dictionary<HexCoord, HexNode>();
        }

        public HexNode CreateStartNode()
        {
            var start = new HexNode(0, NodeType.HexRoom, new HexCoord(0, 0));
            _allNodes.Add(start);
            _coordToNode[new HexCoord(0, 0)] = start;
            return start;
        }

        public void ExpandDepth(HexNode node, int depth)
        {
            if (depth <= 0) return;

            var freeDirs = GetFreeDirections(node);
            Shuffle(freeDirs);

            foreach (var dir in freeDirs)
            {
                if (!node.CanAddNeighbor())
                    break;

                var coord = node.Coord.Neighbor(dir);

                if (HasNeighborAt(node, coord))
                    continue;

                if (_coordToNode.TryGetValue(coord, out var existing))
                {
                    if (existing.CanAddNeighbor())
                    {
                        node.Neighbors.Add(existing);
                        existing.Neighbors.Add(node);
                    }
                }
                else
                {
                    if (_allNodes.Count >= _maxNodes)
                        continue;

                    var newNode = CreateRandomNode(coord);
                    _allNodes.Add(newNode);
                    _coordToNode[coord] = newNode;

                    node.Neighbors.Add(newNode);
                    newNode.Neighbors.Add(node);

                    TryConnectExisting(newNode);

                    ExpandDepth(newNode, depth - 1);
                }
            }
        }

        private void TryConnectExisting(HexNode node)
        {
            for (int i = 0; i < 6; i++)
            {
                if (!node.CanAddNeighbor())
                    break;

                var coord = node.Coord.Neighbor(i);
                if (!_coordToNode.TryGetValue(coord, out var existing))
                    continue;
                if (!existing.CanAddNeighbor())
                    continue;

                bool alreadyConnected = false;
                foreach (var n in node.Neighbors)
                {
                    if (n == existing)
                    {
                        alreadyConnected = true;
                        break;
                    }
                }
                if (alreadyConnected)
                    continue;

                if (_rng.NextDouble() < 0.4)
                {
                    node.Neighbors.Add(existing);
                    existing.Neighbors.Add(node);
                }
            }
        }

        private HexNode CreateRandomNode(HexCoord coord)
        {
            double roll = _rng.NextDouble();
            double cumulative = 0;
            foreach (var (type, weight) in TypeWeights)
            {
                cumulative += weight;
                if (roll <= cumulative)
                    return new HexNode(_nextId++, type, coord);
            }
            return new HexNode(_nextId++, NodeType.Passage, coord);
        }

        private List<int> GetFreeDirections(HexNode node)
        {
            var free = new List<int>();
            for (int dir = 0; dir < 6; dir++)
            {
                var coord = node.Coord.Neighbor(dir);
                bool occupied = false;
                foreach (var n in node.Neighbors)
                {
                    if (((HexNode)n).Coord == coord)
                    {
                        occupied = true;
                        break;
                    }
                }
                if (!occupied)
                    free.Add(dir);
            }
            return free;
        }

        private static bool HasNeighborAt(HexNode node, HexCoord coord)
        {
            foreach (var n in node.Neighbors)
                if (((HexNode)n).Coord == coord)
                    return true;
            return false;
        }

        private void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}
