using ascsite;
using ascsite.Core;
using processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AscSite.Core.AscSci.AscMath
{
    public class BoolEng
    {
        private Dictionary<string, Dictionary<string, string>> operators;
        private Dictionary<string, string> synonims;
        private string eq;

        public static int Min(List<int> arr)
        {
            int ind = arr[0];
            foreach (var a in arr)
                if (a < ind)
                    ind = a;
            return ind;
        }

        public void AddSynonim(string key, string value)
        {
            synonims[key] = value;
        }

        private void AddOps(string syntax, List<string> keys, string values)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                if (!operators.ContainsKey(syntax))
                    operators[syntax] = new Dictionary<string, string>();
                operators[syntax][keys[i]] = values[i].ToString();
            }
        }

        public void AddOperator1f(string syntax, string values)
        {
            var keys = new List<string>() { "0", "1" };
            AddOps(syntax, keys, values);
        }

        public void AddOperator2f(string syntax, string values)
        {
            var keys = new List<string>() { "00", "01", "10", "11" };
            AddOps(syntax, keys, values);
        }

        private string Simplify(string eq)
        {
            eq = eq.ToLower();
            foreach (var syn in synonims)
            {
                var key = syn.Key;
                eq = eq.Replace(key, synonims[key]);
            }

            eq = eq.Replace(" ", "");
            eq = eq.Replace("(1)", "1");
            eq = eq.Replace("(0)", "0");
            return eq;
        }
        
        public BoolEng(string eq)
        {
            operators = new Dictionary<string, Dictionary<string, string>>();
            synonims = new Dictionary<string, string>();
            AddSynonim("¬", "!");
            AddSynonim("∧", "&");
            AddSynonim("^", "&");
            AddSynonim("∨", "|");
            AddSynonim("≡", "==");
            AddSynonim("not", "!");
            AddSynonim("and", "&");
            AddSynonim("or", "|");
            AddOperator1f("!", "10");
            AddOperator2f("&", "0001");
            AddOperator2f("|", "0111");
            AddOperator2f("->", "1101");
            AddOperator2f("==", "1001");
            this.eq = Simplify(eq);
        }

        private string Plugin(string eq, Dictionary<string, string> variables)
        {
            foreach (var key in variables)
            {
                var k = key.Key;
                eq = eq.Replace(k, variables[k]);
            }

            return eq;
        }

        private string CompileOperatorS(string op, string key)
        {
            if (key.Length == 1)
                return op + key;
            else
                return key[0] + op + key[1];
        }

        private Dictionary<string, string> CompileOperator(string op)
        {
            var res = new Dictionary<string, string>();
            foreach (var key in operators[op])
                res[CompileOperatorS(op, key.Key)] = operators[op][key.Key];
            return res;
        }

        private int TakeMinPos(string s, List<string> values)
        {
            var r = new List<int>();
            foreach (var v in values)
            {
                int ind = s.IndexOf(v);
                if (ind != -1)
                    r.Add(ind);
                else
                    r.Add(s.Length + 1);
            }
            var res = -1;
            int m = s.Length;
            for (int i = 0; i < r.Count; i++)
                if (r[i] < m)
                {
                    m = r[i];
                    res = i;
                }
            return res;
        }

        private int MinArg(List<int> arr)
        {
            return arr.IndexOf(Min(arr));
        }

        private string Exec(string s, string op)
        {
            var table = CompileOperator(op);
            var keys = new List<string>();
            foreach (var pair in table)
                keys.Add(pair.Key);
            int pos = TakeMinPos(s, keys);
            while (pos != -1)
            {
                var regex = new Regex(Regex.Escape(keys[pos]));
                var newText = regex.Replace(s, table[keys[pos]], 1);
                s = Simplify(newText);
                pos = TakeMinPos(s, keys);
            }
            return s;
        }

        private string ReservedTokens()
        {
            string r = "()";
            foreach (var pair in operators)
                r += pair.Key;
            return r;
        }

        private List<string> Permutations(int l, string values)
        {
            if (l == 1)
                return new List<string>() { "0", "1" };
            var res = new List<string>();
            foreach (var v in values)
            {
                List<string> subres = Permutations(l - 1, values);
                for (int i = 0; i < subres.Count; i++)
                    //subres[i] = v + subres[i];
                    res.Add(v + subres[i]);
            }
            res.Sort();
            return res;
        }

        override public string ToString()
        {
            return "";
        }

        public string Subs(Dictionary<string, string> variables)
        {
            string exp = Simplify(eq);
            exp = Plugin(exp, variables);
            string oldexp = exp;
            while (exp.Length > 1)
            {
                foreach (var pair in operators)
                {
                    var key = pair.Key;
                    exp = Exec(exp, key);
                }
                if (exp == oldexp)
                    return exp;
                oldexp = exp;
            }
            return exp;
        }

        public List<string> GetTokens()
        {
            string eq = Simplify(this.eq);
            string reserved = ReservedTokens() + "01";
            var res = new List<string>();
            foreach (var i in eq)
                if (!reserved.Contains(i))
                    res.Add(i.ToString());
            return Functions.MakeUnique(res);
        }

        /*
         * Format of the table:
          {[0][0][0]}\n
          {[0][0][1]}\n
          ...
          { - <tr>
          } - </tr>
          [ - <td>
          ] - </td>
         */

        public string CompileTable(string val = "")
        {
            string res = "";
            List<string> tokens = GetTokens();
            tokens.Sort();
            foreach (var values in Permutations(tokens.Count, "01"))
            {
                string line = "{";
                var variables = new Dictionary<string, string>();
                for (int i = 0; i < tokens.Count; i++)
                {
                    variables[tokens[i]] = values[i].ToString();
                    line += "[ " + values[i].ToString() + " ]";
                }
                string value = Subs(variables);
                if (value.Length > 1)
                    throw new ParsingException(Const.ERMSG_BOOLENG_UNRESOLVEDSYMBOL);
                if (string.IsNullOrEmpty(val) || val == value)
                {
                    line += "[ " + value + " ]";
                    line += "}";
                    res += line + "\n";
                }
            }
            return res;
        }
    }
}
