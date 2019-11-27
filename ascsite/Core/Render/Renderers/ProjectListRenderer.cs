using ascsite.Core;
using AscSite.Core.Interface.Database;
using System.Linq;
using System.Text;

namespace AscSite.Core.Render.Renderers
{
    using PostType = AscSite.Core.Interface.Database.Post.TYPE;
    public class ProjectListRenderer : ListRenderer
    {
        private string path;
        public override StringBuilder RenderOne(Post post, StringBuilder sb)
        {
            return sb
                .Append("<h1>")
                .Append(Functions.HtmlTextPreprocess(post.Name))
                .Append("</h1>")
                .Append("<br>")
                .Append(Functions.Md2Html(post.Announcement))
                .Append("<br>")
                .Append(AscButton(path + "?ProjectId=" + post.Id.ToString()))
                .Append("<hr>");
        }

        public ProjectListRenderer(string path) : base(PostType.PROJECT)
        {
            this.path = path;
        }
        public override string Render()
        {
            var res = base.Render();
            return res;
        }
    }
}
