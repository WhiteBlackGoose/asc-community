using ascsite.Core.PyInterface.PyMath;
using ascsite.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using AscSite.Core.AscSci.AscMath;

namespace ascsite.Core.AscSci.Calculator
{
    public class CalcResult
    {
        public string InterpretedAs { get; set; }
        public string Result { get; set; }
        public string SolidResult { get { return Result.Replace("\n", ", ").Replace('I', 'i'); } }

        public string ProcessedResult { get
            {
                return Result.Replace("\n", "<br>");
            } }
    }

    public class AscCalc
    {
        string rawExpression;
        public enum FieldType
        {
            DERIVATIVE,
            BOOL,
            INTEGRAL,
            SOLVE,
            SIMPLIFICATION
        }

        private bool latex;

        public AscCalc(string expr, bool latex=false)
        {
            rawExpression = expr.Trim();
            this.latex = latex;
        }

        public CalcResult Compute()
        {
            CalcResult res = new CalcResult();
            TokenList tokens = new Lexer().GenerateTokens(rawExpression);
            FieldType fieldtype = FindField(tokens);

            PyMath pymath = new PyMath(latex);
            try
            {
                if (fieldtype == FieldType.DERIVATIVE || fieldtype == FieldType.INTEGRAL || fieldtype == FieldType.SOLVE)
                {
                    // get index of `for` keyword and select variable for differential after it
                    int forPos = tokens.FindIndex(token => token == new Token(Token.TYPE.KEYWORD, Names.FOR));
                    if (forPos == -1 || forPos == tokens.Count())
                        throw new ParseException("No `" + Names.FOR + "` token was found. Tokens are: " + tokens.ToString());
                    string diffVar = tokens[forPos + 1].value;

                    // drop out everything before pipe keyword in token list
                    Func<TokenList, int> SplitIndex = list => list.FindIndex(token => token == new Token(Token.TYPE.BRACKET_OPEN, Names.PIPE)) + 1;
                    int idx = SplitIndex(tokens);
                    tokens = new TokenList(tokens.GetRange(idx, tokens.Count() - idx));

                    // add omitted ops and create string expression to pass it to PyMath
                    tokens.AddOmittedOps();
                    string derexp = string.Concat(tokens.Select(token => token.value));

                    var variables = tokens.ExtractVariables();
                    variables.Add(diffVar);
                    if (fieldtype == FieldType.DERIVATIVE)
                        res.Result = pymath.Derivative(derexp, diffVar, variables);
                    else if (fieldtype == FieldType.SOLVE)
                        res.Result = pymath.Solve(derexp, diffVar, variables);
                    else
                        res.Result = pymath.Integral(derexp, diffVar, variables);
                    res.InterpretedAs = string.Concat(tokens.Select(token => token.value));
                }
                if (fieldtype == FieldType.SIMPLIFICATION)
                {
                    tokens.AddOmittedOps();
                    var variables = tokens.ExtractVariables();
                    var derexp = string.Concat(tokens.Select(token => token.value));
                    res.Result = pymath.Simplify(derexp, variables);
                    res.InterpretedAs = derexp;
                }
                if (fieldtype == FieldType.BOOL)
                {
                    bool solve = tokens.Contains(new Token(Token.TYPE.TAG, Names.SOLVE));
                    Func<TokenList, int> SplitIndex = list => list.FindIndex(token => token == new Token(Token.TYPE.BRACKET_OPEN, Names.PIPE)) + 1;
                    int idx = SplitIndex(tokens);
                    tokens = new TokenList(tokens.GetRange(idx, tokens.Count() - idx));
                    foreach (var token in tokens)
                        if (token.type == Token.TYPE.VARIABLE && token.value.Length > 1)
                            throw new ParseException(Const.ERMSG_PREINVALIDTOKEN + token.value + Const.ERMSG_POSTINVALIDTOKEN);
                    var derexp = string.Concat(tokens.Select(token => token.value));
                    res.InterpretedAs = derexp;
                    var be = new BoolEng(derexp);
                    res.Result = be.CompileTable(solve ? "1" : "");
                    List<string> vars = tokens.ExtractVariables();
                    vars = Functions.MakeUnique(vars);
                    if (vars.Count() > Const.LIMIT_BOOLENGVARS)
                        throw new TimeOutException(Const.ERMSG_BOOLENGAMEXC);
                    vars.Sort();
                    string line = "{";
                    foreach (var onevar in vars)
                        line += ": " + onevar + " %";
                    line += ": F %}\n";
                    res.Result = line + res.Result;
                    if (latex)
                    {
                        res.Result = res.Result.Replace(":", "<th>");
                        res.Result = res.Result.Replace("%", "</th>");
                        res.Result = res.Result.Replace("{", "<tr>");
                        res.Result = res.Result.Replace("}", "</tr>");
                        res.Result = res.Result.Replace("[", "<td class=\"cellbool-res\">");
                        res.Result = res.Result.Replace("]", "</td>");
                        res.Result = res.Result.Replace("\n", "");
                        res.Result = "<table id=\"bool-res\">" + res.Result + "</table>";
                    }
                }
            }
            catch (Exception e)
            {
                pymath.Destroy();
                throw e;
            }
            pymath.Destroy();

            return res;
        }

        /// <summary>
        /// returns field of math to subtask computation of expression
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        static FieldType FindField(IEnumerable<Token> tokens)
        {
            if (tokens.Contains(new Token(Token.TYPE.TAG, Names.BOOLEAN)))
                return FieldType.BOOL;
            else if (tokens.Contains(new Token(Token.TYPE.TAG, Names.DERIVATIVE)))
                return FieldType.DERIVATIVE;
            else if (tokens.Contains(new Token(Token.TYPE.TAG, Names.INTEGRAL)))
                return FieldType.INTEGRAL;
            else if (tokens.Contains(new Token(Token.TYPE.TAG, Names.SOLVE)))
                return FieldType.SOLVE;
            else
                return FieldType.SIMPLIFICATION;
        }
    }
}
