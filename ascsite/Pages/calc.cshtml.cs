using ascsite.Core;
using ascsite.Core.AscSci.Calculator;
using ascsite.Core.PyInterface;
using ascsite.Core.PyInterface.PyMath;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;

namespace ascsite
{
    public class CalcModel : PageModel
    {
        public string CalcResponse { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Expression { get; set; }

        public void OnGet()
        {
            if (!string.IsNullOrEmpty(Expression))
            {
                AscCalc calc = new AscCalc(Expression);
                try
                {
                    CalcResponse = calc.Compute();
                }
                catch (Exception e)
                {
                    CalcResponse = "Error: " + e.Message;
                }
            }
            else
            {
                CalcResponse = "Received empty request.";
            }
        }

        public void OnPost()
        {
            // to do
        }
    }
}