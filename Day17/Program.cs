using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Day17
{
    class Program
    {
        static void Main(string[] args)
        {

            
            var example = new Maze("hijkl");
            Console.WriteLine("Example: {0} - {1}", example.pathToVault(), example.maxPathLength());

            var test1 = new Maze("ihgpwlah");
            Console.WriteLine("Test 1:  {0} - {1}", test1.pathToVault(), test1.maxPathLength());

            var test2 = new Maze("kglvqrro");
            Console.WriteLine("Test 2: {0} - {1} ", test2.pathToVault(), test2.maxPathLength());

            var test3 = new Maze("ulqzkmiv");
            Console.WriteLine("Test 3:  {0} - {1}", test3.pathToVault(), test3.maxPathLength());

            var part1 = new Maze("awrkjxxr");
            Console.WriteLine("Part 1:  {0} - {1}", part1.pathToVault(), part1.maxPathLength());



            Console.ReadLine();
        }
    }

    public class Maze
    {
        // U D L R
        State start;
        MD5 md5 = MD5CryptoServiceProvider.Create();
        Queue<State> statesToCheck;
        string seed;

        public Maze(string Seed)
        {
            seed = Seed;
            start = new State(seed, md5);
            start.currentRoom = new Point(0, 0);
            start.path = "";

        }


        public int maxPathLength()
        {
            statesToCheck = new Queue<State>();
            statesToCheck.Enqueue(start);

            int max = 0;
            while (statesToCheck.Count > 0)
            {
                var s = statesToCheck.Dequeue();
                if (s.isDone())
                {
                    //Console.WriteLine();
                    max = Math.Max(max, s.path.Length);
                    //return s.path;
                }else{
                    var openDoors = s.getOpenDoors(); // U D L R are open..
                    if (openDoors[0] && s.currentRoom.y > 0) // up
                        statesToCheck.Enqueue(s.move("U", 0, -1));
                    if (openDoors[1] && s.currentRoom.y < 3) // down
                        statesToCheck.Enqueue(s.move("D", 0, 1));
                    if (openDoors[2] && s.currentRoom.x > 0) // left
                        statesToCheck.Enqueue(s.move("L", -1, 0));
                    if (openDoors[3] && s.currentRoom.x < 3) // right
                        statesToCheck.Enqueue(s.move("R", 1, 0));

                    Console.Write("Queue length: {0}", statesToCheck.Count);
                    Console.SetCursorPosition(0, Console.CursorTop);
                }

            }
            Console.WriteLine();
            return Math.Max(0, max);;
        }

        public string pathToVault()
        {
            statesToCheck = new Queue<State>();
            statesToCheck.Enqueue(start);
         
            while (statesToCheck.Count > 0)
            {
                var s = statesToCheck.Dequeue();
                if (s.isDone())
                {
                    Console.WriteLine();
                    return s.path;
                }
                    var openDoors = s.getOpenDoors(); // U D L R are open..
                    if (openDoors[0] && s.currentRoom.y > 0) // up
                        statesToCheck.Enqueue(s.move("U", 0, -1));
                    if (openDoors[1] && s.currentRoom.y < 3) // down
                        statesToCheck.Enqueue(s.move("D", 0, 1));
                    if (openDoors[2] && s.currentRoom.x > 0) // left
                        statesToCheck.Enqueue(s.move("L", -1, 0));
                    if (openDoors[3] && s.currentRoom.x < 3) // right
                        statesToCheck.Enqueue(s.move("R", 1, 0));

                    Console.Write("Queue length: {0}", statesToCheck.Count);
                    Console.SetCursorPosition(0, Console.CursorTop);

            }
            Console.WriteLine();
            return "<None found>";
        }

    }

    public class State
    {
        public Point currentRoom;
        public string path;
        public string seed;
        MD5 md5;

        public State(string s, MD5 md5)
        {
            seed = s;
            this.md5 = md5;
        }

        public bool isDone()
        {
            return currentRoom.x == 3 && currentRoom.y == 3;
        }

        // U D L R are open..
        public bool[] getOpenDoors()
        {
            var result = new bool[4];
            var h = makeHash(seed + path, md5);
            for (int i = 0; i < 4; i++)
                result[i] = h[i] >= 'b' && h[i] <= 'f';
            return result;

        }

        private string makeHash(string input, MD5 md5)
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            return string.Concat(hash.Select(x => x.ToString("x2")));
        }

        // create new state, moved by dx, dy
        internal State move(string m, int dx, int dy)
        {
            var s = new State(seed, md5);
            s.currentRoom = new Point(currentRoom.x + dx, currentRoom.y + dy);
            s.path = path + m;
            return s;
        }
    }


    public class Point
    {
        public int x;
        public int y;
        public Point()
        {

        }
        public Point(int X, int Y)
        {
            x = X;
            y = Y;
        }
        public override string ToString()
        {
            return string.Format("({0},{1})", x, y);
        }
    }

}
