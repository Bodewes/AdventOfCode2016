using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("{0}", new CityBlockWalker().Walk("R2, L3"));
            System.Console.WriteLine("{0}", new CityBlockWalker().Walk("R2, R2, R2"));
            System.Console.WriteLine("{0}", new CityBlockWalker().Walk("R5, L5, R5, R3"));
            System.Console.WriteLine("{0}", new CityBlockWalker().Walk("L4, L1, R4, R1, R1, L3, R5, L5, L2, L3, R2, R1, L4, R5, R4, L2, R1, R3, L5, R1, L3, L2, R5, L4, L5, R1, R2, L1, R5, L3, R2, R2, L1, R5, R2, L1, L1, R2, L1, R1, L2, L2, R4, R3, R2, L3, L188, L3, R2, R54, R1, R1, L2, L4, L3, L2, R3, L1, L1, R3, R5, L1, R5, L1, L1, R2, R4, R4, L5, L4, L1, R2, R4, R5, L2, L3, R5, L5, R1, R5, L2, R4, L2, L1, R4, R3, R4, L4, R3, L4, R78, R2, L3, R188, R2, R3, L2, R2, R3, R1, R5, R1, L1, L1, R4, R2, R1, R5, L1, R4, L4, R2, R5, L2, L5, R4, L3, L2, R1, R1, L5, L4, R1, L5, L1, L5, L1, L4, L3, L5, R4, R5, R2, L5, R5, R5, R4, R2, L1, L2, R3, R5, R5, R5, L2, L1, R4, R3, R1, L4, L2, L3, R2, L3, L5, L2, L2, L1, L2, R5, L2, L2, L3, L1, R1, L4, R2, L4, R3, R5, R3, R4, R1, R5, L3, L5, L5, L3, L2, L1, R3, L4, R3, R2, L1, R3, R1, L2, R4, L3, L3, L3, L1, L2"));
            System.Console.WriteLine("{0}", new CityBlockWalker().Walk("R8, R4, R4, R8"));
            System.Console.ReadLine();
        }
    }

    public class CityBlockWalker
    {
        int x = 0, y = 0;
        int h = 0; // heading 0 is north, 1 = east, 2 = south, 3 = west, clockwise
        List<Tuple<Direction, int>> steps;
        List<Tuple<int, int>> visited;

        public int Walk(string input)
        {
            visited = new List<Tuple<int, int>>();
            visited.Add(new Tuple<int, int>(0, 0));

            steps = parseInput(input);

            foreach (var step in steps)
            {
                int dx, dy = 0;
                if (step.Item1 == Direction.Right)
                {
                    h = (h + 1) % 4;
                }
                else
                {
                    h = ((h - 1) + 4) % 4;
                }
                switch (h)
                {
                    case 0:
                        dx = 1;
                        dy = 0; break;
                    case 1:
                        dx = 0;
                        dy = 1; break;
                    case 2:
                        dx = -1;
                        dy = 0;
                        ; break;
                    case 3:
                        dx = 0;
                        dy = -1; break;
                    default: throw new Exception();
                }
                for (int s = 0; s < step.Item2; s++)
                {
                    if (Step(dx, dy))
                    {
                        return Math.Abs(x) + Math.Abs(y);
                    }
                }

            }
            return Math.Abs(x) + Math.Abs(y);
        }

        // return true if not a new location.
        private bool Step(int deltaX, int deltaY)
        {
            x += deltaX;
            y += deltaY;
            var newlocation = new Tuple<int, int>(x, y);
            if (visited.Contains(newlocation))
            {
                return true;
            }
            visited.Add(newlocation);
            return false;
        }

        private List<Tuple<Direction, int>> parseInput(string input)
        {

            var tokens = input.Split(new string[] { ", " }, StringSplitOptions.None);
            return tokens.Select(
                      item => new Tuple<Direction, int>(
                        item[0] == 'R' ? Direction.Right : Direction.Left,
                        int.Parse(item.Substring(1))
                      )
                    ).ToList();
        }

        enum Direction
        {
            Left,
            Right
        }

    }
}
