using libraryNodes;

class Program
{
    static void Main()
    {
        var generator = new GraphGenerator(seed: null, maxNodes: 100);
        var current = generator.CreateStartNode();
        generator.ExpandDepth(current, depth: 2);

        while (true)
        {
            RoomRenderer.Render(current);

            Console.WriteLine("\n━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.Write("Куда идти? (1-{0}, q=выход): ", current.Neighbors.Count);

            var input = Console.ReadLine()?.Trim().ToLower();
            if (input == "q")
            {
                Console.Write("Сохранить граф в graph.dot? (y/n): ");
                if (Console.ReadLine()?.Trim().ToLower() == "y")
                {
                    var start = generator.AllNodes[0];
                    GraphExporter.ExportToDot(start, "graph.dot");
                    Console.WriteLine("Сохранено: graph.dot");
                }
                Console.WriteLine("Выход.");
                break;
            }

            if (
                !int.TryParse(input, out int choice)
                || choice < 1
                || choice > current.Neighbors.Count
            )
            {
                Console.WriteLine("Неверный ввод. Нажмите Enter...");
                Console.ReadLine();
                continue;
            }

            current = (HexNode)current.Neighbors[choice - 1];

            if (current.CanAddNeighbor())
                generator.ExpandDepth(current, depth: 2);
        }
    }
}
