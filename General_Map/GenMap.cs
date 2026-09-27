using maps;

namespace TermLibrary.General_Map;

public class GenMap
{
    private List<Node> all_locations { get; set; } = new List<Node>();

    private RuleMap rule_map { get; set; }

    private bool CheckReachable(Node start_node, (Node start, Node end) ignored_link)
    {
        if (start_node == null || ignored_link.start == null || ignored_link.end == null)
            return false;

        HashSet<Node> visited = new HashSet<Node>();

        return DeepSearch(start_node, (ignored_link.start, ignored_link.end), visited)
            == all_locations.Count;
    }

    private int DeepSearch(Node current, (Node start, Node end) ignored_link, HashSet<Node> visited)
    {
        visited.Add(current);
        int count = 1;

        foreach (Node neighbor in current.GetAllNeighbours())
        {
            if (
                !visited.Contains(neighbor)
                && (current != ignored_link.start && neighbor != ignored_link.end)
                && (current != ignored_link.end && neighbor != ignored_link.start)
            ) // WARN: Not working method
            {
                count += DeepSearch(neighbor, ignored_link, visited);
            }
        }

        return count;
    }

    private void GenerateBaseMap()
    {
        Node start_hex = new Node("Hex");
        all_locations.Add(start_hex);

        GenerateNodes(); // Gen couple of locations

        GenerateFullLink(); // Gen max of links

        DropSomeLinks(50);
    }

    private void GenerateNodes()
    {
        List<string> posible_locations = rule_map.GetAllNames();

        foreach (string unique_location in posible_locations)
        {
            for (int i = 0; i < Random.Shared.Next(3, 8); i++) // Random number of any locations
            {
                Node newLocation = new Node(unique_location);
                all_locations.Add(newLocation);
            }
        }
    }

    private void TryLink(Node node1, Node node2)
    {
        if (node1 == node2)
            return;

        if (!rule_map.GetLinksFor(node1.GetName()).Contains(node2.GetName()))
            return;

        node1.AddNewNeighbor(node2);
        node2.AddNewNeighbor(node1);
    }

    private void GenerateFullLink()
    {
        foreach (Node location in all_locations)
            foreach (Node other_locations in all_locations)
                TryLink(location, other_locations);
    }

    private void DropLink(Node node1, Node node2)
    {
        node1.DropNeighbour(node2);
        node2.DropNeighbour(node1);
    }

    private void DropSomeLinks(int percent)
    {
        foreach (Node location in all_locations)
        {
            List<Node> neighbour = location.GetAllNeighbours();
            for (int i = 0; i < neighbour.Count; i++)
            {
                if (
                    Random.Shared.Next(100) < percent
                    && CheckReachable(location, (location, neighbour[i]))
                )
                    DropLink(location, neighbour[i]);
            }
        }
    }

    public GenMap()
    {
        string path = @"rule.json";
        rule_map = RuleMap.LoadFromFile(path);

        Node start_hex = new Node("Hex");
        all_locations.Add(start_hex);

        // TODO: Check save map
        GenerateBaseMap();

        // TODO: Save map
    }
}
