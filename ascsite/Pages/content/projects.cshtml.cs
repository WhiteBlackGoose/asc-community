using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AscSite.Core.Interface.DbInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AscSite.Pages.projects;
using ascsite.Core;
using AscSite.Core.Render.Renderers;

namespace ascsite.Pages.content
{
    public class projectsModel : PageModel
    {
        public string RawResult { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ProjectId { get; set; }
        public void OnGet()
        {
            try
            {
                if (string.IsNullOrEmpty(ProjectId))
                    RawResult = new ProjectListRenderer(Const.PATH_PROJECTS).Render();
                else
                    RawResult = new ProjectPageRenderer(Convert.ToInt32(ProjectId)).Render();
            }
            catch (InvalidRequestException e)
            {
                RawResult = e.Message;
            }
            catch
            {
                RawResult = Const.ERMSG_INTERNAL_ERROR;
            }
        }
    }
}