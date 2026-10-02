namespace TermLibrary.LocNodes;

public class HexDeadEnd : Node
{
    public Node neighbor { get; private set; } = new Node();

    // public Node[] updown_neighbors { get; private set; } = new Node[2]; //TODO: return it!

    public HexDeadEnd()
    {
        name = "DeadEnd";
    }
}
