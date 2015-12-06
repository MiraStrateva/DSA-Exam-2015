using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// 
/// AhoCorasick is copied from Demo sources along with used classes
/// 
namespace Words2
{
    class Node
    {
        public Node()
        {
            this.Children = new Dictionary<char, Node>();
            this.Index = -1;
            this.Faillink = null;
            this.Successlink = null;
        }

        public Dictionary<char, Node> Children { get; set; }
        public int Index { get; set; }
        public Node Faillink { get; set; }
        public Node Successlink { get; set; }
    }


    class Program
    {
        static Dictionary<string, int> matches = new Dictionary<string, int>();
        static void Push(Node root, string str, int i)
        {
            foreach (char c in str)
            {
                if (!root.Children.ContainsKey(c))
                {
                    root.Children.Add(c, new Node());
                }

                root = root.Children[c];
            }
            root.Index = i;
        }

        // BFS
        static void Precompute(Node root)
        {
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                Node x = queue.Dequeue();
                foreach (var pair in x.Children)
                {
                    Node y = x.Faillink;
                    queue.Enqueue(pair.Value);

                    while (y != null && !y.Children.ContainsKey(pair.Key))
                    {
                        y = y.Faillink;
                    }

                    if (y == null)
                    {
                        pair.Value.Faillink = root;
                    }
                    else
                    {
                        pair.Value.Faillink = y.Children[pair.Key];
                    }

                    if (pair.Value.Faillink.Index >= 0)
                    {
                        pair.Value.Successlink = pair.Value.Faillink;
                    }
                    else
                    {
                        pair.Value.Successlink = pair.Value.Faillink.Successlink;
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            string pattern = Console.ReadLine();
            string text = Console.ReadLine();
            List<string> patterns = new List<string>();

            patterns.Add(pattern);
            for (int i = 1; i < pattern.Length; i++)
            {
                var prefix = pattern.Substring(0, i);
                var suffix = pattern.Substring(i);
                patterns.Add(prefix);
                patterns.Add(suffix);
            }

            Aho(patterns, text);

            int matchTotalCount = 0;
            if (matches.ContainsKey(pattern))
            {
                matchTotalCount += matches[pattern];
                
            }

            for (int i = 1; i < pattern.Length; i++)
            {
                var prefix = pattern.Substring(0, i);
                var suffix = pattern.Substring(i);
                var pc = 0;
                if (matches.ContainsKey(prefix))
                {
                    pc = matches[prefix];
                }
                var sc = 0;
                if (matches.ContainsKey(suffix))
                {
                    sc = matches[suffix];
                }

                matchTotalCount += pc*sc;
            }

            Console.WriteLine(matchTotalCount);
        }

        static void Aho(List<string> patterns, string text)
        {
            Node root = new Node();

            for (int i = 0; i < patterns.Count; i++)
            {
                Push(root, patterns[i], i);
            }

            Precompute(root);

            int n = text.Length;

            Node current = root;
            for (int i = 0; i < n; i++)
            {
                while (current != null && !current.Children.ContainsKey(text[i]))
                {
                    current = current.Faillink;
                }

                if (current == null)
                {
                    current = root;
                }
                else
                {
                    current = current.Children[text[i]];
                }

                if (current.Index >= 0)
                {
                    if (!matches.ContainsKey(patterns[current.Index]))
                    {
                        matches.Add(patterns[current.Index], 0);
                    }
                    matches[patterns[current.Index]]++;
                }

                Node x = current.Successlink;
                while (x != null)
                {
                    if (!matches.ContainsKey(patterns[x.Index]))
                    {
                        matches.Add(patterns[x.Index], 0);
                    }
                    matches[patterns[x.Index]]++;

                    x = x.Successlink;
                }
            }
        }
    }
}
