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
            //rtg.states.Add(new ExampleState(1, 1, 1, 2, 3));
            //rtg.solve();

            var rtgDay11 = new RTG();
            rtgDay11.states.Add(new RealState(1, 1, 1, 2, 1, 2, 1, 3, 3, 3, 3));
            rtgDay11.solve();
            Console.ReadLine();
        }
    }


    public class RTG
    {
        public List<RealState> states = new List<RealState>();
        List<RealState> visitedStates = new List<RealState>();

        public RTG()
        {
        }

        public void solve()
        {

            int steps = 0;

            while (true)
            {
                List<RealState> newStates = new List<RealState>();
                List<RealState> validNewStates = new List<RealState>();
                // generate all next states
                // Move all states to visited states
                foreach (var state in states)
                {
                    newStates.AddRange(state.GenerateNewStates());
                    visitedStates.Add(state);
                }
                steps++;

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
                            validNewStates.Add(ns);
                        }
                        else
                        {
                            //nnop
                        }
                    }

                }

                states = validNewStates;

                System.Console.SetCursorPosition(0, 10);
                Console.WriteLine("Steps: {0}, States: {1}", steps, states.Count());
            }
        }

    }


    public interface IState : IEquatable<IState>
    {
        bool isValid();
        bool isDone();
        void print();
        IEnumerable<IState> GenerateNewStates();

    }

    public class RealState : IEquatable<RealState>
    {
        public readonly int e;
        public readonly int ThM; // Thulium
        public readonly int ThG;
        public readonly int PoM; // Plutonium
        public readonly int PoG;
        public readonly int StM; // Strontium
        public readonly int StG;
        public readonly int PrM; // Prometium
        public readonly int PrG;
        public readonly int RuM; // Ruthenium
        public readonly int RuG;

        public RealState(int e, int ThM, int ThG, int PoM, int PoG, int StM, int StG, int PrM, int PrG, int RuM, int RuG)
        {
            this.e = e;
            this.ThM = ThM;
            this.ThG = ThG;
            this.PoM = PoM;
            this.PoG = PoG;
            this.StM = StM;
            this.StG = StG;
            this.PrM = PrM;
            this.PrG = PrG;
            this.RuM = RuM;
            this.RuG = RuG;
        }

        public bool isValid()
        {
            // M types must have G if other G are present
            if ((ThM == PoG || ThM == StG || ThM == PrG || ThM == RuG) && (ThM != ThG))
                return false;
            if ((PoM == ThG || PoM == StG || PoM == PrG || PoM == RuG) && (PoM != PoG))
                return false;
            if ((StM == ThG || StM == PoG || StM == PrG || StM == RuG) && (StM != StG))
                return false;
            if ((PrM == ThG || PrM == PoG || PrM == StG || PrM == RuG) && (PrM != PrG))
                return false;
            if ((RuM == ThG || RuM == PoG || RuM == StG || RuM == PrG) && (RuM != RuG))
                return false;

            return true;

        }

        public bool isDone()
        {
            return
                ThM == 4 && ThG == 4 &&
                PoM == 4 && PoG == 4 &&
                StM == 4 && StG == 4 &&
                PrM == 4 && PrG == 4 &&
                RuM == 4 && RuG == 4;

        }

        public void print()
        {
            System.Console.SetCursorPosition(0, 0);
            for (int i = 1; i < 5; i++)
            {
                Console.WriteLine("F{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11}", i, e == i ? "E " : ".",
                ThM == i ? "ThM" : " . ",
                ThG == i ? "ThG" : " . ",
                PoM == i ? "PoM" : " . ",
                PoG == i ? "PoG" : " . ",
                StM == i ? "StM" : " . ",
                StG == i ? "StG" : " . ",
                PrM == i ? "PrM" : " . ",
                PrG == i ? "PrG" : " . ",
                RuM == i ? "RuM" : " . ",
                RuG == i ? "RUG" : " . "
                    );
            }
        }

        public IEnumerable<RealState> GenerateNewStates()
        {
            var newState = new List<RealState>();
            if (e == 1)  // only up
            {
                SingleMove(e, 1, newState);
                DuoMove(e, 1, newState);
            }else if (e== 2)
            {
                SingleMove(e, 1, newState);
                DuoMove(e, 1, newState);
                // don't move down if empty below.
                if (ThG >= 2 && ThM >= 2 && PoG >= 2 && PoM >= 2 && StG >= 2 && StM >= 2 && PrM >= 2 && PrG >= 2 && RuM >= 2 && RuG >= 2)
                {

                } else{
                    SingleMove(e, -1, newState);
                    DuoMove(e, -1, newState);
                }

            }
            else if (e == 3)
            {
                SingleMove(e, 1, newState);
                DuoMove(e, 1, newState);

                if (ThG >= 3 && ThM >= 3 && PoG >= 3 && PoM >= 3 && StG >= 3 && StM >= 3 && PrM >= 3 && PrG >= 3 && RuM >= 3 && RuG >= 3)
                {

                }
                else
                {
                    SingleMove(e, -1, newState);
                    DuoMove(e, -1, newState);
                }

            }
            else if (e == 4) // only down;
            {
                SingleMove(4, -1, newState);
                DuoMove(4, -1, newState);
            }
            return newState;
        }

        private void DuoMove(int floor, int d, List<RealState> newStates)
        {
            if (ThM == floor && ThG == floor)
                newStates.Add(new RealState(floor + d, ThM + d, ThG + d, PoM, PoG, StM, StG, PrM, PrG, RuM, RuG));
            if (ThM == floor && PoM == floor)
                newStates.Add(new RealState(floor + d, ThM + d, ThG, PoM + d, PoG, StM, StG, PrM, PrG, RuM, RuG));
            if (ThM == floor && PoG == floor)
                newStates.Add(new RealState(floor + d, ThM + d, ThG, PoM, PoG + d, StM, StG, PrM, PrG, RuM, RuG));
            if (ThM == floor && StM == floor)
                newStates.Add(new RealState(floor + d, ThM + d, ThG, PoM, PoG, StM + d, StG, PrM, PrG, RuM, RuG));
            if (ThM == floor && StG == floor)
                newStates.Add(new RealState(floor + d, ThM + d, ThG, PoM, PoG, StM, StG + d, PrM, PrG, RuM, RuG));
            if (ThM == floor && PrM == floor)
                newStates.Add(new RealState(floor + d, ThM + d, ThG, PoM, PoG, StM, StG, PrM + d, PrG, RuM, RuG));
            if (ThM == floor && PrG== floor)
                newStates.Add(new RealState(floor + d, ThM + d, ThG, PoM, PoG, StM, StG, PrM, PrG + d, RuM, RuG));
            if (ThM == floor && RuM == floor)
                newStates.Add(new RealState(floor + d, ThM + d, ThG, PoM, PoG, StM, StG, PrM, PrG, RuM + d, RuG));
            if (ThM == floor && RuG == floor)
                newStates.Add(new RealState(floor + d, ThM + d, ThG, PoM, PoG, StM, StG, PrM, PrG, RuM, RuG + d));


            if (ThG == floor && PoM == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG + d, PoM + d, PoG, StM, StG, PrM, PrG, RuM, RuG));
            if (ThG == floor && PoG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG + d, PoM, PoG + d, StM, StG, PrM, PrG, RuM, RuG));
            if (ThG == floor && StM == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG + d, PoM, PoG, StM + d, StG, PrM, PrG, RuM, RuG));
            if (ThG == floor && StG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG + d, PoM, PoG, StM, StG + d, PrM, PrG, RuM, RuG));
            if (ThG == floor && PrM == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG + d, PoM, PoG, StM, StG, PrM + d, PrG, RuM, RuG));
            if (ThG == floor && PrG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG + d, PoM, PoG, StM, StG, PrM, PrG + d, RuM, RuG));
            if (ThG == floor && RuM == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG + d, PoM, PoG, StM, StG, PrM, PrG, RuM + d, RuG));
            if (ThG == floor && RuG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG + d, PoM, PoG, StM, StG, PrM, PrG, RuM, RuG + d));

            if (PoM == floor && PoG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM + d, PoG + d, StM, StG, PrM, PrG, RuM, RuG));
            if (PoM == floor && StM == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM + d, PoG, StM + d, StG, PrM, PrG, RuM, RuG));
            if (PoM == floor && StG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM + d, PoG, StM, StG + d, PrM, PrG, RuM, RuG));
            if (PoM == floor && PrM == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM + d, PoG, StM, StG, PrM + d, PrG, RuM, RuG));
            if (PoM == floor && PrG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM + d, PoG, StM, StG, PrM, PrG + d, RuM, RuG));
            if (PoM == floor && RuM == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM + d, PoG, StM, StG, PrM, PrG, RuM + d, RuG));
            if (PoM == floor && RuG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM + d, PoG, StM, StG, PrM, PrG, RuM, RuG + d));

            if (PoG == floor && StM == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG + d, StM + d, StG, PrM, PrG, RuM, RuG));
            if (PoG == floor && StG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG + d, StM, StG + d, PrM, PrG, RuM, RuG));
            if (PoG == floor && PrM == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG + d, StM, StG, PrM + d, PrG, RuM, RuG));
            if (PoG == floor && PrG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG + d, StM, StG, PrM, PrG + d, RuM, RuG));
            if (PoG == floor && RuM == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG + d, StM, StG, PrM, PrG, RuM + d, RuG));
            if (PoG == floor && RuG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG + d, StM, StG, PrM, PrG, RuM, RuG + d));

            if (StM == floor && StG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG, StM + d, StG + d, PrM, PrG, RuM, RuG));
            if (StM == floor && PrM == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG, StM + d, StG, PrM + d, PrG, RuM, RuG));
            if (StM == floor && PrG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG, StM + d, StG, PrM, PrG + d, RuM, RuG));
            if (StM == floor && RuM == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG, StM + d, StG, PrM, PrG, RuM + d, RuG));
            if (StM == floor && RuG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG, StM + d, StG, PrM, PrG, RuM, RuG + d));

            if (StG == floor && PrM == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG, StM, StG + d, PrM + d, PrG, RuM, RuG));
            if (StG == floor && PrG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG, StM, StG + d, PrM, PrG + d, RuM, RuG));
            if (StG == floor && RuM == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG, StM, StG + d, PrM, PrG, RuM + d, RuG));
            if (StG == floor && RuG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG, StM, StG + d, PrM, PrG, RuM, RuG + d));

            if (PrM == floor && PrG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG, StM, StG, PrM + d, PrG + d, RuM, RuG));
            if (PrM == floor && RuM == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG, StM, StG, PrM + d, PrG, RuM + d, RuG));
            if (PrM == floor && RuG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG, StM, StG, PrM + d, PrG, RuM, RuG + d));

            if (PrG == floor && RuM == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG, StM, StG, PrM, PrG + d, RuM + d, RuG));
            if (PrG == floor && RuG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG, StM, StG, PrM, PrG + d, RuM, RuG + d));

            if (RuM == floor && RuG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG, StM, StG, PrM, PrG , RuM+ d, RuG + d));
        }
        private void SingleMove(int floor, int d, List<RealState> newStates)
        {
            if (ThM == floor)
                newStates.Add(new RealState(floor + d, ThM + d, ThG, PoM, PoG, StM, StG, PrM, PrG, RuM, RuG));
            if (ThG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG + d, PoM, PoG, StM, StG, PrM, PrG, RuM, RuG));

            if (PoM == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM + d, PoG, StM, StG, PrM, PrG, RuM, RuG));
            if (PoG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG + d, StM, StG, PrM, PrG, RuM, RuG));

            if (StM == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG, StM + d, StG, PrM, PrG, RuM, RuG));
            if (StG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG, StM, StG + d, PrM, PrG, RuM, RuG));

            if (PrM == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG, StM, StG, PrM + d, PrG, RuM, RuG));
            if (PrG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG, StM, StG, PrM, PrG + d, RuM, RuG));

            if (RuM == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG, StM, StG, PrM, PrG, RuM + d, RuG));
            if (RuG == floor)
                newStates.Add(new RealState(floor + d, ThM, ThG, PoM, PoG, StM, StG, PrM, PrG, RuM, RuG + d));

        }

        public bool Equals(RealState other)
        {

            List<Tuple<int, int>> pairs = new List<Tuple<int, int>>();
            pairs.Add(new Tuple<int, int>(ThM, ThG));
            pairs.Add(new Tuple<int, int>(PoM, PoG));
            pairs.Add(new Tuple<int, int>(StM, StG));
            pairs.Add(new Tuple<int, int>(PrM, PrG));
            pairs.Add(new Tuple<int, int>(RuM, RuG));


            List<Tuple<int, int>> otherPirs = new List<Tuple<int, int>>();
            pairs.Add(new Tuple<int, int>(other.ThM, other.ThG));
            pairs.Add(new Tuple<int, int>(other.PoM, other.PoG));
            pairs.Add(new Tuple<int, int>(other.StM, other.StG));
            pairs.Add(new Tuple<int, int>(other.PrM, other.PrG));
            pairs.Add(new Tuple<int, int>(other.RuM, other.RuG));

            return Enumerable.SequenceEqual(pairs.OrderBy(t=>t), otherPirs.OrderBy(t=>t));
        /*
            return e == other.e &&
                ThM == other.ThM &&
                ThG == other.ThG &&
                PoM == other.PoM &&
                PoG == other.PoG &&
                StM == other.StM &&
                StG == other.StG &&
                PrM == other.PrM &&
                PrG == other.PrG &&
                RuM == other.RuM &&
                RuG == other.RuG;
             */
        }
    }


    public class ExampleState : IState, IEquatable<IState>
    {
        public readonly int e;
        public readonly int HM;
        public readonly int LM;
        public readonly int HG;
        public readonly int LG;

        public ExampleState(int e, int HM, int LM, int HG, int LG)
        {
            this.e = e; this.HM = HM; this.LM = LM; this.HG = HG; this.LG = LG;
        }

        public bool isValid()
        {
            // M types must have G if other G are present
            if (HM == LG)
                return HM == HG;

            if (LM == HG)
                return LM == LG;

            return true;
        }

        public bool isDone()
        {
            return HG == 4 && HM == 4 && LG == 4 && LM == 4;
        }

        public void print()
        {
            System.Console.SetCursorPosition(0, 0);
            System.Console.WriteLine("F4 {0} {1} {2} {3} {4}", e == 4 ? "E" : ".", HG == 4 ? "HG" : ". ", HM == 4 ? "HM" : ". ", LG == 4 ? "LG" : ". ", LM == 4 ? "LM" : ". ");
            System.Console.WriteLine("F3 {0} {1} {2} {3} {4}", e == 3 ? "E" : ".", HG == 3 ? "HG" : ". ", HM == 3 ? "HM" : ". ", LG == 3 ? "LG" : ". ", LM == 3 ? "LM" : ". ");
            System.Console.WriteLine("F2 {0} {1} {2} {3} {4}", e == 2 ? "E" : ".", HG == 2 ? "HG" : ". ", HM == 2 ? "HM" : ". ", LG == 2 ? "LG" : ". ", LM == 2 ? "LM" : ". ");
            System.Console.WriteLine("F1 {0} {1} {2} {3} {4}", e == 1 ? "E" : ".", HG == 1 ? "HG" : ". ", HM == 1 ? "HM" : ". ", LG == 1 ? "LG" : ". ", LM == 1 ? "LM" : ". ");
            System.Console.WriteLine("Valid: {0} Done: {1}", isValid(), isDone());
        }

        /// <summary>
        /// Generates all possible new States
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IState> GenerateNewStates()
        {
            var newState = new List<ExampleState>();
            if (e == 1)  // only up
            {
                SingleMove(1, 1, newState);
                DuoMove(1, 1, newState);
            }
            else if (e == 4) // only down;
            {
                SingleMove(4, -1, newState);
                DuoMove(4, -1, newState);
            }
            else // up and down
            {
                SingleMove(e, 1, newState);
                DuoMove(e, 1, newState);
                SingleMove(e, -1, newState);
                DuoMove(e, -1, newState);
            }
            return newState;
        }

        private void DuoMove(int floor, int d, List<ExampleState> newState)
        {
            if (HM == floor && LM == floor)
                newState.Add(new ExampleState(e + d, HM + d, LM + d, HG, LG));
            if (HM == floor && HG == floor)
                newState.Add(new ExampleState(e + d, HM + d, LM, HG + d, LG));
            if (HM == floor && LG == floor)
                newState.Add(new ExampleState(e + d, HM + d, LM, HG, LG + d));
            if (LM == floor && HG == floor)
                newState.Add(new ExampleState(e + d, HM, LM + d, HG + d, LG));
            if (LM == floor && LG == floor)
                newState.Add(new ExampleState(e + d, HM, LM + d, HG, LG + d));
            if (HG == floor && LG == floor)
                newState.Add(new ExampleState(e + d, HM, LM, HG + d, LG + d));
        }

        private void SingleMove(int floor, int d, List<ExampleState> newState)
        {
            if (HM == floor)
                newState.Add(new ExampleState(e + d, HM + d, LM, HG, LG));
            if (LM == floor)
                newState.Add(new ExampleState(e + d, HM, LM + d, HG, LG));
            if (HG == floor)
                newState.Add(new ExampleState(e + d, HM, LM, HG + d, LG));
            if (LG == floor)
                newState.Add(new ExampleState(e + d, HM, LM, HG, LG + d));
        }

        public bool Equals(IState o)
        {
            var other = o as ExampleState;
            return (
                e == other.e &&
                HM == other.HM &&
                HG == other.HG &&
                LG == other.LG &&
                LM == other.LM);
        }
    }


}

