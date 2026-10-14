namespace TermLibrary.HexNodes;

public class HexCorridor : Node
{
    public bool IsDeadEnd { get; set; } = false;
    public bool IsRestRoom { get; set; } = false;
    public HexRoom[] lineal_neighbours = new HexRoom[2];

    // public Node stairs_neighbours = new Node();

    public HexCorridor(int new_id)
    {
        id = new_id;
        name = "HexCorridor";
    }

    public void AddNeighbour(HexRoom new_neighbour)
    {
        if (new_neighbour.name != "HexRoom")
            return;

        for (int i = 0; i < lineal_neighbours.Length; i++)
            if (lineal_neighbours[i] == null)
            {
                lineal_neighbours[i] = new_neighbour;
                break;
            }
    }

    public HexRoom GetOpositRoom(HexRoom start_room)
    {
        return lineal_neighbours.FirstOrDefault(val => val != start_room);
    }
}
