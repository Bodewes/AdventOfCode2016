using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {

            // Part 1
            var part1 = new OneTimePad("cuanljph");
            part1.generateHashes(64);
            Console.ReadLine();

            // Part 2
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var part2 = new OneTimePad("cuanljph", 2016);
            part2.generateHashes(64);
            sw.Stop();
            Console.WriteLine("Time : {0}", sw.Elapsed);
            Console.ReadLine();
        }
    }

    public class OneTimePad
    {
        string seed;
        int keystrech = 0;
        Item[] cache = new Item[100000];
        MD5 md5 = MD5.Create();

        public OneTimePad(string s)
        {
            seed = s;
        }

        public OneTimePad(string s, int k):
            this(s)
        {
            keystrech = k;
        }

        public void generateHashes(int limit)
        {
            // first 1000
            for (int i = 0; i < 1000; i++)
            {
                createItem(i, seed);
                //Console.WriteLine("[" + i + "]>  " + cache[i].ToString());
            }

            int count = 0;
            int index = 0;
            while (count < limit){
                if (isValid(index))
                {
                    count++;
                    Console.WriteLine("["+count+"]> "+index + ">> " + cache[index].ToString());
                }
                index++;
            }
            
        }

        public bool isValid(int index)
        {

            for (int i = index + 1; i < index + 1 + 1000; i++)
            {
                if (cache[i] == null)
                    createItem(i, seed);
                if (cache[i]._5.Contains(cache[index]._3))
                    return true;
            }
            return false;
        }

        private string makeHash(string input)
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            return string.Concat(hash.Select(x => x.ToString("x2")));
            //StringBuilder sb = new StringBuilder();
            //foreach (byte b in hash)
            //    sb.Append(b.ToString("x2"));
            //return sb.ToString();
        }

        public void createItem(int index, string seed){
            string hash = makeHash(seed + index);
            
            //used in part 2
            if (keystrech > 0)
            {
                hash = longHash(hash);
                //Console.WriteLine("S: {0} => {1}", index, hash);
                //Console.ReadLine();
            }

            var item = new Item()
            {
                Hash = hash,
                _3 = '\0'
            };
            // find 3 string.
            for (int i = 0; i < hash.Length - 2; i++)
            {
                if (hash[i] == hash[i + 1] && hash[i] == hash[i + 2])
                {
                    item._3 = hash[i];
                    break;
                }
            }
            // find 5 strings.
            var re = Regex.Matches(hash,"(.)\\1{4,4}");
            item._5 = re.Cast<Match>().Select(x => x.Groups[1].Value[0]).ToList();
            cache[index] =item;
        }

        private string longHash(string hash)
        {
            for (int k = 0; k < keystrech; k++)
            {
                hash = makeHash(hash);
            }
            return hash;
        }

    }

    public class Item{
        public string Hash { get; set; }
        public string LongHash { get; set; }
        public char _3{get;set;}  // first of 3
        public List<char> _5{get;set;} // all 5 sequences

        public override string ToString()
        {
            return Hash + " [" + _3 + "] [" + string.Join(",", _5.ToArray()) + "]";
        }
    }

}
