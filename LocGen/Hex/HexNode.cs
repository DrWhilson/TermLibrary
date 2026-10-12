namespace TermLibrary.HexNodes;

public class Node
{
    public int id { get; protected set; } = 0;

    public string name { get; protected set; } = string.Empty;

    public int seed { get; protected set; } = (int)DateTime.Now.Ticks; //FIX: Truncates Ticks to int (loss of precision). Multiple rapid instantiations get same seed
}
