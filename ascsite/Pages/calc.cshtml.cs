using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ascsite.AscSci;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace ascsite
{
    public class CalcModel : PageModel
    {
        public AscCalc calc = new AscCalc();

        public string CalcResult { get; set; }

        public string expr = "";
        [BindProperty(SupportsGet = true)]
        public string Expr {
            get 
            {
                return expr;
            }
            set
            {
                expr = value;
            }
        }

        public void OnGet()
        {
            if(Expr != null)
                CalcResult = calc.Count(Expr);
        }

        public void OnPost()
        {
            
        }
    }
}
