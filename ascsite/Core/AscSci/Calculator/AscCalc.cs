using ascsite.Core.AscSci.Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ascsite.Core.AscSci.Calculator
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
        /// <summary>
        /// see ICalc.Errors summary
        /// </summary>
        public IEnumerable<string> Errors => errorListImpl;

        /// <summary>
        /// see ICalc.Result summary
        /// </summary>
        public string Result
        { 
            get 
            { 
                if(Errors.Any()) 
                    throw new InvalidOperationException("Result cannot be get from AscCalc instance because an error has occured during parsing");
                return resultExpression; 
            }
        }
            
        /// <summary>
        /// parses string expression using lexer. if any errors occured, returns false either returns true.
        /// errors can be get using Errors getter.
        /// result can be get using Result getter.
        /// </summary>
        /// <returns></returns>
        public bool ParseExpression()
        {
            var tokenList = new Lexer().GenerateTokens(rawExpression);
            if(tokenList.Any(token => token.Type == Token.EnumType.ERROR))
            {
                errorListImpl.Add("unresolved expression part: " + tokenList.First(token => token.Type == Token.EnumType.ERROR));
                return false;
            }
            resultExpression = string.Join(' ', tokenList);
            return true;
        }

        
    }
}
