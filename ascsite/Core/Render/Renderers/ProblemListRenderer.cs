using System.Linq;
using AscSite.Core.Interface.Database;
using System.Text;
using PostType = AscSite.Core.Interface.Database.Post.TYPE;
using ascsite.Core;

namespace AscSite.Core.Render.Renderers
{
    public class ProblemListRenderer : ListRenderer
    {
        readonly string path;
        public ProblemListRenderer(string pagePath) : base(PostType.PROBLEM)
        {
            path = pagePath;
        }

        public override string Render()
        {
            return base.Render();
        }

        public override StringBuilder RenderOne(Post post, StringBuilder sb)
        {
            return sb
                .Append("<h2>")
                .Append(post.Name)
                .Append("</h2>")
                .Append("<br>")
                .Append(Functions.HtmlTextPreprocess(post.Announcement))
                .Append("<br><br>")
                .Append(AscButton(path + "?ProblemId=" + post.Id.ToString()))
                .Append("<hr>");
        }
    }
}
