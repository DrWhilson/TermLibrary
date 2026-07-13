namespace libraryNodes
{
    public class GraphGenerator
    {
        private readonly Random _rng;
        private readonly int _maxNodes;
        private int _nextId;
        private readonly List<HexNode> _allNodes;

        private static readonly (NodeType type, double weight)[] TypeWeights =
        [
            (NodeType.HexRoom, 0.30),
            (NodeType.Passage, 0.50),
            (NodeType.DeadEnd, 0.20)
        ];

        public IReadOnlyList<HexNode> AllNodes => _allNodes;

        public GraphGenerator(int? seed = null, int maxNodes = 100)
        {
            _rng = seed.HasValue ? new Random(seed.Value) : Random.Shared;
            _maxNodes = maxNodes;
            _nextId = 1;
            _allNodes = new List<HexNode>();
        }

        public HexNode CreateStartNode()
        {
            var start = new HexNode(0, NodeType.HexRoom);
            _allNodes.Add(start);
            return start;
        }

        public int ExpandNode(HexNode node, int count)
        {
            int added = 0;
            int openSlots = node.MaxNeighbors - node.Neighbors.Count;
            int canAdd = Math.Min(Math.Min(count, openSlots), _maxNodes - _allNodes.Count);

            for (int i = 0; i < canAdd; i++)
            {
                var newNode = CreateRandomNode();
                _allNodes.Add(newNode);

                node.Neighbors.Add(newNode);
                newNode.Neighbors.Add(node);

                added++;
            }

            return added;
        }

        private HexNode CreateRandomNode()
        {
            double roll = _rng.NextDouble();
            double cumulative = 0;
            foreach (var (type, weight) in TypeWeights)
            {
                cumulative += weight;
                if (roll <= cumulative)
                    return new HexNode(_nextId++, type);
            }
            return new HexNode(_nextId++, NodeType.Passage);
        }

        // Legacy batch generation
        public HexNode Generate()
        {
            var start = CreateStartNode();
            var expandable = new Queue<HexNode>();
            expandable.Enqueue(start);

            while (expandable.Count > 0 && _allNodes.Count < _maxNodes)
            {
                var current = expandable.Dequeue();
                int openSlots = current.MaxNeighbors - current.Neighbors.Count;
                if (openSlots <= 0) continue;

                int neighborsToAdd = Math.Min(
                    _rng.Next(1, openSlots + 1),
                    _maxNodes - _allNodes.Count
                );

                for (int i = 0; i < neighborsToAdd; i++)
                {
                    var newNode = CreateRandomNode();
                    _allNodes.Add(newNode);

                    current.Neighbors.Add(newNode);
                    newNode.Neighbors.Add(current);

                    if (newNode.CanAddNeighbor())
                        expandable.Enqueue(newNode);

                    if (_allNodes.Count >= _maxNodes)
                        break;
                }
            }

            return start;
        }
    }
}
