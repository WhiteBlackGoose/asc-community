using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace ascsite.Core
{
    public class Functions
    {
        public static string FillStringNa(string s)
        {
            return s ?? "";
        }

        public static long Now()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public static List<string> MakeUnique(List<string> v)
        {
            var unique_items = new HashSet<string>(v);
            var result = new List<string>();
            foreach (string s in unique_items)
                result.Add(s);
            return result;
        }

        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
