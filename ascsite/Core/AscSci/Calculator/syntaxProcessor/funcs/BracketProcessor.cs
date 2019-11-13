using System;
using System.Collections.Generic;
using System.Text;

namespace processor.syntaxProcessor
{
    public static class BracketProcessor
    {
        public enum ERRORTYPE
        {
            OK,
            CLOSED_TOO_EARLY,
            UNCLOSED
        }

        private static string bracketOpen = "({[";
        private static string bracketClose = ")}]";
        private static string bracketSolid = BracketProcessor.bracketOpen + BracketProcessor.bracketClose;

        public static bool IsOpen(char br)
        {
            return bracketOpen.Contains(br);
        }

        public static bool IsClose(char br)
        {
            return !IsOpen(br);
        }

        public static int Type(char br)
        {
            int pos1 = bracketOpen.IndexOf(br);
            int pos2 = bracketClose.IndexOf(br);
            if (pos1 == -1)
                return pos2;
            else
                return pos1;
        }

        public static bool IsBracket(char c)
        {
            return Type(c) > -1;
        }

        public static ERRORTYPE BracketCheck(string s)
        {
            string stack = "()";
            foreach (var c in s)
            {
                if (BracketProcessor.IsBracket(c))
                {
                    stack += c.ToString();
                    char c1 = stack[stack.Length - 2];
                    char c2 = stack[stack.Length - 1];
                    if (IsOpen(c1) && IsClose(c2) && Type(c1) != Type(c2))
                        return ERRORTYPE.CLOSED_TOO_EARLY;
                    if (IsOpen(c1) && IsClose(c2) && Type(c1) == Type(c2))
                        stack = stack.Substring(0, stack.Length - 2);
                }
            }
            return stack.Length == 2 ? ERRORTYPE.OK : ERRORTYPE.UNCLOSED;
        }

        public static bool IsValidEnd(char c)
        {
            if (Const.MATHOP_OPLIST.Contains(c))
                return false;
            if (bracketOpen.Contains(c))
                return false;
            if (c == '=')
                return false;
            return true;
        }
    }
}
