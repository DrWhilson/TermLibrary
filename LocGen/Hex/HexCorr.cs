namespace TermLibrary.HexNodes;

public class HexCorridor : Node
{
    public bool IsDeadEnd { get; set; } = false;
    public Node[] lineal_neighbours = new Node[2];
    public Node stairs_neighbours = new Node();

    public HexCorridor(int new_id)
    {
        id = new_id;
        name = "HexCorridor";
    }

    public void AddNeighbour(Node new_neighbour)
    {
        if (new_neighbour.name != "HexRoom") return;

        for (int i = 0; i < lineal_neighbours.Length; i++)
            if (lineal_neighbours[i] != null)
            {
                lineal_neighbours[i] = new_neighbour;
                break;
            }
    }
}
