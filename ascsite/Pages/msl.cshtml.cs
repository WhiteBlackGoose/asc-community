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
