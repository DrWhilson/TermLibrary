namespace libraryNodes
{
    public static class RoomRenderer
    {
        private static readonly Dictionary<NodeType, string[]> Templates = new()
        {
            [NodeType.HexRoom] =
            [
                "    +----------+",
                "   /            \\",
                "  |     [{0}]     |",
                "  |  HexRoom    |",
                "   \\            /",
                "    +----------+",
            ],
            [NodeType.Passage] =
            [
                "+----------------+",
                "|      [{0}]       |",
                "|    Passage     |",
                "+----------------+",
            ],
            [NodeType.DeadEnd] =
            [
                "+--------------+",
                "|     [{0}]      |",
                "|   DeadEnd    |",
                "+--------------+",
            ],
        };

        public static void Render(HexNode node)
        {
            Console.Clear();
            DrawRoom(node);
            DrawExits(node);
        }

        private static void DrawRoom(HexNode node)
        {
            Console.WriteLine("  Координаты: {0}\n", node.Coord);

            var template = Templates[node.NodeType];
            foreach (var line in template)
                Console.WriteLine(line, node.Id);
        }

        private static void DrawExits(HexNode node)
        {
            var exits = node.Neighbors.Where(n => n.NodeType != NodeType.DeadEnd).ToList();

            if (exits.Count == 0)
            {
                Console.WriteLine("\nВсе проходы заблокированы.");
                return;
            }

            Console.WriteLine("\nВыходы:");
            for (int i = 0; i < exits.Count; i++)
            {
                var neighbor = exits[i];
                Console.WriteLine(
                    "  {0}) [{1}] {2}  {3}",
                    i + 1,
                    neighbor.Id,
                    neighbor.NodeType,
                    neighbor.Coord
                );
            }
        }
    }
}
