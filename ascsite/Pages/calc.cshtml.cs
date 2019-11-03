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

       
        public string CalcResponse { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Expr 
        {
            get => Expr ?? string.Empty;
            set => Expr = value;
        }

        public void OnGet()
        {
            if(!string.IsNullOrEmpty(Expr)) 
                CalcResponse = calc.Count(Expr);
        }

        public void OnPost()
        {
            // to do
        }
    }
}
