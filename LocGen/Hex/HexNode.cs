namespace TermLibrary.HexNodes;

public class Node
{
    public int id { get; protected set; } = 0;

    public string name { get; protected set; } = string.Empty;

    public long seed { get; protected set; } = DateTime.UtcNow.Ticks;
}
