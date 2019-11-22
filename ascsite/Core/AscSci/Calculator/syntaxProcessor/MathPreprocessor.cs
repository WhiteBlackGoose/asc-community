using Processor.syntaxProcessor.tokens;
using Processor.syntaxProcessor.tokens.types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Processor.syntaxProcessor
{
    public class MathPreprocessor
    {
        private PostTokenList tokens;
        private MajorSegmentList segms = null;
        public MajorSegmentList GetSegments()
        {
            if (segms == null)
                throw new Exception(); // TODO
            return segms;
        }
        public MathPreprocessor(PostTokenList tokens)
        {
            this.tokens = tokens;
        }

        public PostToken GetExpr()
        {
            return tokens.Select("expression").Item();
        }

        public void Process()
        {
            var res = new MajorSegmentList();
            foreach (var token in tokens)
            {
                if (FieldSegment.GetType(token.Keyname) != Field.Type.NONE)
                {
                    res.Add(new FieldSegment(token.Keyname, token.Data, FieldSegment.GetType(token.Keyname)));
                }
                else if (KeywordSegment.GetType(token.Keyname) != Keyword.Type.NONE)
                {
                    if (KeywordSegment.IsEquality(token.Data))
                        res.Add(new EqKeyword(token.Keyname, token.Data, KeywordSegment.GetType(token.Keyname)));
                    else
                        res.Add(new FixedKeyword(token.Keyname, token.Data, KeywordSegment.GetType(token.Keyname)));
                }
                else
                    res.Add(new ExpressionSegment(token.Keyname, token.Data));
            }
            segms = res;
        }

        public void Subs(string variable, string expression, int count = -1)
        {
            if (count == -1)
                count = segms.Count;
            segms.ForEach((segm) =>
            {
                if (count >= 0 && segm is ExpressionSegment)
                {
                    ((ExpressionSegment)segm).Substitute(variable, expression);
                    count--;
                }
            });
        }
    }
}
