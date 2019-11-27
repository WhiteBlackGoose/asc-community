using System;
using System.Collections.Generic;
using System.Text;

namespace Processor.lexicProcessor
{
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
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
            SYSTEM_CONST
        }

        public Type type;
        public string value;

        public Token(Type type, string value)
        {
            this.type = type;
            this.value = value;
        }

        public bool CannotOpenWith()
        {
            return type == Type.MATH_OP || type == Type.BRACKET_CLOSE;
        }

        public bool CannotCloseWith()
        {
            return type == Type.MATH_OP || type == Type.BRACKET_OPEN || type == Type.SYSTEM_FUNCTION;
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
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)