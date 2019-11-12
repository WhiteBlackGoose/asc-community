using ascsite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ascsite.Core.AscSci.Calculator
{
    public struct Token
    {
        public enum TYPE
        {
            ERROR,
            NUMBER,
            MATH_OP,
            BRACKET_OPEN,
            BRACKET_CLOSE,
            VARIABLE,
            TAG,
            SYSTEM_FUNCTION,
            KEYWORD,
        }

        public TYPE type;
        public string value;

        public Token(TYPE type, string value)
        {
            this.type = type;
            this.value = value;
        }

        public override string ToString()
        {
            return "{ " + type.ToString() + "; " + value.ToString() +  " }";
        }

        public static bool operator ==(Token t1, Token t2) => t1.value == t2.value && t1.type == t2.type;
        public static bool operator !=(Token t1, Token t2) => !(t1 == t2);
    }

    public class TokenList : List<Token>
    {
        /// <summary>
        /// get all variables which are not in `exlude` list
        /// </summary>
        public List<string> ExtractVariables()
        {
            var vars = from token in this
                       where token.type == Token.TYPE.VARIABLE
                       select token.value;
            return new List<string>(vars);
        }

        public TokenList(IEnumerable<Token> collection) : base(collection) { }
        public TokenList() { }

        /// <summary>
        /// Inserts token between two elements in list after `startIndex` if their types are `type1` and `type2` and their values are not in `exclude` list
        /// </summary>
        public void InsertBetween(Token.TYPE type1, Token.TYPE type2, Token token)
        {
            if (this.Count() == 0) return;

            TokenList newList = new TokenList();

            for(int i = 0; i < this.Count() - 1; i++)
            {
                newList.Add(this[i]);
                if (this[i].type == type1 && this[i + 1].type == type2)
                {
                    newList.Add(token);
                }
            }
            newList.Add(this.Last());
            Clear();
            AddRange(newList);
        }

        /// <summary>
        /// adds omitted multiply and power ops in token list, scanning from `start` and ignoring tokens with value from `exclude` list
        /// </summary>
        public void AddOmittedOps()
        {
            var mult = new Token(Token.TYPE.MATH_OP, "*");
            var pow = new Token(Token.TYPE.MATH_OP, "**");
            InsertBetween(Token.TYPE.VARIABLE, Token.TYPE.BRACKET_OPEN, mult);
            InsertBetween(Token.TYPE.NUMBER, Token.TYPE.BRACKET_OPEN, mult);

            InsertBetween(Token.TYPE.BRACKET_CLOSE, Token.TYPE.SYSTEM_FUNCTION, mult);
            InsertBetween(Token.TYPE.VARIABLE, Token.TYPE.SYSTEM_FUNCTION, mult);
            InsertBetween(Token.TYPE.NUMBER, Token.TYPE.SYSTEM_FUNCTION, mult);

            InsertBetween(Token.TYPE.BRACKET_CLOSE, Token.TYPE.NUMBER, pow);
            InsertBetween(Token.TYPE.VARIABLE, Token.TYPE.NUMBER, pow);

            InsertBetween(Token.TYPE.BRACKET_CLOSE, Token.TYPE.VARIABLE, mult);
            InsertBetween(Token.TYPE.NUMBER, Token.TYPE.VARIABLE, mult);

            InsertBetween(Token.TYPE.BRACKET_CLOSE, Token.TYPE.BRACKET_OPEN, mult);
        }

        public override string ToString()
        {
            return string.Join(", ", this.Select(token => token.ToString()));
        }
    }

    public class Lexer
    {
        /// <summary>
        /// math operators as chars that lexer identifies as tokens
        /// </summary>
        private static readonly string operatorList = "+-/*%^<>=,&!";

        private static readonly IEnumerable<string> tagList = new HashSet<string>
        {
            Names.DERIVATIVE,
            Names.INTEGRAL,
            Names.SOLVE,
            Names.NOERRORS,
            Names.BOOLEAN,
        };

        private static readonly IEnumerable<string> functionList = new HashSet<string>
        {
            Names.COS,
            Names.SIN,
            Names.TAN,
            Names.LOG,
            Names.SQRT,
            Names.BFUNC,
            Names.TBFUNC
        };

        private static readonly IEnumerable<string> keywordList = new HashSet<string>
        {
            Names.FOR,
        };

        /// <summary>
        /// generates list of tokens. If lexer cannot identify a token, its type is ERROR
        /// </summary>
        /// <param name="expr">
        /// string expression, which lexer tokenizes
        /// </param>
        /// <returns></returns>
        public TokenList GenerateTokens(string expr)
        {
            TokenList tokenList = new TokenList();
            if (string.IsNullOrEmpty(expr)) return tokenList;
            expr += '\n'; // to avoid checking end of expr

            int idx = 0;

            while (idx < expr.Length)
            {
                if(expr[idx] == '_' || char.IsLetter(expr[idx]))
                {
                    Token token = ParseVariable(expr, ref idx);
                    if (tagList.Contains(token.value))
                        token.type = Token.TYPE.TAG;
                    else if (functionList.Contains(token.value))
                        token.type = Token.TYPE.SYSTEM_FUNCTION;
                    else if (keywordList.Contains(token.value))
                        token.type = Token.TYPE.KEYWORD;
                    tokenList.Add(token);
                }
                else if (char.IsDigit(expr[idx]))
                {
                    tokenList.Add(ParseNumeric(expr, ref idx));
                }
                else if (operatorList.Contains(expr[idx]))
                {
                    tokenList.Add(new Token(Token.TYPE.MATH_OP, expr[idx].ToString()));
                    idx++;
                }
                else if ("{([|".Contains(expr[idx]))
                {
                    tokenList.Add(new Token(Token.TYPE.BRACKET_OPEN, expr[idx].ToString()));
                    idx++;
                }
                else if ("})]|".Contains(expr[idx]))
                {
                    tokenList.Add(new Token(Token.TYPE.BRACKET_CLOSE, expr[idx].ToString()));
                    idx++;
                }
                else if (char.IsWhiteSpace(expr[idx]))
                {
                    idx++;
                }
                else
                {
                    tokenList.Add(new Token(Token.TYPE.ERROR, Const.ERMSG_UNRESOLVED_SYMBOL + expr[idx]));
                    break;
                }
            }

            return tokenList;
        }

        private Token ParseVariable(string expr, ref int idx)
        {
            StringBuilder builder = new StringBuilder();
            while(expr[idx] == '_' || char.IsLetter(expr[idx]))
            {
                builder.Append(expr[idx]);
                idx++;
            }
            return new Token(Token.TYPE.VARIABLE, builder.ToString());
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
                return new Token(Token.TYPE.NUMBER, builder.ToString());

            while (char.IsDigit(expr[idx]))
            {
                builder.Append(expr[idx]);
                idx++;
            }

            return new Token(Token.TYPE.NUMBER, builder.ToString());
        }
    }
}
