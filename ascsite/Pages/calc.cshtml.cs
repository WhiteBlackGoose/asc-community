using ascsite.Core.AscSci.Calculator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


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
                ICalc calc = new AscCalc(Expression);
                if(calc.ParseExpression())
                {
                    CalcResponse = calc.Result;
                }
                else
                {
                    CalcResponse = string.Join('\n', calc.Errors);
                }
            }
        }

        public void OnPost()
        {
            // to do
        }
    }
}