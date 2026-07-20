namespace libraryNodes
{
    public static class GraphExporter
    {
        public static void ExportToDot(HexNode start, string filePath)
        {
            var visited = new HashSet<int>();
            var nodes = new List<HexNode>();
            var edges = new HashSet<(int, int)>();

            var queue = new Queue<HexNode>();
            queue.Enqueue(start);
            visited.Add(start.Id);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                nodes.Add(node);

                foreach (var neighbor in node.Neighbors)
                {
                    var a = Math.Min(node.Id, neighbor.Id);
                    var b = Math.Max(node.Id, neighbor.Id);
                    edges.Add((a, b));

                    if (!visited.Contains(neighbor.Id))
                    {
                        visited.Add(neighbor.Id);
                        queue.Enqueue((HexNode)neighbor);
                    }
                }
            }

            using var writer = new StreamWriter(filePath);
            writer.WriteLine("graph G {");
            writer.WriteLine("  layout=neato;");
            writer.WriteLine("  overlap=false;");
            writer.WriteLine("  splines=true;");
            writer.WriteLine("  node [style=filled];");
            writer.WriteLine();

            foreach (var node in nodes)
            {
                var (shape, color) = node.NodeType switch
                {
                    NodeType.HexRoom => ("hexagon", "#AED6F1"),
                    NodeType.Passage => ("box", "#F9E79F"),
                    NodeType.DeadEnd => ("square", "#F5B7B1"),
                    NodeType.Transition => ("hexagon", "#82E0AA"),
                    NodeType.PseudoDeadEnd => ("square", "#D5D8DC"),
                    _ => ("ellipse", "#FFFFFF")
                };

                writer.WriteLine(
                    "  {0} [label=\"[{0}] {1}\\n{2}\", shape={3}, fillcolor=\"{4}\"];",
                    node.Id, node.NodeType, node.Coord, shape, color
                );
            }

            writer.WriteLine();

            foreach (var (a, b) in edges)
                writer.WriteLine("  {0} -- {1};", a, b);

            writer.WriteLine("}");
        }
    }
}
