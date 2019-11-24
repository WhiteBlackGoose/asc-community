using AscSite.Core.Interface.Database;
using AscSite.Core.Interface.DbInterface;
using AscSite.Pages.projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscSite.Core.Render.Renderers
{
    public class ProjectPageRenderer : PageRenderer
    {
        public ProjectPageRenderer(int postId) : base(postId)
        {
            
        }

        public override string Render()
        {
            var post = DbInterface.GetPostById(postId);
            var sb = new StringBuilder();
            sb
                .Append("<h1>")
                .Append(post.Name)
                .Append("</h1>")
                .Append("<br>")
                .Append(AscmdPage.Md2Html(post.Announcement))
                .Append("<hr>")
                .Append(AscmdPage.Md2Html(post.Body));
            return sb.ToString();
        }
    }
}
