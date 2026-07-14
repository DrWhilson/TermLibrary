namespace libraryNodes
{
    public static class RoomRenderer
    {
        public static void Render(HexNode node)
    {
        Console.Clear();
        DrawRoom(node);
        DrawExits(node);
        DrawActions(node);
    }

        private static void DrawRoom(HexNode node)
        {
            switch (node.NodeType)
            {
                case NodeType.HexRoom:
                    DrawHexRoom(node);
                    break;
                case NodeType.Passage:
                    DrawPassage(node);
                    break;
                case NodeType.DeadEnd:
                    DrawDeadEnd(node);
                    break;
            }
        }

        private static void DrawHexRoom(HexNode node)
        {
            Console.WriteLine("    +----------+");
            Console.WriteLine("   /            \\");
            Console.WriteLine("  |     [{0}]     |", node.Id);
            Console.WriteLine("  |  HexRoom    |");
            Console.WriteLine("   \\            /");
            Console.WriteLine("    +----------+");
        }

        private static void DrawPassage(HexNode node)
        {
            Console.WriteLine("+----------------+");
            Console.WriteLine("|      [{0}]       |", node.Id);
            Console.WriteLine("|    Passage     |");
            Console.WriteLine("+----------------+");
        }

        private static void DrawDeadEnd(HexNode node)
        {
            Console.WriteLine("+--------------+");
            Console.WriteLine("|     [{0}]      |", node.Id);
            Console.WriteLine("|   DeadEnd    |");
            Console.WriteLine("+--------------+");
        }

        private static void DrawExits(HexNode node)
        {
            if (node.Neighbors.Count == 0)
            {
                Console.WriteLine("\nНет выходов.");
                return;
            }

            Console.WriteLine("\nВыходы:");
            for (int i = 0; i < node.Neighbors.Count; i++)
            {
                var neighbor = (HexNode)node.Neighbors[i];
                Console.WriteLine("  {0}) [{1}] {2}", i + 1, neighbor.Id, neighbor.NodeType);
            }
        }

        public static void DrawActions(HexNode node)
        {
            if (node.NodeType == NodeType.HexRoom)
            {
                Console.WriteLine("\nДействия:");
                Console.WriteLine("  {0}) Взять книгу", node.Neighbors.Count + 1);
            }
        }
    }
}
