using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day15
{
    class Program
    {
        static void Main(string[] args)
        {
            Timing example = new Timing(
                new Func<int, int>[]{
                    t => (4 + t)%5,
                    t => (1 + t)%2
                });
            example.run();

            Timing part1 = new Timing(
                new Func<int, int>[]{
                    t => (1  + t)%13,
                    t => (10 + t)%19,
                    t => (2  + t)%3,
                    t => (1  + t)%7,
                    t => (3  + t)%5,
                    t => (5  + t)%17
                });
            part1.run();

            Timing part2 = new Timing(
                new Func<int, int>[]{
                    t => (1  + t)%13,
                    t => (10 + t)%19,
                    t => (2  + t)%3,
                    t => (1  + t)%7,
                    t => (3  + t)%5,
                    t => (5  + t)%17,
                    t => (0  + t)% 11
                });
            part2.run();

            Console.ReadLine();
        }
    }

    public class Timing
    {
        // Array of Disc functions:
        // Function: f(time) => (disc_start_pos + time ) % positions
        private Func<int, int>[] discs;
        public Timing(Func<int, int>[] input)
        {
            discs = input;
        }


        public void run()
        {
            int timer = 0;
            while (true)
            {
                //if (timer % 1000 == 0)
                //    Console.WriteLine("Checking: {0}", timer);
                bool done = true;
                for (int disc = 0; disc < discs.Count(); disc++)
                {
                    if (discs[disc](timer + disc + 1) > 0)
                    {
                        done = false;
                        break;
                    }

                }
                if (done)
                {
                    Console.WriteLine("Done at: {0}", timer);
                    break;
                }
                timer++;
            }
        }

    }
}
