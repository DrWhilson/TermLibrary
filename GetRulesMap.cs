using System.Text.Json;
using System.Text.Json.Serialization;

namespace maps
{
    public class RuleMap
    {
        // Internal Class
        private class Node
        {
            public required string name { get; set; }
        }

        private class Link
        {
            public required string target { get; set; }
            public required string source { get; set; }
        }

        // Target Lists
        [JsonInclude]
        private List<Node> Nodes { get; set; } = new List<Node>();

        [JsonInclude]
        private List<Link> Links { get; set; } = new List<Link>();

        private static readonly JsonSerializerOptions jason_options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
        };

        public static RuleMap LoadFromFile(string file_path)
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Path.GetFullPath(Path.Combine(exePath, @"..\..\..\"));
            string filePath = Path.Combine(projectRoot, file_path);

            if (!File.Exists(filePath)) //FIX: Was checking file_path instead of resolved filePath
            {
                throw new FileNotFoundException($"File {filePath} does not exist");
            }

            string json_string = File.ReadAllText(filePath); //FIX: Was reading from file_path instead of resolved filePath
            return JsonSerializer.Deserialize<RuleMap>(json_string, jason_options)!;
        }

        public List<string> GetLinksFor(string sourceNode)
        {
            var targets = new List<string>();
            foreach (var link in Links)
            {
                if (string.Equals(link.source, sourceNode, StringComparison.OrdinalIgnoreCase))
                    targets.Add(link.target);

                if (string.Equals(link.target, sourceNode, StringComparison.OrdinalIgnoreCase))
                    targets.Add(link.source); //FIX: Returns bidirectional links. JSON defines directed links (source->target), this makes graph undirected
            }
            return targets;
        }

        public List<string> GetAllNames()
        {
            return Nodes.Where(node => node?.name != null).Select(node => node.name!).ToList();
        }
    }
}
