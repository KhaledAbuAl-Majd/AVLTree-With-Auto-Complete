using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVLTree_AutoComplete
{
    public class AVLNode
    {
        public string Value { get; set; }
        public AVLNode Left { get; set; }
        public AVLNode Right { get; set; }
        public int Height { get; set; }

        public AVLNode(string value)
        {
            this.Value = value;
            this.Height = 1;
        }
    }

    public class AVLTree
    {
        private AVLNode root;

        public void Insert(string value)
        {
            root = Insert(root, value);
        }

        private AVLNode Insert(AVLNode node, string value)
        {
            if (node == null)
                return new AVLNode(value);

            if (value.CompareTo(node.Value) < 0)
                node.Left = Insert(node.Left, value);
            else if (value.CompareTo(node.Value) > 0)
                node.Right = Insert(node.Right, value);
            else
                return node;

            UpdateHeight(node);
            return Balance(node);
        }

        void UpdateHeight(AVLNode node)
        {
            node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));
        }

        private int Height(AVLNode node)
        {
            return node != null ? node.Height : 0;
        }

        private int GetBalanceFactor(AVLNode node)
        {
            return node != null ? Height(node.Left) - Height(node.Right) : 0;
        }

        private AVLNode Balance(AVLNode node)
        {
            int balanceFactor = GetBalanceFactor(node);

            //RR
            if (balanceFactor > 1 && GetBalanceFactor(node.Left) >= 0)
            {
                return RightRotate(node);
            }

            //LL
            if (balanceFactor < -1 && GetBalanceFactor(node.Right) <= 0)
            {
                return LeftRotate(node);
            }

            //LR
            if (balanceFactor > 1 && GetBalanceFactor(node.Left) < 0)
            {
                node.Left = LeftRotate(node.Left);

                return RightRotate(node);
            }

            //RL
            if (balanceFactor < -1 && GetBalanceFactor(node.Right) > 0)
            {
                node.Right = RightRotate(node.Right);

                return LeftRotate(node);
            }

            return node;
        }

        public AVLNode RightRotate(AVLNode originalRoot)
        {
            AVLNode NewRoot = originalRoot.Left;

            AVLNode OriginalLeftChild = NewRoot.Right;

            originalRoot.Left = OriginalLeftChild;

            NewRoot.Right = originalRoot;

            UpdateHeight(originalRoot);
            UpdateHeight(NewRoot);

            return NewRoot;
        }

        public AVLNode LeftRotate(AVLNode originalRoot)
        {
            AVLNode NewRoot = originalRoot.Right;

            AVLNode originaRightChild = NewRoot.Left;

            originalRoot.Right = originaRightChild;

            NewRoot.Left = originalRoot;

            UpdateHeight(originalRoot);
            UpdateHeight(NewRoot);

            return NewRoot;
        }

        public void Delete(string value)
        {
            root = DeleteNode(root, value);
        }

        private AVLNode DeleteNode(AVLNode node, string value)
        {
            if (node == null)
                return node;

            if (value.CompareTo(node.Value) < 0)
                node.Left = DeleteNode(node.Left, value);
            else if (value.CompareTo(node.Value) > 0)
                node.Right = DeleteNode(node.Right, value);
            else
            {
                if (node.Left == null)
                    return node.Right;

                if (node.Right == null)
                    return node.Left;

                var temp = MinValueNode(node.Right);

                node.Value = temp.Value;

                node.Right = DeleteNode(node.Right, temp.Value);
            }

            UpdateHeight(node);

            return Balance(node);
        }

        private AVLNode MinValueNode(AVLNode node)
        {
            var current = node;

            while (current.Left != null)
                current = current.Left;

            return current;
        }

        public bool Exists(string value)
        {
            return Search(value) != null;
        }

        public AVLNode Search(string value)
        {
            return Search(root, value);
        }

        private AVLNode Search(AVLNode node, string value)
        {
            if (node == null || value == node.Value)
                return node;

            if (value.CompareTo(node.Value) < 0)
                return Search(node.Left, value);

            return Search(node.Right, value);
        }

        //public void PrintTree()
        //{
        //    PrintTree(root, 0);
        //    Console.WriteLine();
        //}

        //private void PrintTree(AVLNode node, int level)
        //{
        //    if (node == null)
        //        return;

        //    PrintTree(node.Right, level + 1);
        //    Console.WriteLine($"{new string(' ', level * 10)} {node.Value}\n");
        //    PrintTree(node.Left, level + 1);
        //}

        public void PrintTree()
        {
            PrintTree(root, "", true);
        }

        private void PrintTree(AVLNode node, string indent, bool last)
        {
            if (node != null)
            {
                Console.Write(indent);
                if (last)
                {
                    Console.Write("R----");
                    indent += "     ";
                }
                else
                {
                    Console.Write("L----");
                    indent += "|    ";
                }
                Console.WriteLine(node.Value);
                PrintTree(node.Left, indent, false);
                PrintTree(node.Right, indent, true);
            }
        }

        //DFS - Immediate Execution
        public IEnumerable<string> AutoComplete(string prefix)
        {
            List<string> result = new List<string>();
            AutoComplete(root, prefix, result);
            return result;
        }

        //DFS - Immediate Execution
        private void AutoComplete(AVLNode node, string prefix, List<string> results)
        {
            if (node != null)
            {
                if (node.Value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(node.Value);

                    AutoComplete(node.Left, prefix, results);
                    AutoComplete(node.Right, prefix, results);
                }
                else if (string.Compare(prefix, node.Value, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    AutoComplete(node.Left, prefix, results);
                }
                else
                    AutoComplete(node.Right, prefix, results);
            }

            results = new List<string>();
        }

        //BFS - Queue - Deffered Execution
        public IEnumerable<string> AutoComplete1(string prefix)
        {
            if (root == null)
                yield break;

            Queue<AVLNode> queue = new Queue<AVLNode>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current == null)
                    continue;

                if (current.Value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    yield return current.Value;

                    queue.Enqueue(current.Left);
                    queue.Enqueue(current.Right);
                }
                else if (string.Compare(prefix, current.Value, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    queue.Enqueue(current.Left);
                }
                else
                    queue.Enqueue(current.Right);
            }
        }

        //DFS - Stack - Deffered Execution
        public IEnumerable<string> AutoComplete2(string prefix)
        {
            if (root == null)
                yield break;

            Stack<AVLNode> stack = new Stack<AVLNode>();
            stack.Push(root);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                if (current == null)
                    continue;

                if (current.Value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    yield return current.Value;

                    stack.Push(current.Right);
                    stack.Push(current.Left);
                }
                else if (string.Compare(prefix, current.Value, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    stack.Push(current.Left);
                }
                else
                    stack.Push(current.Right);
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            AVLTree tree = new AVLTree();
            string[] words = { "Ahmad", "Mohammed", "Motasem", "Mohab", "Abla", "Abeer", "Abdullah", "Abbas", "Montaser", "Khalil", "Khalid" };

            foreach (var word in words)
            {
                tree.Insert(word);
            }
            tree.PrintTree();

            Console.WriteLine("\nEnter a prefix to search:\n");
            string prefix = Console.ReadLine();
            var completions = tree.AutoComplete(prefix);
            //var completions = tree.AutoComplete1(prefix);
            //var completions = tree.AutoComplete2(prefix);

            Console.WriteLine($"\nSuggestions for '{prefix}':\n");
            foreach (var completion in completions)
            {
                Console.WriteLine(completion);
            }
        }
    }
}
