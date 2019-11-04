using ascsite.Core.PyInterface.PyMath;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ascsite.Core.AscSci.Calculator
{
    public class AscCalc
    {
        string rawExpression;
        public enum FieldType
        {
            DERIVATIVE,
            BOOL
        }

        public AscCalc(string expr)
        {
            rawExpression = expr.Trim();
        }

        public string Compute()
        {
            TokenList tokens = new Lexer().GenerateTokens(rawExpression);
            FieldType fieldtype = FindField(tokens);

            if(fieldtype == FieldType.DERIVATIVE)
            {
                // get index of `for` keyword and select variable for differential after it
                int forPos = tokens.FindIndex(token => token == new Token(Token.TYPE.KEYWORD, CONST.FOR));
                if (forPos == -1 || forPos == tokens.Count())
                    throw new ArgumentException("No `" + CONST.FOR + "` token was found. Tokens are: " + tokens.ToString());
                string diffVar = tokens[forPos + 1].value;

                // throw everything before pipe keyword in token list
                Func<List<Token>, int> SplitIndex = list => list.FindIndex(token => token == new Token(Token.TYPE.BRACKET_OPEN, CONST.PIPE)) + 1;
                int idx = SplitIndex(tokens);
                tokens = new TokenList(tokens.GetRange(idx, tokens.Count() - idx));

                // add omitted ops and create string expression to pass it to PyMath
                tokens.AddOmittedOps();
                string derexp = string.Concat(tokens.Select(token => token.value));
                PyMath pymath = new PyMath();
                
                var variables = tokens.ExtractVariables();
                return pymath.Derivative(derexp, diffVar, variables);
            }
            return tokens.ToString();
        }

        /// <summary>
        /// returns field of math to subtask computation of expression
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        static FieldType FindField(IEnumerable<Token> tokens)
        {
            if (tokens.Contains(new Token(Token.TYPE.TAG, CONST.BOOLEAN)))
                return FieldType.BOOL;
            else
                return FieldType.DERIVATIVE;
        }
    }
}
