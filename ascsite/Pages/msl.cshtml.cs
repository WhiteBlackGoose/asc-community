using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ascsite.Core.MSLInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ascsite.Pages
{
    public class MSLModel : PageModel
    {
        public MSLModel()
        {
            CodeText = "namespace Program\n" +
            "{\n"+
            "using namespace System;\n"+
            "\n"+
	        "    class ProgramClass\n"+
            "    {\n"+
            "        static function Main()\n"+
            "        {\n" +
            "            Console.Print(\"hello world\");\n" +
            "        }\n" +
            "    }\n" +
            "}\n";
    }

        [BindProperty]
        public string CodeText { get; set; }

        public string OutputMSL { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            string output = MSLInterface.Run(CodeText);
            OutputMSL = output;
        }
    }
}
