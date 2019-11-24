using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ascsite.Core;

namespace Processor.lexicProcessor
{
    public class TokenList : List<Token>
    {
        /// <summary>
        /// get all variables which are not in `exlude` list
        /// </summary>
        public Token Item()
        {
            if (this.Count() != 1)
                throw new ParsingException("Unexpected token");
            return this[0];
        }
        public List<string> ExtractVariables()
        {
            var vars = from token in this
                       where token.type == Token.Type.VARIABLE
                       select token.value;
            return new List<string>(vars);
        }

        public TokenList(IEnumerable<Token> collection) : base(collection) { }
        public TokenList() { }

        public TokenList Cut(int startPos)
        {
            var res = new TokenList();
            for (int i = startPos; i < this.Count(); i++)
                res.Add(this[i]);
            return res;
        }

        /// <summary>
        /// Inserts token between two elements in list after `startIndex` if their types are `type1` and `type2` and their values are not in `exclude` list
        /// </summary>
        public void InsertBetween(Token.Type type1, Token.Type type2, Token token)
        {
            if (this.Count() == 0) return;

            TokenList newList = new TokenList();

            for (int i = 0; i < this.Count() - 1; i++)
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
            var mult = new Token(Token.Type.MATH_OP, "*");
            var pow = new Token(Token.Type.MATH_OP, "**");
            
            InsertBetween(Token.Type.NUMBER, Token.Type.BRACKET_OPEN, mult);

            InsertBetween(Token.Type.BRACKET_CLOSE, Token.Type.SYSTEM_FUNCTION, mult);

            InsertBetween(Token.Type.VARIABLE, Token.Type.BRACKET_OPEN, mult);
            InsertBetween(Token.Type.VARIABLE, Token.Type.SYSTEM_FUNCTION, mult);
            InsertBetween(Token.Type.VARIABLE, Token.Type.NUMBER, pow);
            InsertBetween(Token.Type.VARIABLE, Token.Type.VARIABLE, mult);
            InsertBetween(Token.Type.BRACKET_CLOSE, Token.Type.VARIABLE, mult);
            

            InsertBetween(Token.Type.SYSTEM_CONST, Token.Type.BRACKET_OPEN, mult);
            InsertBetween(Token.Type.SYSTEM_CONST, Token.Type.SYSTEM_FUNCTION, mult);
            InsertBetween(Token.Type.SYSTEM_CONST, Token.Type.NUMBER, pow);
            InsertBetween(Token.Type.SYSTEM_CONST, Token.Type.SYSTEM_CONST, mult);
            InsertBetween(Token.Type.BRACKET_CLOSE, Token.Type.SYSTEM_CONST, mult);

            InsertBetween(Token.Type.NUMBER, Token.Type.VARIABLE, mult);
            InsertBetween(Token.Type.VARIABLE, Token.Type.SYSTEM_CONST, mult);
            InsertBetween(Token.Type.NUMBER, Token.Type.SYSTEM_CONST, mult);

            InsertBetween(Token.Type.NUMBER, Token.Type.SYSTEM_FUNCTION, mult);
            InsertBetween(Token.Type.BRACKET_CLOSE, Token.Type.NUMBER, pow);
            InsertBetween(Token.Type.BRACKET_CLOSE, Token.Type.BRACKET_OPEN, mult);
        }

        public override string ToString()
        {
            return string.Join(", ", this.Select(token => token.ToString()));
        }
    }
}
