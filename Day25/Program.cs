using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day25
{
    class Program
    {
        static void Main(string[] args)
        {
            var PCpart1 = new Computer(Input.part1);
            PCpart1.A = 158;
            PCpart1.Run();
            Console.ReadLine();
        }
    }

    public class Computer
    {

        public int A = 0;
        public int B = 0;
        public int C = 0;
        public int D = 0;

        public string Display = "";

        int pc = 0;

        List<string[]> prog;

        public Computer(string lines)
        {
            prog = lines.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Split(' ')).ToList();

        }

        public void print()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("PC{4} => A: {0}, B: {1}, C: {2}, D: {3}             ", A, B, C, D, pc);
            Console.WriteLine("Display: {0}", Display);
        }

        public void Run()
        {


            Optimize();
            print();
            printProgram();
            Console.ReadLine();
            while (pc < prog.Count())
            {
                Execute(prog[pc]);
                print();

                //printProgram();
                //Console.ReadLine();
            }
            print();
        }

        /*
         * inc a
         * dec b
         * jnz b -2
         * ==> add b to a, clear b
         * add b a // add b to a store in a
         * cpy 0 b
         * nop
         */


        private void Optimize()
        {
            for (int i = 0; i < prog.Count() - 2; i++)
            {
                if (prog[i][0] == "inc" &&
                    prog[i + 1][0] == "dec" &&
                    prog[i + 2][0] == "jnz")
                {
                    var src = prog[i + 1][1];
                    var dst = prog[i][1];
                    prog[i] = new string[] { "add", src, dst };
                    prog[i + 1] = new string[] { "cpy", "0", src };
                    prog[i + 2] = new string[] { "nop" };
                }

                if (prog[i][0] == "jnz" && prog[i][1] == "0" && prog[i][2] == "0")
                {
                    prog[i] = new string[] { "nop" };
                }

            }
        }

        public void printProgram()
        {
            Console.SetCursorPosition(0, 2);
            Console.ForegroundColor = ConsoleColor.DarkRed;

            for (var i = 0; i < prog.Count(); i++)
            {
                var op = prog[i];
                if (i == pc)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("{0} {1} {2}", op[0], op.Length > 1 ? op[1] : "", op.Length > 2 ? op[2] : "");
            }

        }

        private void Execute(string[] op)
        {
            switch (op[0])
            {
                case "tgl":
                    // inc --> dec
                    // dec --> inc
                    // tgl --> inc
                    // cpy x y --> jnz x y
                    // jnz x y --> cpy x y
                    var delta = ReadReg(op[1]);
                    var instrc = pc + delta;
                    if (instrc >= 0 && instrc < prog.Count)
                    {
                        switch (prog[instrc][0])
                        {
                            case "inc":
                                prog[instrc][0] = "dec"; break;
                            case "dec":
                                prog[instrc][0] = "inc"; break;
                            case "tgl":
                                prog[instrc][0] = "inc"; break;
                            case "cpy":
                                prog[instrc][0] = "jnz"; break;
                            case "jnz":
                                prog[instrc][0] = "cpy"; break;
                        }
                    }
                    pc++;

                    break;
                case "cpy":
                    switch (op[1])
                    {
                        case "a":
                        case "b":
                        case "c":
                        case "d":
                            Copy(op[1], op[2]); // reg to reg
                            break;
                        default:
                            Copy(int.Parse(op[1]), op[2]);
                            break;
                    }
                    pc++;
                    break;
                case "inc":
                    if (op[1] == "a") A++;
                    if (op[1] == "b") B++;
                    if (op[1] == "c") C++;
                    if (op[1] == "d") D++;
                    pc++;
                    break;
                case "dec":
                    if (op[1] == "a") A--;
                    if (op[1] == "b") B--;
                    if (op[1] == "c") C--;
                    if (op[1] == "d") D--;
                    pc++;
                    break;
                case "jnz":
                    if (ReadReg(op[1]) != 0)
                    {
                        pc += ReadReg(op[2]);
                    }
                    else
                    {
                        pc++;
                    }
                    break;

                case "nop": // no op
                    pc++;
                    break;
                case "add": // add a to b in b
                    var sum = ReadReg(op[1]) + ReadReg(op[2]);
                    WriteReg(op[2], sum);
                    pc++;
                    break;
                case "out":
                    Display += ReadReg(op[1]);
                    pc++;
                    break;


            }
        }


        private void Copy(string src, string dest)
        {
            int val = 0; ;
            if (src == "a") val = A;
            if (src == "b") val = B;
            if (src == "c") val = C;
            if (src == "d") val = D;
            Copy(val, dest);
        }
        private void Copy(int src, string dest)
        {
            if (dest == "a") A = src;
            if (dest == "b") B = src;
            if (dest == "c") C = src;
            if (dest == "d") D = src;
        }

        private int ReadReg(string reg)
        {
            if (reg == "a") return A;
            if (reg == "b") return B;
            if (reg == "c") return C;
            if (reg == "d") return D;

            return int.Parse(reg);
        }

        private void WriteReg(string reg, int value)
        {
            if (reg == "a") A = value;
            if (reg == "b") B = value;
            if (reg == "c") C = value;
            if (reg == "d") D = value;
        }
    }

    public static class Input
    {

        public const string part1 = @"cpy a d
cpy 4 c
cpy 643 b
inc d
dec b
jnz b -2
dec c
jnz c -5
cpy d a
jnz 0 0
cpy a b
cpy 0 a
cpy 2 c
jnz b 2
jnz 1 6
dec b
dec c
jnz c -4
inc a
jnz 1 -7
cpy 2 b
jnz c 2
jnz 1 4
dec b
dec c
jnz 1 -4
jnz 0 0
out b
jnz a -19
jnz 1 -21";
    }

}
