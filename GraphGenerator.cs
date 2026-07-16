namespace libraryNodes
{
    public class GraphGenerator
    {
        private readonly Random _rng;
        private readonly int _maxNodes;
        private int _nextId;
        private readonly List<HexNode> _allNodes;
        private readonly Dictionary<HexCoord, HexNode> _coordToNode;

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

        public void GenerateGrid(int maxHexRooms = 20)
        {
            var start = CreateStartNode();
            var queue = new Queue<HexNode>();
            var enqueued = new HashSet<HexCoord> { start.Coord };
            queue.Enqueue(start);
            int hexRoomCount = 1;

            while (queue.Count > 0 && hexRoomCount < maxHexRooms && _allNodes.Count < _maxNodes)
            {
                var hexRoom = queue.Dequeue();

                for (int dir = 0; dir < 6; dir++)
                {
                    if (!hexRoom.CanAddNeighbor())
                        break;
                    if (_allNodes.Count >= _maxNodes)
                        break;

                    var passageCoord = hexRoom.Coord.Neighbor(dir);

                    if (_coordToNode.TryGetValue(passageCoord, out var existingNode))
                    {
                        if (
                            (existingNode.NodeType == NodeType.Passage || existingNode.NodeType == NodeType.DeadEnd)
                            && existingNode.CanAddNeighbor()
                            && !hexRoom.Neighbors.Contains(existingNode)
                        )
                        {
                            hexRoom.Neighbors.Add(existingNode);
                            existingNode.Neighbors.Add(hexRoom);
                        }
                        continue;
                    }

                    var corridorType = _rng.NextDouble() < 0.5 ? NodeType.Passage : NodeType.DeadEnd;
                    var passage = new HexNode(_nextId++, corridorType, passageCoord);
                    _allNodes.Add(passage);
                    _coordToNode[passageCoord] = passage;
                    hexRoom.Neighbors.Add(passage);
                    passage.Neighbors.Add(hexRoom);

                    var hexCoord2 = passageCoord.Neighbor(dir);

                    if (_coordToNode.TryGetValue(hexCoord2, out var hexRoom2))
                    {
                        if (
                            hexRoom2.NodeType == NodeType.HexRoom
                            && hexRoom2.CanAddNeighbor()
                            && passage.CanAddNeighbor()
                            && !passage.Neighbors.Contains(hexRoom2)
                        )
                        {
                            passage.Neighbors.Add(hexRoom2);
                            hexRoom2.Neighbors.Add(passage);
                        }
                    }
                    else if (hexRoomCount < maxHexRooms && _allNodes.Count < _maxNodes)
                    {
                        hexRoom2 = new HexNode(_nextId++, NodeType.HexRoom, hexCoord2);
                        _allNodes.Add(hexRoom2);
                        _coordToNode[hexCoord2] = hexRoom2;
                        hexRoomCount++;

                        passage.Neighbors.Add(hexRoom2);
                        hexRoom2.Neighbors.Add(passage);

                        if (!enqueued.Contains(hexCoord2))
                        {
                            enqueued.Add(hexCoord2);
                            queue.Enqueue(hexRoom2);
                        }
                    }
                }
            }
        }

        public void ExpandDepth(HexNode node, int depth)
        {
            if (depth <= 0)
                return;

            var newNodes = FillNodeSlots(node);
            ExpandChildren(newNodes, depth);
        }

        private List<HexNode> FillNodeSlots(HexNode node)
        {
            var freeDirs = GetFreeDirections(node);
            Shuffle(freeDirs);

            var newNodes = new List<HexNode>();

            foreach (var dir in freeDirs)
            {
                if (!node.CanAddNeighbor())
                    break;

                var coord = node.Coord.Neighbor(dir);

                if (HasNeighborAt(node, coord))
                    continue;

                if (_coordToNode.TryGetValue(coord, out var existing))
                {
                    if (existing.CanAddNeighbor() && CanConnectTypes(node, existing))
                    {
                        node.Neighbors.Add(existing);
                        existing.Neighbors.Add(node);
                    }
                }
                else
                {
                    if (_allNodes.Count >= _maxNodes)
                        continue;

                    var newNode = CreateRandomNode(coord, node.NodeType);
                    _allNodes.Add(newNode);
                    _coordToNode[coord] = newNode;

                    node.Neighbors.Add(newNode);
                    newNode.Neighbors.Add(node);

                    newNodes.Add(newNode);
                }
            }

            return newNodes;
        }

        private void ExpandChildren(List<HexNode> nodes, int depth)
        {
            foreach (var child in nodes)
            {
                TryConnectExisting(child);
                ExpandDepth(child, depth - 1);
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
                if (node.Neighbors.Contains(existing))
                    continue;

                if (_rng.NextDouble() < 0.4 && CanConnectTypes(node, existing))
                {
                    node.Neighbors.Add(existing);
                    existing.Neighbors.Add(node);
                }
            }
        }

        private HexNode CreateRandomNode(HexCoord coord, NodeType parentType)
        {
            NodeType type = parentType switch
            {
                NodeType.HexRoom => _rng.NextDouble() < 0.6 ? NodeType.Passage : NodeType.DeadEnd,
                NodeType.Passage => NodeType.HexRoom,
                _ => NodeType.Passage,
            };
            return new HexNode(_nextId++, type, coord);
        }

        private List<int> GetFreeDirections(HexNode node)
        {
            var free = new List<int>();
            for (int dir = 0; dir < 6; dir++)
            {
                var coord = node.Coord.Neighbor(dir);
                if (!HasNeighborAt(node, coord))
                    free.Add(dir);
            }
            return free;
        }

        private static bool HasNeighborAt(HexNode node, HexCoord coord)
        {
            foreach (var n in node.Neighbors)
                if (n.Coord == coord)
                    return true;
            return false;
        }

        private static bool CanConnectTypes(HexNode a, HexNode b)
        {
            if (a.NodeType == NodeType.HexRoom && b.NodeType == NodeType.HexRoom)
                return false;
            if (a.NodeType == NodeType.Passage && b.NodeType == NodeType.Passage)
                return false;
            return true;
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
