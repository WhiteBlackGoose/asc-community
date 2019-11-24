using ascsite.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Processor.syntaxProcessor
{
    public class PostToken
    {
        public enum PostTokenType
        {
            UNDEFINED,
            VOID,
            EXPRESSION,
            EQUALITY
        }
        public PostTokenType type { get; private set; }
        public string Keyname { get; set; }
        public string Data { get; private set; }
        private bool frozen;
        public PostToken(string keyname)
        {
            Keyname = keyname;
            Data = "";
            frozen = false;
        }
        public void Freeze()
        {
            if (BracketProcessor.BracketCheck(Data) != BracketProcessor.ERRORTYPE.OK)
                throw new ParsingException("Invalid brackets in `" + Data + "`");
            frozen = true;
            if (string.IsNullOrEmpty(Data))
                this.type = PostTokenType.VOID;
            else if (Data.Contains("="))
                this.type = PostTokenType.EQUALITY;
            else
                this.type = PostTokenType.EXPRESSION;
        }

        public string GetBeforeEq()
        {
            if (!frozen)
                throw new InternalException("Attempt to call GetBeforeEq before freezing PostToken");
            if (type != PostTokenType.EQUALITY)
                throw new ParsingException("Expected `=`");
            return Data.Substring(0, Data.IndexOf("="));
        }

        public string GetAfterEq()
        {
            if (!frozen)
                throw new InternalException("Attempt to call GetAfterEq before freezing PostToken");
            if (type != PostTokenType.EQUALITY)
                throw new ParsingException("Expected `=`");
            return Data.Substring(Data.IndexOf("=") + 1);
        }

        public void AddData(string adata)
        {
            if (frozen)
                return;
            Data += adata;
        }

        public override string ToString()
        {
            if (!frozen)
                return "{ " + Keyname + " : " + Data + " }";
            else
                if (type == PostTokenType.VOID)
                return "{ " + Keyname + " : " + type + " }";
            else if (type == PostTokenType.EXPRESSION)
                return "{ " + Keyname + " : " + Data + " : " + type + " }";
            else
                return "{ " + Keyname + " : " + GetBeforeEq() + " : " + GetAfterEq() + " : " + type + " }";

        }

        public List<string> ParseAsList(char delim = ',')
        {
            return null;
        }
    }
}
