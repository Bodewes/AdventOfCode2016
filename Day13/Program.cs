using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowHeight = 60;

            Maze exampleMaze = new Maze(10, new Point(1, 1), new Point(7, 4));
            exampleMaze.printMaze(20, 20);
            var path = exampleMaze.FindPath();
            printPath(path);
            Console.Write("Path: {0}", path.Count);
            Console.ReadLine();
            Console.Clear();

            Maze real = new Maze(1352, new Point(1, 1), new Point(31, 39));
            real.printMaze(40, 44);
            Console.ReadLine();
            var realPath = real.FindPath();
            printPath(realPath);
            Console.SetCursorPosition(0, 0);
            Console.Write("Path: {0}", realPath.Count);
            Console.ReadLine();

        }

        static void printPath(List<Point> path)
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (Point p in path)
            {
                Console.SetCursorPosition(p.X, p.Y);
                Console.Write("@");
            }
        }
    }

    public class Maze
    {

        int s;
        Node startNode;
        Node endNode;
        Node[,] nodes = new Node[100, 100]; // max? replace with endless storage.
        Point goal;
        public Maze(int seed, Point start, Point end)
        {
            goal = end;
            s = seed;
            startNode = getNode(start);
            startNode.State = NodeState.Open;
            endNode = getNode(end);
        }

        public List<Point> FindPath()
        {
            // The start node is the first entry in the 'open' list
            List<Point> path = new List<Point>();
            bool success = Search(startNode);
            if (success)
            {
                // If a path was found, follow the parents from the end node to build a list of locations
                Node node = this.endNode;
                while (node.Parent != null)
                {
                    path.Add(node.Location);
                    node = node.Parent;
                }

                // Reverse the list so it's in the correct order when returned
                path.Reverse();
            }

            return path;
        }


        private void printPathToNode(Node n)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            var current = n;
            while (current != null)
            {
                Console.SetCursorPosition(n.Location.X,n.Location.Y);
                Console.Write("@");
                current = current.Parent;
            }
        }

        private Node getNode(Point p)
        {
            return getNode(p.X, p.Y);
        }
        private Node getNode(int x, int y)
        {
            if (this.nodes[x, y] == null)
            {
                this.nodes[x, y] = new Node(x, y, !isWall(x, y), goal);
            }
            return this.nodes[x, y];
        }

        public void printMaze(int w, int h)
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;
            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                    Console.Write(isWall(i, j) ? "#" : ".");
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(goal.X, goal.Y);
            Console.Write("*");
        }

        public void printNodes(int w, int h)
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;
            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    if (nodes[i, j] != null)
                        Console.Write(nodes[i, j].State == NodeState.Open ? "O" : "x");
                    else
                        Console.Write(" ");
                }
                    
                Console.WriteLine();
            }
        }

        public bool isWall(int x, int y)
        {
            var w = s + x * x + 3 * x + 2 * x * y + y + y * y;
            bool even = true; // 0 bits one, thus even
            while (w > 0)
            {
                if ((w & 0x1) == 1)
                    even = !even;
                w >>= 1;
            }
            return !even;
        }

        private bool Search(Node currentNode)
        {
            printPathToNode(currentNode);
            //printNodes(10,10);
            List<Node> nextNodes = GetAdjacentValidNodes(currentNode);
            nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));
            foreach (var nextNode in nextNodes)
            {
                if (nextNode.Location.X == goal.X && nextNode.Location.Y == goal.Y)
                {
                    return true;
                }
                else
                {
                    if (Search(nextNode))
                        return true;
                }

            }
            return false;
        }

        private List<Node> GetAdjacentValidNodes(Node fromNode)
        {
            List<Node> walkableNodes = new List<Node>();
            IEnumerable<Point> nextLocations = GetAdjacentLocations(fromNode.Location);
            foreach (var location in nextLocations)
            {
                int x = location.X;
                int y = location.Y;

                // Stay within the grid's boundaries
                if (x < 0 || y < 0)
                    continue;

                Node node = getNode(location);

                // Ignore non-walkable nodes
                if (!node.IsWalkable)
                    continue;

                // Ignore already-closed nodes
                if (node.State == NodeState.Closed)
                    continue;

                // Already-open nodes are only added to the list if their G-value is lower going via this route.
                if (node.State == NodeState.Open)
                {
                    float traversalCost = Node.GetTraversalCost(node.Location, fromNode.Location);
                    float gTemp = fromNode.G + traversalCost;
                    if (gTemp < node.G)
                    {
                        node.Parent = fromNode;
                        walkableNodes.Add(node);
                    }
                }
                else
                {
                    // If it's untested, set the parent and flag it as 'Open' for consideration
                    node.Parent = fromNode;
                    node.State = NodeState.Open;
                    walkableNodes.Add(node);
                }
            }

            return walkableNodes;
        }

        private IEnumerable<Point> GetAdjacentLocations(Point p)
        {
            List<Point> locs = new List<Point>();
            locs.Add(new Point(p.X + 1, p.Y));
            locs.Add(new Point(p.X - 1, p.Y));
            locs.Add(new Point(p.X, p.Y + 1));
            locs.Add(new Point(p.X, p.Y - 1));
            return locs;
        }



    }

    public class Node
    {
        public Point Location { get; set; }
        public bool IsWalkable { get; set; }
        public float F { get { return this.G + this.H; } }
        public float G { get; set; }
        public float H { get; set; }
        public NodeState State { get; set; }
        Node parentNode;
        public Node Parent
        {
            get { return this.parentNode; }
            set
            {
                // When setting the parent, also calculate the traversal cost from the start node to here (the 'G' value)
                this.parentNode = value;
                this.G = this.parentNode.G + GetTraversalCost(this.Location, this.parentNode.Location);
            }
        }
        public Node(int x, int y, bool isWalkable, Point endLocation)
        {
            this.Location = new Point(x, y);
            this.State = NodeState.Untested;
            this.IsWalkable = isWalkable;
            this.H = GetTraversalCost(this.Location, endLocation);
            this.G = 0;
        }

        internal static float GetTraversalCost(Point location, Point otherLocation)
        {
            float deltaX = otherLocation.X - location.X;
            float deltaY = otherLocation.Y - location.Y;
         //   return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            return Math.Abs(deltaX) + Math.Abs(deltaY);
        }
    }
    public enum NodeState { Untested, Open, Closed }


    public class Point 
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public override bool Equals(object obj)
        {
            var p = obj as Point;
            if (p!= null)
                return false;
            return this.X == p.X && this.Y == p.Y;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }



}

