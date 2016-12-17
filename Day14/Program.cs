using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
        volatile Item[] cache = new Item[25000];
        

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
            //generate(1000, 0);
            List<Thread> t = new List<Thread>();
            for (int i = 0; i < 8; i++)
            {
                var start = i * 3000;
                t.Add(new Thread(() => generate(3000,start)));
                t[i].Start();
            }

            // wait on all threads.
            foreach (Thread thread in t)
                    { thread.Join(); }

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

        public void generate(int count, int start)
        {
            Console.WriteLine("Creating {0} from {1}", count, start);
            using (MD5 md5 = MD5.Create())
            {
                for (int i = 0; i < count; i++)
                {
                    createItem(i+start, seed, md5);
                }
            }
            Console.WriteLine("Done {0}", start);
        }

        public bool isValid(int index)
        {
            using (MD5 md5 = MD5.Create())
            {
                for (int i = index + 1; i < index + 1 + 1000; i++)
                {
                    if (cache[i] == null)
                        createItem(i, seed, md5);
                    if (cache[i]._5.Contains(cache[index]._3))
                        return true;
                }
            }
            return false;
        }

        private string makeHash(string input, MD5 md5)
        {

                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hash = md5.ComputeHash(inputBytes);

                //return string.Concat(hash.Select(x => x.ToString("x2")));

                //StringBuilder sb = new StringBuilder();
                //foreach (byte b in hash)
                //    sb.Append(b.ToString("x2"));
                //return sb.ToString();
                return Helper.ByteArrayToHexViaLookup32UnsafeDirect(hash);

        }

        public void createItem(int index, string seed, MD5 md5){
            string hash = makeHash(seed + index, md5);
            
            //used in part 2
            if (keystrech > 0)
            {
                hash = longHash(hash, md5);
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

        private string longHash(string hash, MD5 md5)
        {
            for (int k = 0; k < keystrech; k++)
            {
                hash = makeHash(hash, md5);
            }
            return hash;
        }



    }

    public unsafe static class Helper
    {
        private static readonly uint[] _lookup32Unsafe = CreateLookup32Unsafe();
        private static readonly uint* _lookup32UnsafeP = (uint*)GCHandle.Alloc(_lookup32Unsafe, GCHandleType.Pinned).AddrOfPinnedObject();

        private static uint[] CreateLookup32Unsafe()
        {
            var result = new uint[256];
            for (int i = 0; i < 256; i++)
            {
                string s = i.ToString("x2");
                if (BitConverter.IsLittleEndian)
                    result[i] = ((uint)s[0]) + ((uint)s[1] << 16);
                else
                    result[i] = ((uint)s[1]) + ((uint)s[0] << 16);
            }
            return result;
        }

        public static string ByteArrayToHexViaLookup32Unsafe(byte[] bytes)
        {
            var lookupP = _lookup32UnsafeP;
            var result = new char[bytes.Length * 2];
            fixed (byte* bytesP = bytes)
            fixed (char* resultP = result)
            {
                uint* resultP2 = (uint*)resultP;
                for (int i = 0; i < bytes.Length; i++)
                {
                    resultP2[i] = lookupP[bytesP[i]];
                }
            }
            return new string(result);
        }

        public static string ByteArrayToHexViaLookup32UnsafeDirect(byte[] bytes)
        {
            var lookupP = _lookup32UnsafeP;
            var result = new string((char)0, bytes.Length * 2);
            fixed (byte* bytesP = bytes)
            fixed (char* resultP = result)
            {
                uint* resultP2 = (uint*)resultP;
                for (int i = 0; i < bytes.Length; i++)
                {
                    resultP2[i] = lookupP[bytesP[i]];
                }
            }
            return result;
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
