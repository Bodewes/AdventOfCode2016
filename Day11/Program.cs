using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            //var rtg = new RTG();
            //rtg.states.Add(new ExampleState(1,   1, 1, 2, 3));
            //rtg.solve();

            //var rtgDay11 = new RTG();
            //rtgDay11.states.Add(new RealState(1,   1, 1, 2, 1, 2, 1, 3, 3, 3, 3));
            //rtgDay11.solve();

            var rtg = new RTG();
            var start = new State(1, new int[] { 2 + 1 * 16, 3 + 1 * 16 });
            //var start = new State(1, new int[] { 1 + 1 * 16, 1 + 2 * 16, 1 + 2 * 16});
            //var start = new State(1, new int[] { 1 + 1 * 16, 1 + 2 * 16, 1 + 2 * 16, 3 + 3 * 16, 3 + 3 * 16 });
            start.print();
            Console.ReadLine();
            rtg.states.Add(start);
            rtg.solve();
            Console.ReadLine();

            // combinations in array, one int per combination
            // Genertors: 1,2,3,4
            // Chips:     16, 17, 18,19   (1+0xf);
        }
    }


    public class RTG
    {
        public List<State> states = new List<State>();
        List<State> visitedStates = new List<State>();

        public RTG()
        {
        }

        public void solve()
        {

            int steps = 0;

            while (true)
            {
                steps++;

                List<State> validNewStates = new List<State>();
                // generate all next states
                // Move all states to visited states
                foreach (var state in states)
                {
                    // check in current state
                    visitedStates.Add(state);
                    Console.SetCursorPosition(0, 0);
                    state.print();

                    IEnumerable<State> newStates = state.GenerateNewStates();

                    // Check states
                    // Keep nexts possible states.
                    foreach (var ns in newStates)
                    {
                        if (ns.isDone())
                        {
                            ns.print();
                            Console.WriteLine("DONE in {0}", steps);
                            return;
                        }

                        if (ns.isValid()) // new state is valid
                        {
                            if (!visitedStates.Contains(ns))
                            { // and not visited before.
                                Console.SetCursorPosition(0, 7);
                                ns.print();
                                validNewStates.Add(ns);
                                Console.ReadLine();
                            }
                            else
                            {
                                //nnop
                            }
                        }

                    }
                }

                states = validNewStates;
                if (states.Count() == 0)
                {
                    Console.WriteLine("States dried up");
                    return;
                }

                System.Console.SetCursorPosition(0, 15);
                Console.WriteLine("Steps: {0}, Visited: {1},   States to check: {2}", steps,visitedStates.Count(), states.Count());
            }
        }

    }




    public class State : IEquatable<State>
    {
        readonly int[] items;
        readonly int e;
        // combinations in array, one int per combination
        // Genertors: 1,2,3,4
        // MChips:     16, 17, 18,19   (1+0xf);


        public State(int e, int[] start)
        {
            Array.Sort(start);
            items = start;
            this.e = e;
        }

        public const int G = 0x0f; // generator mask
        public const int M = 0xf0; // chips mask.

        public void print()
        {
            for (int level = 4; level > 0; level--)
            {
                Console.Write("F{0}  ", level);
                Console.Write("{0} ", e == level ? "EE" : "..");
                for (int c = 0; c < items.Length; c++)
                {
                    if ((items[c] & G) == level) Console.Write((char)(c + 'a') + "G "); else Console.Write(".. ");
                    if ((items[c] & M) == level * 16) Console.Write((char)(c + 'a') + "M "); else Console.Write(".. ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Valid: {0}  Done: {1}, State: {2}", isValid(), isDone(), String.Join(",", items));

        }

        public bool isValid()
        {
            // any unpaired generator on a floor with other generators is doomed

            for (int c = 0; c < items.Length; c++) // check all items
            {
                int m = lm(items[c]); // level chip.
                if (lg(items[c]) == lm(items[c])) // if level generator and level mchip are the same: safe!
                {

                }
                else
                {
                    for (int c2 = 0; c2 < items.Length; c2++) // check others if G at same level as current M
                    {
                        if (c2 == c) continue; // skip self
                        if (lg(items[c2]) == m)
                        {
                            return false; // found a generator at same level as loney chip M.
                        }
                    }
                }
            }
            return true;
        }

        // level generator
        private int lg(int x)
        {
            return x & G;
        }
        //level microchip
        private int lm(int x)
        {
            return (x & M) / 16;
        }

        // all combinations must be '0x44';
        public bool isDone()
        {
            for (int c = 0; c < items.Length; c++)
            {
                if (items[c] != 0x44)
                    return false;
            }
            return true;
        }

        public bool Equals(State other)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != other.items[i])
                    return false;
            }
            if (e != other.e)
                return false;
            return true;
        }

        public IEnumerable<State> GenerateNewStates()
        {
            // new states:
            // loop over all items
            List<State> newStates = new List<State>();
            for (int i = 0; i < items.Length; i++)
            {

                if (lm(items[i]) == e && lg(items[i]) == e) // move both
                    Two(newStates, i, 'G', i, 'M');

                if (lm(items[i]) == e) // chip at current level
                {
                    One(newStates, i, 'M'); // one chip 
                    // find others combos
                    for (int j = i + 1; j < items.Length; j++)
                    {
                        if (lm(items[j]) == e)
                            Two(newStates, i, 'M', j, 'M'); // two up, chips
                        if (lg(items[j]) == e)
                            Two(newStates, i, 'M', j, 'G'); // two up, chip and gen.
                    }
                }

                if (lg(items[i]) == e)
                {
                    One(newStates, i, 'G'); // one ge
                    for (int j = i + 1; j < items.Length; j++)
                    {
                        if (lm(items[j]) == e)
                            Two(newStates, i, 'G', j, 'M'); // two up, chips
                        if (lg(items[j]) == e)
                            Two(newStates, i, 'G', j, 'G'); // two up, chip and gen.
                    }
                }
            }
            return newStates;

        }

        private void One(List<State> newStates, int i, char type)
        {
            if (e > 1)
                One(newStates, i, type, -1);
            if (e < 4)
                One(newStates, i, type, 1);
        }

        private void One(List<State> newStates, int i, char type, int delta)
        {
            var t = (int[])items.Clone();
            if (type == 'G')
                t[i] += delta;
            if (type == 'M')
                t[i] += delta * 16;
            newStates.Add(new State(e + delta, t));
        }

        private void Two(List<State> states, int i, char type_i, int j, char type_j)
        {
            if (e > 1)
                Two(states, i, type_i, j, type_j, -1);
            if (e < 4)
                Two(states, i, type_i, j, type_j, 1);
        }


        private void Two(List<State> states, int i, char type_i, int j, char type_j, int delta)
        {
            var t = (int[])items.Clone();
            if (type_i == 'G')
                t[i] += delta;
            if (type_i == 'M')
                t[i] += delta * 16;

            if (type_j == 'G')
                t[j] += delta;
            if (type_j == 'M')
                t[j] += delta * 16;
            states.Add(new State(e + delta, t));
        }




    }

}

