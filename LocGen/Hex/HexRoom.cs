namespace TermLibrary.HexNodes;

public class HexRoom : Node
{
    public int layer_id { get; private set; } = 0;
    public HexCorridor[] flat_neighbors { get; private set; } = new HexCorridor[6];

    // public Node[] updown_neighbors { get; private set; } = new Node[2]; //TODO: return it!

    public HexRoom(int new_id)
    {
        name = "HexRoom";
        id = new_id;
    }

    public HexRoom(int new_id, int new_layer_id)
    {
        name = "HexRoom";
        id = new_id;
        layer_id = new_layer_id;
    }

    public void AddNeighbour(HexCorridor new_neighbour)
    {
        if (new_neighbour.name != "HexCorridor")
            return;

        for (int i = 0; i < flat_neighbors.Length; i++)
            if (flat_neighbors[i] == null)
            {
                flat_neighbors[i] = new_neighbour;
                break;
            }
    }

    public void AddNeighbour(HexRoom new_neighbour)
    {
        if (new_neighbour.name != "HexRoom" || GetNextRooms().Contains(new_neighbour)) return;

        HexCorridor conection_corridor = new HexCorridor();

        this.AddNeighbour(conection_corridor);
        new_neighbour.AddNeighbour(conection_corridor);
    }

    public List<HexRoom> GetNextRooms()
    {
        List<HexRoom> next_rooms = new List<HexRoom>();
        foreach (HexCorridor cooridor in flat_neighbors)
            if (cooridor != null)
                next_rooms.Add(cooridor.GetOpositRoom(this));

        return next_rooms;
    }
}
