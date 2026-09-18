using System.Text.Json;
using System.Text.Json.Serialization;

namespace maps
{
    public class RuleMap
    {
        // Internal Class
        private class Node
        {
            public required string Name { get; set; }
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
            if (!File.Exists(file_path))
            {
                throw new FileNotFoundException($"File {file_path} does not exist");
            }

            string json_string = File.ReadAllText(file_path);
            return JsonSerializer.Deserialize<RuleMap>(json_string, jason_options)!;
        }

        public List<string> GetTargetsFor(string sourceNode)
        {
            var targets = new List<string>();
            foreach (var link in Links)
            {
                if (string.Equals(link.source, sourceNode, StringComparison.OrdinalIgnoreCase))
                    targets.Add(link.target);
            }
            return targets;
        }
    }
}
