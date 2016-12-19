using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day19
{
    class Program
    {
        static void Main(string[] args)
        {
            var we = new WhiteElephant();
            Console.WriteLine(we.part1(5)+1);
            Console.WriteLine(we.part1(13)+1);
            Console.WriteLine(we.part1(23)+1);
            Console.WriteLine(we.part1(3001330) + 1);

            Console.WriteLine(we.part2(5));
            Console.WriteLine(we.part2(3001330));
            Console.ReadLine();


        }
    }

    public class WhiteElephant
    {
        
        public int part1(int elves)
        {
            int count = 1;
            int result = 0;
            while (count < elves)
            {
                result = elves % count;
                count = count << 1;
            }
            return result*2;
        }


        public int part2(int size)
        {
            Elf root = new Elf() { Id = 1 };
            Elf target = null;
            Elf current = root;
            // fill elves
            for (int i = 1; i < size; i++){
                current.Next = new Elf() { Id = i+1, Prev = current };
                current = current.Next;
                if (i == size / 2) target = current;
            }
            current.Next = root;
            root.Prev = current;

            current = root;

            int count = size;
            while (current.Next != current)
            {
                // remove target
                target.Prev.Next = target.Next;
                target.Next.Prev = target.Prev;
                target = count % 2 == 1 ? target.Next.Next : target.Next;
                count--;

                current = current.Next;

            }


            return current.Id;
        }

    }

    public class Elf
    {
        public Elf Next;
        public Elf Prev;
        public int Id;

    }
}
