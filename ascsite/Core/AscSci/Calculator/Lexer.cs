using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ascsite.Core.AscSci.Calculator
{
    public struct Token
    {
        public enum EnumType
        {
            ERROR,
            INTEGER,
            FLOAT,
            MATH_OP,
            BRACKET_OPEN,
            BRACKET_CLOSE,
        }

        public EnumType Type { get; }
        public string Value { get; }

        /// <summary>
        /// creates token with type and value provided
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public Token(EnumType type, string value)
        {
            Type = type;
            Value = value;
        }

        /// <summary>
        /// string respresentation of token in format: `{ type; value }`
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "{ " + Type.ToString() + "; " + Value.ToString() +  " }";
        }
    }

    public class Lexer
    {
        /// <summary>
        /// math operators as chars that lexer identifies as tokens
        /// </summary>
        private readonly string operatorList = "+-/*%^";

        /// <summary>
        /// generates list of tokens. If lexer cannot identify a token, its type is ERROR
        /// </summary>
        /// <param name="expr">
        /// string expression, which lexer tokenizes
        /// </param>
        /// <returns></returns>
        public IEnumerable<Token> GenerateTokens(string expr)
        {
            List<Token> tokenList = new List<Token>();
            if (string.IsNullOrEmpty(expr)) return tokenList;
            expr += '\n'; // to avoid checking end of expr

            int idx = 0;
            string cur = string.Empty;
            while (idx < expr.Length)
            {
                if (char.IsDigit(expr[idx]))
                {
                    tokenList.Add(ParseNumeric(expr, ref idx));
                }
                else if (operatorList.Contains(expr[idx]))
                {
                    tokenList.Add(ParseOperator(expr, ref idx));
                }
                else if ("{([|".Contains(expr[idx]))
                {
                    tokenList.Add(new Token(Token.EnumType.BRACKET_OPEN, expr[idx].ToString()));
                    idx++;
                }
                else if ("})]|".Contains(expr[idx]))
                {
                    tokenList.Add(new Token(Token.EnumType.BRACKET_CLOSE, expr[idx].ToString()));
                    idx++;
                }
                else if (expr[idx] == ' ' || expr[idx] == '\t' || expr[idx] == '\n')
                {
                    idx++;
                }
                else
                {
                    tokenList.Add(new Token(Token.EnumType.ERROR, "unresolved symbol: " + expr[idx]));
                    break;
                }
            }

            return tokenList;
        }

        /// <summary>
        /// parses mathematical operator, increasing idx accordingly
        /// </summary>
        /// <param name="expr">full expression</param>
        /// <param name="idx">index of operator</param>
        /// <returns></returns>
        private Token ParseOperator(string expr, ref int idx)
        {
            idx++;
            switch (expr[idx - 1])
            {
                case '+':
                    return new Token(Token.EnumType.MATH_OP, "+");
                case '-':
                    return new Token(Token.EnumType.MATH_OP, "-");
                case '*':
                    return new Token(Token.EnumType.MATH_OP, "*");
                case '/':
                    return new Token(Token.EnumType.MATH_OP, "/");
                case '%':
                    return new Token(Token.EnumType.MATH_OP, "%");
                case '^':
                    return new Token(Token.EnumType.MATH_OP, "^");
                default:
                    return new Token(Token.EnumType.ERROR, expr[idx].ToString());
            }
        }

        /// <summary>
        /// parses numerical value, increasing idx accordingly
        /// </summary>
        /// <param name="expr">full expression</param>
        /// <param name="idx">index of value</param>
        /// <returns></returns>
        private Token ParseNumeric(string expr, ref int idx)
        {
            StringBuilder builder = new StringBuilder();

            while (char.IsDigit(expr[idx]))
            {
                builder.Append(expr[idx]);
                idx++;
            }

            if (expr[idx] == '.' || expr[idx] == ',')
                builder.Append(expr[idx++]);
            else
                return new Token(Token.EnumType.INTEGER, builder.ToString());

            while (char.IsDigit(expr[idx]))
            {
                builder.Append(expr[idx]);
                idx++;
            }

            return new Token(Token.EnumType.FLOAT, builder.ToString());
        }
    }
}
