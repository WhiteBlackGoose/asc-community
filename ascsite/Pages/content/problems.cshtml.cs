using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ascsite.Core;
using AscSite.Core.WebInterface;
using AscSite.Pages.projects;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Processor;
using AscSite.Core.Interface.DbInterface;
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
                RawResult = Const.ERMSG_INTERNAL_ERROR;
            }
        }
        
    }
}