using ascsite.Core.Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ascsite.Core.AscSci
{
    public class AscCalc : ICalc
    {
        string rawExpression;
        string resultExpression;

        public AscCalc(string expr)
        {
            rawExpression = expr;
            errorListImpl = new List<string>();
        }

        List<string> errorListImpl;
        public IEnumerable<string> Errors => errorListImpl;

        public string Result
        { 
            get 
            { 
                if(!Errors.Any()) 
                    throw new InvalidOperationException("Result cannot be get from AscCalc instance because an error has occured during parsing");
                return resultExpression; 
            } 
        }
            
        public bool ParseExpression()
        {
            errorListImpl.Add("AscCalc is not implemented yet");
            return false;
        }
    }
}
