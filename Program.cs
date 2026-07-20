using libraryNodes;

class Program
{
    const int ExpandDepth = 2;

    static void Main()
    {
        var generator = new GraphGenerator(seed: null, maxNodes: 5000);
        generator.GenerateGrid(maxHexRooms: 15);
        var current = generator.AllNodes[0];

        while (true)
        {
            RoomRenderer.Render(current);

            if (current.NodeType == NodeType.HexRoom)
                generator.ExpandFrom(current, ExpandDepth);

            var exits = current.Neighbors
                .Where(n => n.NodeType != NodeType.DeadEnd && n.NodeType != NodeType.PseudoDeadEnd)
                .ToList();

            Console.WriteLine("\n━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.Write("Куда идти? (1-{0}, q=выход): ", exits.Count);

            var input = Console.ReadLine()?.Trim().ToLower();
            if (input == "q")
            {
                var start = generator.AllNodes[0];
                GraphExporter.ExportToDot(start, "graph.dot");
                Console.WriteLine("Сохранено: graph.dot");
                Console.WriteLine("Выход.");
                break;
            }

            if (
                !int.TryParse(input, out int choice)
                || choice < 1
                || choice > exits.Count
            )
            {
                Console.WriteLine("Неверный ввод. Нажмите Enter...");
                Console.ReadLine();
                continue;
            }

            current = exits[choice - 1];
        }
    }
}
