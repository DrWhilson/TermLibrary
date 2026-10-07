namespace TermLibrary.HexNodes;

public class HexRoom : Node
{
    public Node[] flat_neighbors { get; private set; } = new Node[6];

    // public Node[] updown_neighbors { get; private set; } = new Node[2]; //TODO: return it!

    public HexRoom(int new_id)
    {
        name = "Hex";
        id = new_id;
    }
}
