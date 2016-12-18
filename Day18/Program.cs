using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day18
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = new Tiles(Input.test);
            t.run(10);

            var p1 = new Tiles(Input.part1);
            p1.run(40);

            var p2 = new Tiles(Input.part1);
            p2.run(400000);


            Console.ReadLine();
        }
    }

    public class Tiles
    {
        bool[] row;
        public Tiles(string startRow)
        {
            row = new bool[startRow.Length];
            for (int i = 0; i < startRow.Length; i++)
                row[i] = startRow[i] == '^'; // true are traps!

        }

        public void run(int count)
        {
            var total = 0;
            total += countSafe(row);
            //Console.WriteLine("{0}\t{1}\t{2}", 1,total, print(row));
            for (int i = 0; i < count-1; i++)
            {
                row = nextRow(row);
                total += countSafe(row);
                //Console.WriteLine("{0}\t{1}\t{2}", i+2,total, print(row));
            }

            Console.WriteLine(total);
        }

        public string print(bool[] row)
        {
            StringBuilder sb = new StringBuilder();
            foreach(bool b in row)
                sb.Append(b?"^":".");
            return sb.ToString();
        }

        public bool[] nextRow(bool[] row)
        {
            bool[] next = new bool[row.Length];
            bool L, C,R;
            for (int i = 0; i < row.Length; i++)
            {
                if (i == 0)
                    L = false;
                else
                    L = row[i - 1];
                if (i == row.Length - 1)
                    R = false;
                else
                    R = row[i + 1];
                C = row[i];

                next[i] = (L && C && !R) ||
                          (!L && C && R) ||
                          (L && !C && !R) ||
                          (!L && !C && R);
            }
            return next;
        }

        public int countSafe(bool[] row)
        {
            return row.Count(x => !x);
        }
    }

    static public class Input
    {
        public const string test = ".^^.^.^^^^";
        public const string part1 = "^.....^.^^^^^.^..^^.^.......^^..^^^..^^^^..^.^^.^.^....^^...^^.^^.^...^^.^^^^..^^.....^.^...^.^.^^.^";
    }
}
