using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day24
{
    class Program
    {
        static void Main(string[] args)
        {

            var m = new Maze();
            m.readMaze(Input.Maze);
            m.printMaze();


            //var path = m.FindPath(m.getNode(m.goals[0]), m.getNode(m.goals[3]));
            //Console.WriteLine("From {0} to {1} takes {2} steps", m.goals[0].goalId, m.goals[3].goalId, path.Count);


            // calculate all distances between goals and from start.
            for (int i = 0; i < m.goals.Count; i++)
            {
                for (int j = i + 1; j < m.goals.Count; j++)
                {
                    var path = m.FindPath(m.getNode(m.goals[i]), m.getNode(m.goals[j]));
                    //m.printMaze();
                    //m.printPathToNode(m.getNode(m.goals[j]));
                    //Console.ReadLine();
                    Console.WriteLine("From {0} to {1} takes {2} steps",m.goals[i].goalId, m.goals[j].goalId, path.Count);
                    //m.printPathToNode(m.getNode(m.goals[1]));
                }
            }
            m.goals =  m.goals.OrderBy(g => g.goalId).ToList();

            var all = GetPermutations(new int[]{1,2,3,4,5,6,7} , 7);
            int min = int.MaxValue;
            foreach (var a in all)
            {
                //from 0 to start
                var run = 0;
                var prev = 0;
                Console.Write("0");
                for(int i = 0; i< a.Count(); i++){
                    Console.Write("-->{0}",m.goals[a.ElementAt(i)].goalId);
                    run += m.FindPath(m.getNode(m.goals[prev]), m.getNode(m.goals[a.ElementAt(i)])).Count();
                    prev = a.ElementAt(i);
                }
                Console.Write("-->{0}", m.goals[0].goalId);
                run += m.FindPath(m.getNode(m.goals[prev]), m.getNode(m.goals[0])).Count();
                if (run < min)
                   min = run;
     
                Console.WriteLine(min);
            }
            Console.WriteLine(min);
            Console.ReadLine();
        
        }

        static IEnumerable<IEnumerable<T>>
            GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }
    }

    public class Maze
    {
        public void readMaze(string input)
        {


            var lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            w = lines[0].Length;
            h = lines.Length;

            nodes = new Node[h, w]; //y,x 

            for (var i = 0; i < h; i++)
            { //y
                for (var j = 0; j < w; j++)
                { // x
                    nodes[i, j] = new Node(j, i, lines[i][j] != '#');
                    if (lines[i][j] != '.' && lines[i][j] != '#')
                    {
                        var g = new Point(j, i);
                        g.goalId = int.Parse("" + lines[i][j]);
                        goals.Add(g);
                    }
                }
            }
        }

        int w;
        int h;

        Node[,] nodes;
        List<Node> nextNodes;
        public List<Point> goals = new List<Point>();

        public List<Point> FindPath(Node startNode, Node endNode)
        {
            // Reset nodes for current goal.
            resetNodes(endNode.Location);
            startNode.State = NodeState.Open;

            // The start node is the first entry in the 'open' list
            List<Point> path = new List<Point>();
            bool success = Search(startNode, endNode.Location, nextNodes);
            if (success)
            {
                // If a path was found, follow the parents from the end node to build a list of locations
                Node node = endNode;
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

        private void resetNodes(Point currentGoal)
        {
            foreach (var n in nodes)
            {
                n.State = NodeState.Untested;
                n.Parent = null;
                n.H = Node.GetTraversalCost(n.Location, currentGoal);
                n.G = 0;
            }
            nextNodes = new List<Node>();
        }


        public void printPathToNode(Node n)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            var current = n;
            while (current != null)
            {
                Console.SetCursorPosition(current.Location.X, current.Location.Y);
                Console.Write("@");
                current = current.Parent;
            }
        }

        public Node getNode(Point p)
        {
            return getNode(p.X, p.Y);
        }
        public Node getNode(int x, int y)
        {
            return this.nodes[y, x];
        }

        public void printMaze()
        {
            Console.BufferWidth = w + 1;
            Console.SetCursorPosition(0, 0);
            Console.SetWindowSize(w + 1, h + 10);
            Console.ForegroundColor = ConsoleColor.White;
            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    Console.Write(isWall(i, j) ? "#" : ".");
                    //var n = getNode(i,j);
                    //Console.Write(n.IsWalkable? n.State== NodeState.Open?"O": n.State == NodeState.Closed? "X":"." : "#"); 
                }

                Console.WriteLine();
            }
            foreach (var goal in goals)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(goal.X, goal.Y);
                Console.Write(goal.goalId);
            }
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
            return !getNode(x, y).IsWalkable;
        }

        public bool Search(Node currentNode, Point goal, List<Node> nextNodes)
        {
            //printMaze();
            //printPathToNode(currentNode);
            //Console.ReadLine();

            currentNode.State = NodeState.Closed;
            nextNodes.Remove(currentNode);
            //printNodes(10,10);
            if (!currentNode.IsWalkable)
                return false;

            nextNodes.AddRange(GetAdjacentValidNodes(currentNode));
            nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));
            foreach (var nextNode in nextNodes)
            {
                if (nextNode.Location.X == goal.X && nextNode.Location.Y == goal.Y)
                {
                    return true;
                }
                else
                {
                    if (Search(nextNode, goal, nextNodes))
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
                if (value != null)
                    this.G = this.parentNode.G + GetTraversalCost(this.Location, this.parentNode.Location);
                else
                    this.G = 0;
            }
        }
        public Node(int x, int y, bool isWalkable)
        {
            this.Location = new Point(x, y);
            this.State = NodeState.Untested;
            this.IsWalkable = isWalkable;
            //this.H = GetTraversalCost(this.Location, endLocation);
            this.G = 0;
        }

        public static float GetTraversalCost(Point location, Point otherLocation)
        {
            float deltaX = otherLocation.X - location.X;
            float deltaY = otherLocation.Y - location.Y;
            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            //return Math.Abs(deltaX) + Math.Abs(deltaY);
        }
    }
    public enum NodeState { Untested, Open, Closed }
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int goalId { get; set; }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public override bool Equals(object obj)
        {
            var p = obj as Point;
            if (p != null)
                return false;
            return this.X == p.X && this.Y == p.Y;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }


    public static class Input
    {
        public const string Example =
@"###########
#0.1.....2#
#.#######.#
#4.......3#
###########";
        public const string Maze =
@"#####################################################################################################################################################################################
#.....#.........#.#...#.....#.............#.......#.....#.....#...........#...#.........#.#.#.....#.......#...............#..........3#.#.#.....#.......#...#.....#...#.#.#.....#...#
#.###.#.#.###.#.#.#.#.#.#.#.#.#.#####.#####.#.###.#.#.#######.###.#.#######.#.#.#.#.#.#.#.#.#.#####.#.#.###.#######.#.###.###.#.#.#.#.#.#.#.#.#.#.#.#.#####.#.###.#.#.#.#.###.#.###.#
#.......#.#...#...#.#...#...#.#...#...#.#...#.....#...#.#.....#.....#.....#.......#...#...#.................#.#.............#...#.....#.........#...#...#.#...#...#.....#.......#...#
#.#.#.###.#.#.###.#.#.#.#.###.#.###.###.#.#.#.#######.#.#####.#.#.#####.#.#.#.#####.#.###.#.#####.#####.#.###.###.###.#####.#.#.#.#.#.#.#.#.#.#.###.###.#.#.#.#.#####.#.#.#.#.#.#####
#..1#.......#...........#...#.........#.#.....#...#.#...#.........#...#...#...#.....#.#...#.#.#.....#...#.#.#...#.......#.........#.......#...#.#...#.....#.#.....#...#...#..2#.....#
#.#####.###.#.#.#.###.###.###.#####.#.#.#.#.###.#.#.#.#.#####.###.#.#.#####.#.#.#.#.#.#.###.#.#.#.#.#.#.#.#.#.#.#.#.#####.###.#.#.#####.###.###.#.#.#.###.#.#####.#.#.#.###.#.#.#.#.#
#...#.............#.#...#.#...#...#.#.#...#...#.............#.#.....#.........#.........#.#...#.#.#.#...#.......#.#.......#...#...#.#.......#...#.#.....#.........#.#.#.#.........#.#
#.#.#.###.###.#.#.#.#.#.#.#.#.#.#.#.###.###.###.#.#####.#.#.#.###.#.#.#.#####.#.#.###.#.#.#.#.#.###.#.#.#.#####.#####.###.#.#.#.#.#.#.#######.#.#.#.#.#.#.#######.#.#.#.#.###.#.#.#.#
#.......#...#.....#.....#.......#...#.....#.#.#.........#.......#.#.....#...#.#...#...#.#.....#...#.#...........#.........#...#.#.#...#.#.....#.....#.....#...........#...#.......#.#
#####.###.#.###########.###.#.###.###.###.#.#.#.###.#.###.###.#.#.#.#####.#.#.#.#########.#####.#.#.###.#.#.#.#.#.###.#.#.#.###.#.#####.#.#.#.#.#.#.#.#####.###.#####.###.#.#.#.#.#.#
#...#...#.......#.....#.....#.....#.....#.......#.#.#.....#...........#.....#.#.#.#.......#.....#.......#...........#.#...#...#.#.......#...#.....#...#.#...#.#...#...#.....#.....#.#
#.#.###.#.###.#.#####.#.#.#.#.#.#.###.#.###.###.#.#.#.###.#.#.#.###.#.#.###.#.#.#.#.#.#.#.###.#.#.#######.###.#######.#.###.###.#.###.#.#.#.###.###.#.#.#.#.#.#.#.#.###.#.#.###.#.#.#
#.....#...#.........#...#...#.#.#.........#.#.#...#.#...#.#...#.#.........#.....#.#...#.#...#...#.......#.....#...#...#.#.....#.......#.#...#...#.........#.#...#.#.........#.#...#.#
#.###.###########.#.###.#.#.#.#####.#.#.#.###.#.#.#.#####.#.###.#.#.#######.#####.#.#.#.#.###.#.#.###.#.#.#####.###.#.#.#.#.#.#########.###.#.#.#.#.#.###.#.#.#.#.#.#.#.#.#.#.#####.#
#.#.................#.............#...#...#.#.#.#...#...#...#.....#.......#.#.#...#...........#.........#.......#.........#...#...#...#.........#.#...#...#.........#.........#...#.#
#.#.#.#####.#######.###.###.###.#.#######.#.#.###.###.#.#.#.#####.#####.###.#.#.#.#.###.#.###.#.#.#####.#.#.#.#.#.#.###.#.#.#.#.#.#.#.#.###.###.#######.###.#.#.#.#.#.#.###.#.#.#.#.#
#...#.#.#...................#0............#...........#.#.....#.#.....#.#.........#.....#.......#.......#.....#.......#.#...#.......#.#.#...#.............#...#.....#.#.......#...#6#
#.#.#.#.#.###.#.#.#.#.#####.###.#.#.#####.#####.###.#.###.###.#.#.#.#.#.#.#####.#.#.#.#.#.#####.#.###.#.#####.#.#####.#.#.#.#.#####.#.#.#.#.#.#.#.#########.#.###.###.#######.#.#.###
#.#...#.#.......#.#.#.#.....#...#...#.#...#...#.#...#.........#...#...#...#.....#.....#.....#...#.....#.......#.....#...#...#.#.....#.#...#.#.#.#.#.......#...#.......#...#...#...#.#
#.###.#.###.#.#.#.#.#.###.#.#.#.###.#.###.#.#.#.#.###.#.#.#.#.#.#.#####.#.#####.#.#####.#.#.#.###.#.#############.###.###.###.###########.#.###.#.#.#.###.#.###.###.#.#.#.#.#.#.###.#
#.....#.#...#...#...#...#.#.#.........#.....#...#...#.#.....#...#.#...........#.#.......#...#.#.......#.#...#.........#...#...#.#.#.....#...#.#.#.#.......#...........#...#.#.......#
#.#.#####.#.###########.#.#.#.#############.#.#.#.#.#######.#######.###.#.###.###.###.#######.#.###.#.#.#.#.#######.###.###.###.#.#.#.#.#.#.#.#.#.#.###.#.#######.###.###.#.#.#.#####
#...#.#.......#.................#.#.........#.....#.#.#.....#...#.....#.......#...#...#.......#.#...#.#.#...#...........#.#.#.....#.#.........#...#.#...........#...#.....#...#.#...#
###.#.#.#.###.#.#.#.#.#.#.###.#.#.#.#.#.#.###.#.#.#.#.#.#.#.###.#.###.#.###.###.#.#####.#####.###.#.#.#.#######.#.#.#.#.#.#.#.###.#.#.###.#.#.###.#.#.#####.#.#.#.###.#.#.###.#.#.#.#
#...#...#.....#.#.#...#...#...#...#.............#.....#...#.#.#...#.............#.#.............#...#.#.#...#.#.#...#.#...#.#.#.......#.#.......#...#.#.....#...#...#.#...#.#...#...#
#.#.#.#.#.#####.#.#.#.#.#.#.#######.###.#######.#.###.#.###.#.#.#.###.#.#.###.#.#.#.#.#.#.#.###.###.#.###.###.###.###.###.###.###.#####.#######.###.#.###.#.#.###.#.#.#.###.###.#.#.#
#...#.#.#.......#.#.#...#...........#.........#.#.#...#.#.#.#.#.#.............#...#...#...#.....#.......#...#.#...#...#...#...#.........#...#...#.....#.#.....#.#.#...#...#.#...#...#
###.#.#.#.###.###.###.#.#####.#.#.#.#.#.#####.###.#.###.#.#.#.#.###.#.###.###.###.#.#.#.###.###.###.###.###.###.#.#.###.#.#.#.#.###.#.#.#.#.#.#.#.#.#.#.#.#.#.#.#.#.#.###.#.###.###.#
#...#...#.#...#...#.#.#.......#...#...#.#.......#.......#.#.....#.........#...........#.....#...#...#.......#...........#...#...#.#.#...#.......#...#.....#.....#.#....5#.....#.....#
#.#.#.#####.#.#.#.#.###.#.#.#.###.#.#.###.#####.#.#.#.#.###.#.#.#.#.#.#.#.#.#.#.#.###############.#.###.#.#.#.###.###.#.#.#.#.###.#.#.###.#####.#.#.#####.###.###.#.#.#.###.#.#.#.#.#
#.........#.#...#.....#...#.#.#...#.....#...#.....#.....#...#.#.#...#.#.....#...#.............#...#.#.....#.#.....#...........#...#.............#...#...#.#...#...#.#.......#...#...#
#.#.#.#.#.#.###.#####.###.#.#.#.#.#.###.#.###.#.###.#.#.#.#.#.###.#.#.#.#####.#.#####.#####.###.#.#.#.#############.#####.#.###.#.###.#.#.#.#.#####.#.#.#.#.#.#.#.#.#.#.#.#.#.#######
#4#.#.....#.#.....#...#...#...#...#...#.#.#...#...#...#.#.....#...#...#.........#...#.#.....#...#.#...#.#.....#.#.#...#...#.#...#.#.......#.#.......#...#.......#.#.#.#.#.........#.#
#####.#.###.###.###.#####.###.#.#.###.#.#.#.#.#.#.#.#.#####.#.#.#.#.###.#.#.#.#.#.#.#.#.#.###.#.#.###.#.#.#.#.#.#.###.#.#.###.#.#.###.#.#.#.###.###.#.#.#.#.#####.#.###.#.#####.###.#
#.......#...#...#...#.#.#.........#...#.#7#.#...#...#.......#.#.#.#.....#.#.....#.....#.....#...#.#.#.#...........#...#.....#.............#...............#.....#.........#...#.....#
#####################################################################################################################################################################################";

    }
}
