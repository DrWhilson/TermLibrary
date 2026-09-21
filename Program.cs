using maps;

class Program
{
    static void Main()
    {
        string path = "./rule.json";
        RuleMap rule_map = RuleMap.LoadFromFile(path);

        List<string> targets = rule_map.GetLinksFor("Hex");

        foreach (var target in targets)
        {
            Console.WriteLine($"- {target}");
        }
    }
}
