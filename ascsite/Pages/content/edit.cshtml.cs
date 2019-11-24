using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ascsite.Core;
using AscSite.Core.Interface.Database;
using AscSite.Core.Interface.DbInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ascsite.Pages.content
{
    using RelationType = UserPostContribution.TYPE;
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
        [BindProperty]
        public string RelationTypes { get; set; }

        public string Status { get; set; }

        public string StatusColor { get; set; } = "black";

        public void OnGet()
        {
            if(PostId == null)
            {
                Status = "";
                StatusColor = "black";
                return;
            }
            try
            {
                var prj = DbInterface.GetPostById(Convert.ToInt32(PostId));
                PostName = prj.Name;
                PostAnnouncement = prj.Announcement;
                PostBody = prj.Body;
                PostType = prj.Type.ToString();

                var users = DbInterface.GetUsersRelatedToPostById(Convert.ToInt32(PostId));
                RelationTypes = string.Empty;
                foreach (var entry in users)
                    RelationTypes += entry.userData.Id + ": " + (int)entry.postRelation + '\n';

                Status = "Loaded";
                StatusColor = "green";
            }
            catch (Exception e)
            {
                StatusColor = "red";
                Status = "Error: " + e.Message;
            }
        }

        public void OnPost()
        {
            RelationTypes = Functions.FillStringNa(RelationTypes);
            try
            {
                if (Functions.GetHashString(Password) != "706B21EA65649CBFD4CF10852FC063740812884D26FBDB2F01F010ADC5F5EA25")
                {
                    Status = "Incorrect password";
                    StatusColor = "red";
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
                {
                    int postId = Convert.ToInt32(PostId);
                    DbInterface.AddOrUpdatePost(post);

                    DbInterface.DeletePostRelations(postId);
                    var relations = ParseUserRelations(RelationTypes);
                    DbInterface.AddRelations(postId, relations);
                }
                else
                {
                    post.Id = DbInterface.Count() + 1;
                    DbInterface.AddPost(post);
                    var relations = ParseUserRelations(RelationTypes);
                    DbInterface.AddRelations(post.Id, relations);
                }
                Status = "Page has been changed successfully";
                StatusColor = "green";
            }
            catch (Exception e)
            {
                Status = "Error: " + e.Message;
                StatusColor = "red";
            }
        }

        private List<DbInterface.PostUserEntry> ParseUserRelations(string relationTypes)
        {
            List<DbInterface.PostUserEntry> result = new List<DbInterface.PostUserEntry>();
            string[] relations = relationTypes.Split("\r\n");
            foreach(var rel in relations)
            {
                if (string.IsNullOrEmpty(rel)) continue;
                var pair = rel.Split(':');
                result.Add(new DbInterface.PostUserEntry
                {
                    userData = new User { Id = Convert.ToInt32(pair[0].Trim()) },
                    postRelation = (RelationType)Convert.ToInt32(pair[1].Trim())
                });
            }
            return result;
        }
    }
}