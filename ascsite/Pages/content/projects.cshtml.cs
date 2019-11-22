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

namespace ascsite.Pages.content
{
    public class projectsModel : PageModel
    {
        public string RawResult { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ProjectId { get; set; }
        public void OnGet()
        {
            if (string.IsNullOrEmpty(ProjectId))
            {
                RawResult = GetAllProjects();
            }
            else
            {
                try
                {
                    RawResult = GetProjectById(ProjectId);
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

        private string GetProjectById(string id)
        {
            var project = DbInterface.GetProjectById(Convert.ToInt32(id));
            return new StringBuilder()
                .Append("<h1>")
                .Append(project.Name)
                .Append("</h1>")
                .Append("<br>")
                .Append(AscmdPage.Md2Html(project.Announcement))
                .Append("<hr>")
                .Append(AscmdPage.Md2Html(project.Body))
                .ToString();
        }

        private string GetAllProjects()
        {
            var projects = DbInterface.GetProjects();
            var builder = new StringBuilder();
            builder.Append("<h1>Our projects</h1>");
            foreach (var project in projects)
            {
                builder
                    .Append("<h3>")
                    .Append(project.Name)
                    .Append("</h3>")
                    .Append("<br>")
                    .Append(AscmdPage.Md2Html(project.Announcement))
                    .Append("<a class=\"asc-button big-asc-button\" href=\"")
                    .Append("projects?ProjectId=")
                    .Append(project.Id)
                    .Append("\">Read more →</a>")
                    .Append("<hr>");
            }
            return builder.ToString();
        }
    }
}