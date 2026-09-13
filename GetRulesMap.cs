using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace RuleMap
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

        private static readonly JasonSerializerOptions jason_options = new JasonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        private static RuleMap LoadFromFile(string file_path)
        {
            if (!File.Exists(file_path)) { throw new FileNotFoundException($"File {file_path} does not exist"); }

            string json_string = File.ReadAllText(file_path);
            return JsonSerializer.Deserialize<RuleMap>(json_string, jason_options);
        }

        public RuleMap()
        {
            string path = "./rule.json";

            RuleMap rule_map = RuleMap.LoadFromFile();
        }
    }
}
