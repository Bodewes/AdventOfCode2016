using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            var PC = new Computer(Input.Real);
            PC.Run();
            Console.ReadLine();
        }
    }


    public class Computer
    {

        int A = 0;
        int B = 0;
        int C = 1;
        int D = 0;

        int pc = 0;

        List<string[]> prog;

        public Computer(string lines)
        {
            prog = lines.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Split(' ')).ToList();

        }

        public void print()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("PC{4} => A: {0}, B: {1}, C: {2}, D: {3}", A, B, C, D, pc);
        }

        public void Run()
        {


            Optimize();
            printProgram();
            Console.ReadLine();
            while (pc < prog.Count())
            {
                Execute(prog[pc]);
                print();
                printProgram();
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

            }
        }

        public void printProgram()
        {
            Console.SetCursorPosition(0,2);
            Console.ForegroundColor = ConsoleColor.DarkRed;

            for(var i = 0; i< prog.Count(); i++){
                var op = prog[i];
                if (i == pc)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("{0} {1} {2}", op[0], op.Length>1?op[1]:"", op.Length>2?op[2]:"");
            }
            
        }

        private void Execute(string[] op)
        {
            switch (op[0])
            {
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
                        pc += int.Parse(op[2]);
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
                    WriteReg(op[2],sum );
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
        public const string Test = @"cpy 41 a
inc a
inc a
dec a
jnz a 2
dec a";

        public const string Real = @"cpy 1 a
cpy 1 b
cpy 26 d
jnz c 2
jnz 1 5
cpy 7 c
inc d
dec c
jnz c -2
cpy a c
inc a
dec b
jnz b -2
cpy c b
dec d
jnz d -6
cpy 19 c
cpy 11 d
inc a
dec d
jnz d -2
dec c
jnz c -5";

    }
}
