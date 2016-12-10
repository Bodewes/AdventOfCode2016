using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Done: "+new Password().generatePassword2("abc"));
            System.Console.WriteLine("Done: " + new Password().generatePassword2("ojvtpuvg"));

            System.Console.ReadLine();
        }
    }

    public class Password
    {

        public System.Security.Cryptography.MD5 md5 = null;
        public Password()
        {
            md5 = System.Security.Cryptography.MD5.Create();
        }

        public string generatePassword2(string doorID)
        {
            var count = 0;

            char[] code = { '_', '_', '_', '_', '_', '_', '_', '_' };
            for (var i = 0; i < 8; i++)
            {
                while (true)
                {
                    var c = CreateMD5(doorID + count++);
                    if (c.StartsWith("00000"))
                    {
                        if (c[5] == '0' || c[5] == '1' || c[5] == '2' || c[5] == '3' || c[5] == '4' || c[5] == '5' || c[5] == '6' || c[5] == '7')
                        {
                            var pos = Convert.ToInt32(c[5] + "");
                            var r = c[6];
                            if (pos <= 8)
                            {
                                if (code[pos] == '_')
                                {
                                    code[pos] = r;
                                    break;
                                }

                            }
                        }
                    }
                }
                System.Console.WriteLine(new string(code));
            }
            return new string(code);
        }


        public string generatePassword(string doorID)
        {
            var count = 0;

            var code = "";
            for (var i = 0; i<8;i++){
                while (true)
                {
                    var c = CreateMD5(doorID + count++);
                    if (c.StartsWith("00000"))
                    {
                        code += c[5];
                        break;
                    }
                }
                System.Console.WriteLine(code);
            }
            return code;
        }



        public string CreateMD5(string input)
        {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
        }
    }

}
