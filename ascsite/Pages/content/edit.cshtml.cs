using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ascsite.Core;
using AscSite.Core.Interface.DbInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ascsite.Pages.content
{
    public class editModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string PrjId { get; set; }
        [BindProperty]
        public string PrjName { get; set; }
        [BindProperty]
        public string PrjAnnouncement { get; set; }
        [BindProperty]
        public string PrjBody { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public string LastError { get; set; }

        public void OnGet()
        {
            if(PrjId == null)
            {
                LastError = "";
                return;
            }
            try
            {
                var prj = DbInterface.GetProjectById(Convert.ToInt32(PrjId));
                PrjName = prj.Name;
                PrjAnnouncement = prj.Announcement;
                PrjBody = prj.Body;
                LastError = "Loaded";
            }
            catch
            {

            }
        }

        public void OnPost()
        {
            try
            {
                if (Functions.GetHashString(Password) != "706B21EA65649CBFD4CF10852FC063740812884D26FBDB2F01F010ADC5F5EA25")
                {
                    LastError = "Incorrect password";
                    return;
                }
                var prj = new AscSite.Core.Interface.Database.Project
                {
                    Id = Convert.ToInt32(PrjId),
                    Name = PrjName,
                    Announcement = PrjAnnouncement,
                    Body = PrjBody
                };
                DbInterface.AddOrUpdateProject(prj);
                LastError = "OK";
            }
            catch
            {

            }
        }
    }
}