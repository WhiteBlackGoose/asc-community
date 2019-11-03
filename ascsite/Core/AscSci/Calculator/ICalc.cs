using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ascsite.Core.AscSci.Calculator
{
    interface ICalc
    {
        /// <summary>
        /// parses expression and returns true on success
        /// </summary>
        bool ParseExpression();

        /// <summary>
        /// list of errors, occured during parsing expression. Empty, if no errors occured
        /// </summary>
        IEnumerable<string> Errors { get; }

        /// <summary>
        /// returns result of parsed expression. 
        /// Calling this method when an error is occured, results in InvalidOperationException
        /// </summary>
        string Result { get; }
    }
}
