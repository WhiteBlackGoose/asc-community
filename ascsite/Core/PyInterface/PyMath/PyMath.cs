using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ascsite.Core.PyInterface.PyMath
{
    public class PyMath
    {
        private PyInterface pyInterface;

        public PyMath()
        {
            pyInterface = new PyInterface();
            pyInterface.Import("sympy");
            pyInterface.ImportAllFrom("sympy");
        }

        public string Derivative(string expr, string variable, IEnumerable<string> tokens)
        {
            string code = "";
            expr = expr.Replace("^", "**");
            foreach(var token in tokens)
            {
                code += token + " = sympy.Symbol('" + token + "')\n";
            }
            code += "print(sympy.diff(" + expr + ", " + variable + "))";
            return pyInterface.Exec(code).Replace("**", "^");
        }
    }
}
