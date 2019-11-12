using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ascsite.Core
{
    public class ParseException : Exception
    {
        public ParseException(string message) : base(Const.ERMSG_PREOUTPUT + message)
        {
            
        }
    }

    public class SecurityException : Exception
    {
        public SecurityException() : base(Const.ERMSG_SEQURITY)
        {

        }
    }

    public class TimeOutException : Exception
    {
        public TimeOutException(string msg) : base(Const.ERMSG_EXECUTE_TIMEOUT + ": " + msg)
        {

        }
    }
}
