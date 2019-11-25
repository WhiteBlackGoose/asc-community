using Processor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ascsite.Core
{
    abstract class ASCException : Exception
    {
        private static System.IO.StreamWriter fileStream = new System.IO.StreamWriter(Const.PATH_LOGFILE, true);
        private void Log(string msg)
        {
            fileStream.Write('[' + DateTime.Now.ToString() + "]: ");
            fileStream.Write(msg);
            fileStream.WriteLine();
            fileStream.Flush();
        }
        public ASCException(string msg) : base(msg) 
        {
            Log(msg);
        }

        public ASCException(Exception e)
        {
            string msg = e.Message;
            while(e.InnerException != null)
            {
                msg += "\n>" + e.InnerException.Message;
                e = e.InnerException;
            }
            Log(msg);
        }
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

    class InvalidRequestException : UserException
    {
        public InvalidRequestException() : base(Const.ERMSG_INVALID_REQUEST) { }
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
        public InternalException(string msg) : base(Const.ERMSG_INTERNAL_ERROR + ": " + msg) { }
        public InternalException(Exception e) : base(e) { }
    }
}
