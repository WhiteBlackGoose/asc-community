using processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ascsite.Core.PyInterface.PyMath
{
    public class PyMath
    {
        private PyInterface pyInterface;
        public bool latex;

        public void Destroy()
        {
            pyInterface.ProcessStop();
        }

        public PyMath()
        {
            pyInterface = new PyInterface(Const.PATH_PYOUT);            
            this.latex = false;
        }

        public void TokensAdd(PyInterface pyInterface, IEnumerable<string> tokens)
        {
            string code = "";
            foreach (var token in tokens)
            {
                code += "S" + token + "\n";
            }
            pyInterface.Run(code);
        }
        private string PrintExpr(string sympyCommand, bool multiAns=false)
        {
            string res = Const.PYINTERFACE_EVALREQ.ToString();
            if (!latex)
                res += sympyCommand;
            else
            {
                if (!multiAns)
                    res += "sympy.latex(" + sympyCommand + ")";
                else
                    res += "\"@\".join([str(sympy.latex(i)) for i in " + sympyCommand + "])";
            }
            return res;
        }

        private string LatexOnNeed(string expr)
        {
            if (latex)
                return @"\[" + expr + @"\]";
            else
                return expr;
        }

        public string ExprPrepare(string expr)
        {
            return expr.Replace("^", "**");
        }

        private bool IsFloat(string str)
        {
            return !str.Contains(' ') && Regex.Match(str, @"^[+-]?\d+\.\d+$").Success;
        }

        private string NumZeroCut(string num)
        {
            if (num.IndexOf(".") != -1)
            {
                int lastIdx = num.Length - 1;
                while (lastIdx >= 0 && num[lastIdx] == '0')
                    lastIdx--;
                if (num[lastIdx] == '.') lastIdx--;
                num = num.Substring(0, lastIdx + 1);
            }
            return num;
        }

        private string ExprPostProc(string expr)
        {
            if (IsFloat(expr))
                return NumZeroCut(expr);
            else
                return expr;
        }

        public string Derivative(string expr, string variable, IEnumerable<string> tokens)
        {
            TokensAdd(pyInterface, tokens);
            expr = ExprPrepare(expr);
            string res = pyInterface.Run(PrintExpr("sympy.diff(" + expr + ", " + variable + ")"));
            return LatexOnNeed(ExprPostProc(res).Replace("**", "^"));
        }

        public string Integral(string expr, string variable, IEnumerable<string> tokens)
        {
            TokensAdd(pyInterface, tokens);
            expr = ExprPrepare(expr);
            string res = pyInterface.Run(PrintExpr("sympy.integrate(" + expr + ", " + variable + ")"));
            return LatexOnNeed(ExprPostProc(res).Replace("**", "^"));
        }
        
        public static bool IsLetter(char s)
        {
            string A = "qwertyuiopasdfghjklzxcvbnm";
            return (A + A.ToUpper()).Contains(s);
        }

        public string Simplify(string expr, IEnumerable<string> tokens, bool appr = true)
        {
            TokensAdd(pyInterface, tokens);
            expr = ExprPrepare(expr);
            string analyt = ExprPostProc(pyInterface.Run(PrintExpr("sympy.simplify(" + expr + ")")).Replace("**", "^"));
            string approx = ExprPostProc(pyInterface.Run(PrintExpr("sympy.simplify(expand(" + expr + ")).evalf()")).Replace("**", "^"));
            string res = "";
            if (analyt != approx && appr)
            {
                res = Const.TITLE_ANALYTICAL_SIMPLIFY + LatexOnNeed(analyt.ToString()) + "\n" + Const.TITLE_APPROXIMATE_SIMPLIFY + LatexOnNeed(approx.ToString());
            }
            else
                res = LatexOnNeed(analyt.ToString());
            return res;
        }

        public string Solve(string expr, string variable, IEnumerable<string> tokens)
        {
            TokensAdd(pyInterface, tokens);
            expr = ExprPrepare(expr);
            expr = PrintExpr("sympy.solve(" + expr + ", " + variable + ")", multiAns: true);
            string pyres = pyInterface.Run(expr).Replace("**", "^");
            List<string> roots = PyInterface.FromPyList(pyres, '@');
            List<string> result = new List<string>();
            foreach (var root in roots)
                    result.Add(LatexOnNeed(variable + " = " + ExprPostProc(root)));
            return string.Join("\n", result.ToArray());
        }
    }
}
