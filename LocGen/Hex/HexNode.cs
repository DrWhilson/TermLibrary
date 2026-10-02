namespace TermLibrary.LocNodes;

public class HexRoom : Node
{
    public Node[] flat_neighbors { get; private set; } = new Node[6];

    // public Node[] updown_neighbors { get; private set; } = new Node[2]; //TODO: return it!

    public HexRoom()
    {
        name = "Hex";
    }
}
