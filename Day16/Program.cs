using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    class Program
    {
        static void Main(string[] args)
        {
            Dragon d = new Dragon();
            var result = d.Run(d.Read("10000"), 20);
            Console.WriteLine( result);

            using (Timer t = new Timer())
            {
                var part1 = d.Run(d.Read("01000100010010111"), 272);
                Console.WriteLine(part1);
            }

            using (Timer t = new Timer())
            {
                var part2 = d.Run(d.Read("01000100010010111"), 35651584);
                Console.WriteLine(part2);
            }

            Console.ReadLine();
        }
    }

    public class Dragon
    {

        public void Print(bool[] input)
        {
            Console.WriteLine( String.Join("", input.Select(x=> x?"1":"0") ) );
        }

        public bool[] Read(string input)
        {
            return input.ToCharArray().Select(c => c == '1').ToArray();

        }

        public string Run(bool[] input, int size)
        {
            //Print(input);
            var t = input;
            while (t.Length < size)
            {
                t = Step(t);
               // Print(t);
            }

            var t2 = t.Take(size).ToArray();
           // Print(t2);

            while (t2.Length % 2 == 0)
            {
                t2 = Checksum(t2);
                //Print(t2);
            }
            return String.Join("", t2.Select(x => x ? "1" : "0"));
        }


        public bool[] Step(bool[] input)
        {
            bool[] result = new bool[input.Length*2+1];

            var b = input.Reverse().Select(x => !x);
            result = input.Concat(new bool[] {false}.AsEnumerable()).Concat(b).ToArray();

            return result;
        }

        public bool[] Checksum(bool[] input)
        {
            bool[] result = new bool[input.Length / 2];
            for (int i = 0; i < input.Length/2; i++)
            {
                result[i] = input[i*2] == input[i*2 + 1];
            }
            return result;
        }
    }

    public class Timer : IDisposable
    {
        private readonly Stopwatch _sw;
        public Timer()
        {
            _sw = new Stopwatch();
            _sw.Start();
        }

        public void Dispose()
        {
            _sw.Stop();
            Console.WriteLine("Running time: {0}",_sw.Elapsed);
        }
    }
    
}
