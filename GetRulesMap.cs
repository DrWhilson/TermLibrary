using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace maps
{
    public class RuleMap
    {
        // Internal Class
        private class Node
        {
            public string Name { get; set; }
        }

        private class Link
        {
            public string target { get; set; }
            public string source { get; set; }
        }

        // Target Lists
        private List<Node> Nodes { get; set; } = new List<Node>();
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
            return JsonSerializer.Deserialize<RuleMap>(json_string, jason_options);
        }

        // public List<string> GetTergetsFor(string source_node)
        // {
        //     var targets = new List<string>();
        //     foreach (var link in Links)
        //     {
        //         if (string.Equals((link.source, source_node, StringComparison.OrdinalIgnoreCase)))
        //             targets.Add(link.Target);
        //     }
        //     return targets;
        // }
    }
}
