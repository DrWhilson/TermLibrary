namespace TermLibrary.LocNodes;

public class Node
{
    public int id { get; protected set; } = 0;

    public string name { get; protected set; } = string.Empty;

    public int seed { get; protected set; } = (int)DateTime.Now.Ticks;
}
