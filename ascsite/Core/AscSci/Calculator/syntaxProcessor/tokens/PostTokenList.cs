using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace processor.syntaxProcessor
{
    public class PostTokenList : List<PostToken>
    {
        public PostToken Item()
        {
            if (this.Count() != 1)
                throw new Exception(); // TODO
            return this[0];
        }

        public PostTokenList Select(string text)
        {
            var res = new PostTokenList();
            foreach (var token in this)
                if (token.Keyname == text)
                    res.Add(token);
            return res;
        }

        public PostTokenList Select(PostToken.PostTokenType type)
        {
            var res = new PostTokenList();
            foreach (var token in this)
                if (token.type == type)
                    res.Add(token);
            return res;
        }

        public void Freeze()
        {
            foreach (var v in this)
                v.Freeze();
        }

        public PostTokenList()
        {

        }

        public bool LastFinished()
        {
            if (this.Count() == 0)
                return true;
            var last = this.Last;
            if (last.Data == "")
                return false;
            var lastsym = last.Data[last.Data.Length - 1];
            if (BracketProcessor.IsValidEnd(lastsym) && BracketProcessor.BracketCheck(last.Data) != BracketProcessor.ERRORTYPE.UNCLOSED)
                return true;
            return false;
        }

        public PostToken Last
        {
            get
            {
                return this[this.Count() - 1];
            }
        }

        public override string ToString()
        {
            var r = "";
            foreach (var m in this)
                r += m.ToString();
            return r.Replace("}{", "}\n{");
        }

        public int Pos(PostToken token)
        {
            for (int i = 0; i < this.Count(); i++)
                if (this[i] == token)
                    return i;
            return -1;
        }

        public PostTokenList CutFrom(PostToken token)
        {
            var res = new PostTokenList();
            int pos = Pos(token);
            if (pos == -1)
                throw new Exception(); // TODO
            for (int i = pos; i < this.Count(); i++)
                res.Add(this[i]);
            return res;
        }
    }
}
