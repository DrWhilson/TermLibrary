namespace libraryNodes
{
    public class GraphNode<T>
    {
        public T Id { get; set; }
        public string Type { get; set; }
        public List<GraphNode<T>> Neighbors { get; set; }

        public GraphNode(T id, string type)
        {
            Id = id;
            Type = type;
            Neighbors = new List<GraphNode<T>>();
        }
    }
}
