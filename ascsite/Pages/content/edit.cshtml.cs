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
        public string PostId { get; set; }
        [BindProperty]
        public string PostName { get; set; }
        [BindProperty]
        public string PostAnnouncement { get; set; }
        [BindProperty]
        public string PostBody { get; set; }
        [BindProperty]
        public string PostType { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public string LastError { get; set; }

        public void OnGet()
        {
            if(PostId == null)
            {
                LastError = "";
                return;
            }
            try
            {
                var prj = DbInterface.GetPostById(Convert.ToInt32(PostId));
                PostName = prj.Name;
                PostAnnouncement = prj.Announcement;
                PostBody = prj.Body;
                PostType = prj.Type.ToString();
                LastError = "Loaded";
            }
            catch (Exception e)
            {
                LastError = "Error: " + e.Message;
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

                if(PostName == "" && PostAnnouncement == "" && PostBody == "")
                {
                    var Id = Convert.ToInt32(PostId);
                    DbInterface.RemovePostById(Id);
                }
                var post = new AscSite.Core.Interface.Database.Post
                {
                    Id = Convert.ToInt32(PostId),
                    Name = PostName,
                    Announcement = PostAnnouncement,
                    Body = PostBody,
                    Type = Convert.ToInt32(PostType)
                };
                if (PostId != "-1")
                    DbInterface.AddOrUpdatePost(post);
                else
                {
                    post.Id = DbInterface.Count() + 1;
                    DbInterface.AddPost(post);
                }
                LastError = "OK";
            }
            catch (Exception e)
            {
                LastError = "Error: " + e.Message;
            }
        }
    }
}