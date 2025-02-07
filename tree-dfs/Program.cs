namespace TreeDfs;

class Node
{
    public string Value { get; set; }
    public IEnumerable<Node> Children { get; set; }

    public Node(string value, IEnumerable<Node> children)
    {
        Value = value;
        Children = children;
    }
}

class Program
{
    static void Main()
    {
        var root = new Node("root", new[]
        {
            new Node("child1", new[]
            {
                new Node("child1.1", new Node[0]),
                new Node("child1.2", new Node[0]),
            }),
            new Node("child2", new[]
            {
                new Node("child2.1", new Node[0]),
                new Node("child2.2", new Node[0]),
            }),
        });

        Dfs(root);
        Console.WriteLine();
        DfsNonRecursive(root);
    }

    static void Dfs(Node node)
    {
        Console.WriteLine(node.Value);
        foreach (var child in node.Children)
        {
            Dfs(child);
        }
    }

    static void DfsNonRecursive(Node root)
    {
        var path = new Stack<Node>();
        path.Push(root);
        while (path.Count > 0)
        {
            var node = path.Pop();
            Console.WriteLine(node.Value);
            foreach (var child in node.Children.Reverse())
            {
                path.Push(child);
            }
        }
    }
}
