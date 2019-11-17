using ascsite.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processor.lexicProcessor
{
    public class Lexer
    {
        /// <summary>
        /// math operators as chars that lexer identifies as tokens
        /// </summary>
        private static readonly string operatorList = Const.MATHOP_OPLIST;

        private static readonly IEnumerable<string> keywordList = new HashSet<string>
        {
            Names.DERIVATIVE,
            Names.INTEGRAL,
            Names.SOLVE,
            Names.NOERRORS,
            Names.BOOLEAN,
            Names.FOR,
            Names.WHERE,
            Names.PLOT
        };

        private static readonly IEnumerable<string> functionList = new HashSet<string>
        {
            Names.COS,
            Names.SIN,
            Names.TAN,
            Names.LOG,
            Names.SQRT,
            Names.BFUNC,
            Names.TBFUNC,
            Names.PIECEWISE
        };

        private static readonly IEnumerable<string> constList = new HashSet<string>
        {
            Names.CONST_E,
            Names.CONST_PI,
            Names.CONST_TRUE,
            Names.CONST_EQ,
        };

        /// <summary>
        /// generates list of tokens. If lexer cannot identify a token, its type is ERROR
        /// </summary>
        /// <param name="expr">
        /// string expression, which lexer tokenizes
        /// </param>
        /// <returns></returns>
        /// 
        private string expr;
        public Lexer(string expr)
        {
            this.expr = expr;
        }

        public TokenList GenerateTokens()
        {
            TokenList tokenList = new TokenList();
            if (string.IsNullOrEmpty(expr)) return tokenList;
            expr += '\n'; // to avoid checking end of expr

            int idx = 0;

            while (idx < expr.Length)
            {
                if (char.IsLetter(expr[idx]))
                {
                    Token token = ParseVariable(expr, ref idx);
                    if (keywordList.Contains(token.value))
                        token.type = Token.Type.KEYWORD;
                    else if (functionList.Contains(token.value))
                        token.type = Token.Type.SYSTEM_FUNCTION;
                    else if (constList.Contains(token.value))
                        token.type = Token.Type.SYSTEM_CONST;
                    tokenList.Add(token);
                }
                else if (char.IsDigit(expr[idx]))
                {
                    tokenList.Add(ParseNumeric(expr, ref idx));
                }
                else if (operatorList.Contains(expr[idx]))
                {
                    tokenList.Add(new Token(Token.Type.MATH_OP, expr[idx].ToString()));
                    idx++;
                }
                else if ("{([|".Contains(expr[idx]))
                {
                    tokenList.Add(new Token(Token.Type.BRACKET_OPEN, expr[idx].ToString()));
                    idx++;
                }
                else if ("})]|".Contains(expr[idx]))
                {
                    tokenList.Add(new Token(Token.Type.BRACKET_CLOSE, expr[idx].ToString()));
                    idx++;
                }
                else if (char.IsWhiteSpace(expr[idx]))
                {
                    idx++;
                }
                else
                {
                    throw new LexisException(expr[idx].ToString()); // TODO
                }
            }

            return ConcatTokens(tokenList);
        }

        TokenList ConcatTwoTokens(TokenList list, Token token1, Token token2, Token replacement)
        {
            if (list.Count() == 0) return list;

            TokenList newList = new TokenList();
            newList.Add(list[0]);

            for (int i = 1; i < list.Count(); i++)
            {
                if (list[i - 1] == token1 && list[i] == token2)
                {
                    newList[newList.Count() - 1] = replacement;
                }
                else
                {
                    newList.Add(list[i]);
                }
            }
            return newList;
        }

        private TokenList ConcatTokens(TokenList list)
        {
            list = ConcatTwoTokens(
                list,
                new Token(Token.Type.MATH_OP, "^"),
                new Token(Token.Type.MATH_OP, "^"),
                new Token(Token.Type.MATH_OP, "^^")
            );

            return list;
        }

        private Token ParseVariable(string expr, ref int idx)
        {
            StringBuilder builder = new StringBuilder();
            while (char.IsLetter(expr[idx]))
            {
                builder.Append(expr[idx]);
                idx++;
            }
            return new Token(Token.Type.VARIABLE, builder.ToString());
        }

        private Token ParseNumeric(string expr, ref int idx)
        {
            StringBuilder builder = new StringBuilder();

            while (char.IsDigit(expr[idx]))
            {
                builder.Append(expr[idx]);
                idx++;
            }

            if (expr[idx] == '.')
                builder.Append(expr[idx++]);
            else
                return new Token(Token.Type.NUMBER, builder.ToString());

            while (char.IsDigit(expr[idx]))
            {
                builder.Append(expr[idx]);
                idx++;
            }

            return new Token(Token.Type.NUMBER, builder.ToString());
        }
    }
}
