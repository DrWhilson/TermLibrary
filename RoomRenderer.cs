namespace libraryNodes
{
    public static class RoomRenderer
    {
        public static void ShowBookPage(string text, int pageNumber)
        {
            const int lineWidth = 44;
            const int pad = 2;
            int innerWidth = lineWidth + pad * 2;

            string[] lines = new string[25];
            for (int i = 0; i < 25; i++)
            {
                int start = i * lineWidth;
                if (start < text.Length)
                {
                    int len = Math.Min(lineWidth, text.Length - start);
                    lines[i] = text.Substring(start, len).PadRight(lineWidth);
                }
                else
                {
                    lines[i] = new string(' ', lineWidth);
                }
            }

            string pageStr = $"\u2500\u2500 \u0441\u0442\u0440. {pageNumber} \u2500\u2500";
            int leftPad = (lineWidth - pageStr.Length) / 2;
            int rightPad = lineWidth - pageStr.Length - leftPad;

            Console.WriteLine("\u250c" + new string('\u2500', innerWidth) + "\u2510");
            Console.WriteLine("\u2502" + new string(' ', innerWidth) + "\u2502");
            for (int i = 0; i < 25; i++)
                Console.WriteLine(
                    "\u2502" + new string(' ', pad) + lines[i] + new string(' ', pad) + "\u2502"
                );
            Console.WriteLine("\u2502" + new string(' ', innerWidth) + "\u2502");
            Console.WriteLine(
                "\u2502"
                    + new string(' ', pad)
                    + new string(' ', leftPad)
                    + pageStr
                    + new string(' ', rightPad)
                    + new string(' ', pad)
                    + "\u2502"
            );
            Console.WriteLine("\u2502" + new string(' ', innerWidth) + "\u2502");
            Console.WriteLine("\u2514" + new string('\u2500', innerWidth) + "\u2518");
        }

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
