using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ascsite.Core
{
    public class Functions
    {
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
    }
}
