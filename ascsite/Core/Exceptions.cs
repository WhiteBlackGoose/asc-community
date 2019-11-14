using processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ascsite.Core
{
    abstract class ASCException : Exception
    {
        public ASCException(string msg) : base(msg) { }
    }



    /*  USER ERROR  */
    abstract class UserException : ASCException
    {
        public UserException(string msg) : base(msg) { }
    }


    abstract class ResourceOutException : UserException
    {
        public ResourceOutException(string msg) : base(msg) { }
    }

    class TimeOutException : ResourceOutException
    {
        public TimeOutException() : base(Const.ERMSG_EXECUTE_TIMEOUT) { }
    }

    class CPUMEMOutException : ResourceOutException
    {
        public CPUMEMOutException() : base(Const.ERMSG_EXECUTE_CPUMEMOUT) { }
    }


    abstract class SyntaxException : UserException
    {
        public SyntaxException(string msg) : base(msg) { }
    }

    class LexisException : SyntaxException
    {
        public LexisException(string msg) : base(Const.ERMSG_INVALID_SYMBOL + ": " + msg) { }
    }

    class ParsingException : SyntaxException
    {
        public ParsingException(string msg) : base(Const.ERMSG_INVALID_SYNTAX + ": " + msg) { }
    }


    class SecurityException : UserException
    {
        public SecurityException() : base(Const.ERMSG_SEQURITY) { }
    }




    /*  INTERNAL EXCEPTION  */
    class InternalException : ASCException
    {
        public InternalException() : base(Const.ERMSG_INTERNAL_ERROR) { }
    }
}
