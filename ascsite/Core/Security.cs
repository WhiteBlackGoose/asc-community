using processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ascsite.Core
{
    public class Security
    {
        private static List<string> ForbiddenWords = new List<string>(){ 
            "file",
            "sql",
            "open",
            "\n",
            "\\n",
            "system",
            "import",
            "eval",
            "_",
            "vars",
            "Symbol",
            "socket",
            "sympy",
        };

        public static bool ExprSecure(string expr)
        {
            expr = expr.ToLower();
            foreach (string word in ForbiddenWords) //общаемся без мата
                if (expr.Contains(word))
                    return false;
            if (expr.Count() > Const.LIMIT_REQLEN)
                return false;
            return true;
        }
    }
}
