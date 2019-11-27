using System;
using ascsite.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AscSite.Core.Render.Renderers;

namespace ascsite.Pages
{
    public class ProblemsModel : PageModel
    {
        public string RawResult { get; set; }

        [BindProperty(SupportsGet=true)]
        public string ProblemId { set; get; }
        public void OnGet()
        {
            try
            {
                if (string.IsNullOrEmpty(ProblemId))
                    RawResult = new ProblemListRenderer(Const.PATH_PROBLEMS).Render();
                else
                    RawResult = new ProblemPageRenderer(Convert.ToInt32(ProblemId)).Render();
            }
            catch (InvalidRequestException e)
            {
                RawResult = e.Message;
            }
            catch(Exception e)
            {
                string _ = e.Message; // for debug
                RawResult = Const.ERMSG_INTERNAL_ERROR;
            }
        }
        
    }
}