namespace TermLibrary.LocNodes;

public class HexCorridor : Node
{
    public Node[] lineal_neighbours = new Node[2];

    public Node stairs_neighbours = new Node();

    public HexCorridor()
    {
        name = "HexCorridor";
    }
}
