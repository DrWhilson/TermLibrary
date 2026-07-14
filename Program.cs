using libraryNodes;

class Program
{
    static void Main()
    {
        var generator = new GraphGenerator(seed: null, maxNodes: 100);
        var current = generator.CreateStartNode();
        generator.ExpandNode(current, 2);

        while (true)
        {
            RoomRenderer.Render(current);

            bool hasActions = current.NodeType == NodeType.HexRoom;
            int actionCount = hasActions ? 1 : 0;
            int totalOptions = current.Neighbors.Count + actionCount;

            Console.WriteLine("\n━━━━━━━━━━━━━━━━━━━━━━━━");

            if (hasActions)
                Console.Write("Куда идти? (1-{0}, {1}=взять книгу, q=выход): ",
                    current.Neighbors.Count, current.Neighbors.Count + 1);
            else
                Console.Write("Куда идти? (1-{0}, q=выход): ", current.Neighbors.Count);

            var input = Console.ReadLine()?.Trim().ToLower();
            if (input == "q")
            {
                Console.WriteLine("Выход.");
                break;
            }

            if (!int.TryParse(input, out int choice) || choice < 1 || choice > totalOptions)
            {
                Console.WriteLine("Неверный ввод. Нажмите Enter...");
                Console.ReadLine();
                continue;
            }

            if (choice <= current.Neighbors.Count)
            {
                current = (HexNode)current.Neighbors[choice - 1];

                if (current.CanAddNeighbor())
                    generator.ExpandNode(current, 2);
            }
            else if (hasActions && choice == current.Neighbors.Count + 1)
            {
                var random = new Random();
                const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var text = new char[1100];
                for (int i = 0; i < 1100; i++)
                    text[i] = chars[random.Next(chars.Length)];
                RoomRenderer.ShowBookPage(new string(text), random.Next(1, 999));
                Console.WriteLine("\nНажмите Enter...");
                Console.ReadLine();
            }
        }
    }
}
