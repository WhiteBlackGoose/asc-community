using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace AscSite.Core.Interface
{
    //TODO
    public class MathSharpCompiler
    {
        private StringBuilder code;
        public MathSharpCompiler()
        {
            code = new StringBuilder();
            Add("using ");
        }
        public void Add(string code)
        {
            this.code.Append(code);
        }
        public string Run()
        {
            string csProgram = code.ToString();
            return "";
        }
    }
}
