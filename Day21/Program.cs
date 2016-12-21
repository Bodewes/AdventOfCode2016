using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day21
{
    class Program
    {
        static void Main(string[] args)
        {

            var s = new Scrambler();
            var example = s.Run("abcde", Input.Example);
            Console.WriteLine(">> "+example);
            Console.WriteLine("<< "+s.Run(example, Input.Example, true));

            var part1 = s.Run("abcdefgh", Input.Part1);
            Console.WriteLine(">> " + part1);
            Console.WriteLine("<< " + s.Run(part1, Input.Part1, true));

            // Hmm reversed is bug ridden
            Console.WriteLine(s.Run("fbgdceah", Input.Part1), true);

            // Solve it brute force...
            s.Part2("fbgdceah");
            Console.ReadLine();
        }
    }

    public class Scrambler
    {

        public void Part2(string input)
        {
            generate("", "abcdefgh", input);
        }
        private void generate(string x, string rest, string test)
        {
            if (rest.Length == 0)
            {
                if ( Run(x, Input.Part1) == test){
                    Console.WriteLine("FOUND: "+x);
                }
            }
            else
            {
                for (int i = 0; i < rest.Length; i++)
                {
                    generate(x + rest[i], rest.Remove(i, 1), test);
                }
            }
        }


        public string Run(string input, string rules, bool backwards = false)
        {

            var ruleSet = rules.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (backwards)
                ruleSet = ruleSet.Reverse().ToArray();

            foreach (var rule in ruleSet)
            {
                var param = rule.Split(' ');
                switch (param[0])
                {
                    case "swap":
                        if (param[1] == "position")
                            input = Swap(input, int.Parse(param[2]), int.Parse(param[5]));
                        if (param[1] == "letter")
                            input = Swap(input, param[2][0], param[5][0]);
                        break;
                    case "rotate":

                        if (param[1] == "right")
                            if (backwards)
                                input = RotateLeft(input, int.Parse(param[2]));
                            else
                                input = RotateRight(input, int.Parse(param[2]));
                        if (param[1] == "left")
                            if (backwards)
                                input = RotateRight(input, int.Parse(param[2]));
                            else
                                input = RotateLeft(input, int.Parse(param[2]));
                        if (param[1] == "based")
                        {
                            if (backwards)
                            {
                                if (input.Length == 8)
                                {
                                    switch (input.IndexOf(param[6]))
                                    {
                                        case 0: input = RotateLeft(input, 1); break;
                                        case 1: input = RotateLeft(input, 1); break;
                                        case 2: input = RotateLeft(input, 6); break;
                                        case 3: input = RotateLeft(input, 2); break;
                                        case 4: input = RotateLeft(input, 7); break;
                                        case 5: input = RotateLeft(input, 3); break;
                                        case 6: input = RotateLeft(input, 0); break;
                                        case 7: input = RotateLeft(input, 4); break;
                                    }

                                }
                                else
                                {
                                    switch (input.IndexOf(param[6]))
                                    {
                                        case 0: input = RotateLeft(input, 1); break;
                                        case 1: input = RotateLeft(input, 1); break;
                                        case 2: input = RotateLeft(input, 4); break;
                                        case 3: input = RotateLeft(input, 2); break;
                                        case 4: input = RotateLeft(input, 7); break;
                                    }
                                }

                                    
                            }
                            else
                            {
                                var i = input.IndexOf(param[6]);
                                i = i >= 4 ? i += 1 : i;
                                input = RotateRight(input, i + 1);
                            }
                        }

                        break;
                    case "reverse":
                        input = Reverse(input, int.Parse(param[2]), int.Parse(param[4]));
                        break;
                    case "move":
                        if (backwards)
                            input = Move(input, int.Parse(param[5]), int.Parse(param[2]));
                        else
                            input = Move(input, int.Parse(param[2]), int.Parse(param[5]));
                        break;

                }
               // Console.WriteLine(input);

            }
            return input;

        }

        private string Move(string input, int p1, int p2)
        {
            //Console.WriteLine("Move {0} {1}", p1, p2);
            char c = input[p1];
            var result = input.Substring(0, p1) + input.Substring(p1 + 1);
            return result.Substring(0, p2) + c + result.Substring(p2);
        }

        private string RotateLeft(string input, int p)
        {
           // Console.WriteLine("Left {0}", p);
            p = p % input.Length;
            return input.Substring(p, input.Length - p) + input.Substring(0, p);
        }

        private string RotateRight(string input, int p)
        {
            //Console.WriteLine("Right {0}", p);
            p = p % input.Length;
            return input.Substring(input.Length - p, p) + input.Substring(0, input.Length - p);
        }

        private string Reverse(string input, int p1, int p2)
        {
            //Console.WriteLine("Reverse {0} {1}", p1, p2);
            return input.Substring(0, p1) + string.Join("", input.Substring(p1, p2 - p1 + 1).Reverse()) + input.Substring(p2 + 1);
        }

        private string Swap(string input, char p1, char p2)
        {
            //Console.WriteLine("Swap {0} {1}", p1, p2);
            return Swap(input, input.IndexOf(p1), input.IndexOf(p2));
        }

        private string Swap(string input, int p1, int p2)
        {
            //Console.WriteLine("Swap {0} {1}", p1, p2);
            int min = Math.Min(p1, p2);
            int max = Math.Max(p1, p2);
            return input.Substring(0, min) + input[max] + input.Substring(min + 1, max - min - 1) + input[min] + input.Substring(max + 1);
        }
    }

    public static class Input
    {
        public const string Example =
@"swap position 4 with position 0
swap letter d with letter b
reverse positions 0 through 4
rotate left 1 step
move position 1 to position 4
move position 3 to position 0
rotate based on position of letter b
rotate based on position of letter d
";
        public const string Part1 =
            @"swap letter a with letter d
move position 6 to position 4
move position 5 to position 1
swap letter h with letter e
rotate based on position of letter a
move position 6 to position 2
reverse positions 0 through 1
rotate based on position of letter h
rotate based on position of letter g
rotate based on position of letter h
reverse positions 4 through 7
swap letter a with letter f
swap position 2 with position 7
move position 7 to position 5
reverse positions 0 through 5
rotate based on position of letter f
rotate right 4 steps
swap position 3 with position 0
move position 1 to position 2
reverse positions 4 through 6
swap position 3 with position 5
swap letter a with letter c
swap position 5 with position 2
swap position 7 with position 2
move position 2 to position 5
rotate based on position of letter h
rotate right 2 steps
swap position 3 with position 4
move position 0 to position 1
reverse positions 1 through 7
reverse positions 1 through 4
rotate based on position of letter b
rotate right 7 steps
rotate left 0 steps
swap position 6 with position 1
reverse positions 1 through 3
reverse positions 0 through 3
move position 0 to position 4
rotate based on position of letter f
reverse positions 0 through 7
reverse positions 0 through 1
move position 1 to position 7
move position 7 to position 6
rotate based on position of letter b
reverse positions 3 through 5
reverse positions 0 through 3
swap letter c with letter h
reverse positions 3 through 5
swap position 3 with position 6
swap letter d with letter g
move position 5 to position 6
swap position 6 with position 2
rotate right 5 steps
swap letter e with letter g
rotate based on position of letter e
rotate based on position of letter c
swap letter g with letter e
rotate based on position of letter b
rotate based on position of letter b
swap position 0 with position 2
move position 6 to position 0
move position 5 to position 0
rotate left 2 steps
move position 0 to position 5
rotate left 7 steps
swap letter b with letter g
rotate based on position of letter d
swap letter h with letter e
swap letter d with letter c
rotate based on position of letter f
move position 5 to position 0
rotate left 5 steps
swap position 0 with position 7
swap position 0 with position 3
rotate left 4 steps
rotate left 1 step
rotate right 6 steps
swap position 0 with position 1
reverse positions 4 through 6
reverse positions 4 through 6
move position 6 to position 3
move position 7 to position 4
rotate right 4 steps
swap letter g with letter d
swap letter c with letter e
swap letter e with letter h
rotate right 5 steps
rotate based on position of letter g
rotate based on position of letter g
rotate left 3 steps
swap letter h with letter g
reverse positions 0 through 4
rotate right 4 steps
move position 6 to position 4
rotate based on position of letter c
swap position 2 with position 6
swap position 7 with position 2
rotate right 1 step
swap position 3 with position 1
swap position 4 with position 6";

    }
}
