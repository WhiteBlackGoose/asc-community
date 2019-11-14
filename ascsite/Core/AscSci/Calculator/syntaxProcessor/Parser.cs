using processor.lexicProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace processor.syntaxProcessor
{
    public class Parser
    {
        private TokenList tokens;
        public Parser(TokenList tokens)
        {
            this.tokens = tokens;
        }

        public PostToken ExpMerge(TokenList list)
        {
            var res = new PostToken(Names.EXPRESSION);
            foreach (var l in list)
                res.AddData(l.value);
            return res;
        }

        public PostTokenList Parse()
        {
            var res = new PostTokenList();
            for(var i = 0; i < tokens.Count(); i++)
            {
                Token token = tokens[i];
                if (token.type != Token.Type.KEYWORD)
                {
                    if (res.LastFinished() && !token.IsAmbiguous())
                        res.Add(new PostToken("expression"));
                    res.Last.AddData(token.value);
                }
                else
                    res.Add(new PostToken(token.value));
            }
            return res;//
        }
    }
}
