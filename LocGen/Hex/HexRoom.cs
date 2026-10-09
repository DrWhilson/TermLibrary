namespace TermLibrary.HexNodes;

public class HexRoom : Node
{
    public Node[] flat_neighbors { get; private set; } = new Node[6];
    // public Node[] updown_neighbors { get; private set; } = new Node[2]; //TODO: return it!

    public HexRoom(int new_id)
    {
        name = "HexRoom";
        id = new_id;
    }

    public void AddNeighbour(Node new_neighbour)
    {
        if (new_neighbour.name != "HexCorridor") return;

        for (int i = 0; i < flat_neighbors.Length; i++)
            if (flat_neighbors[i] != null)
            {
                flat_neighbors[i] = new_neighbour;
                break;
            }
    }
}
