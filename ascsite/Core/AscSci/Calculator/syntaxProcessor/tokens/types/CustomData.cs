using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ascsite.Core;

namespace Processor.syntaxProcessor.tokens.types
{
    public class CustomData
    {
        public enum Type
        {
            FIXED,
            RANGE,
            LIST,
            NONE
        }
        public static Type GetType(string data)
        {
            var lexer = new lexicProcessor.Lexer(data);
            var tokens = lexer.GenerateTokens();
            if (tokens.Count == 0)
                return Type.NONE;
            if (tokens.Count == 1 || (data.IndexOf(Const.DEL_LIST) == -1 && data.IndexOf(Const.DEL_RANGE) == -1))
                return Type.FIXED;
            var elems = data.Split(Const.DEL_LIST).ToList();
            if(elems.Count > 1)
            {
                var seemsList = true;
                foreach(var elem in elems)
                    if(BracketProcessor.BracketCheck(elem) != BracketProcessor.ERRORTYPE.OK)
                    {
                        seemsList = false;
                        break;
                    }
                if (seemsList)
                    return Type.LIST;
            }
            elems = data.Split(Const.DEL_RANGE).ToList();
            if (elems.Count() != 2)
                return Type.NONE;
            else
            {
                var seemsRange = true;
                foreach (var elem in elems)
                    if (BracketProcessor.BracketCheck(elem) != BracketProcessor.ERRORTYPE.OK)
                    {
                        seemsRange = false;
                        break;
                    }
                if (seemsRange)
                    return Type.RANGE;
                else
                    return Type.NONE;
            }
        }
    }
}
