using System;
using System.Collections.Generic;
using System.Text;

namespace processor.lexicProcessor
{
    public class Token
    {
        public enum Type
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

        public Type type;
        public string value;

        public Token(Type type, string value)
        {
            this.type = type;
            this.value = value;
        }

        public bool IsAmbiguous()
        {
            return type == Type.MATH_OP || type == Type.BRACKET_CLOSE || type == Type.BRACKET_OPEN;
        }

        public override string ToString()
        {
            return "{ " + type.ToString() + "; " + value.ToString() + " }";
        }

        public static bool operator ==(Token t1, Token t2) => t1.value == t2.value && t1.type == t2.type;
        public static bool operator !=(Token t1, Token t2) => !(t1 == t2);

        public static bool operator ==(Token t, string value) => t.value == value;
        public static bool operator !=(Token t, string value) => !(t == value);
    }
}
