using AscSite.Core.Interface.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscSite.Core.Render.Renderers
{
    using PostType = AscSite.Core.Interface.Database.Post.TYPE;
    public class ProjectListRenderer : ListRenderer
    {
        private string path;
        public override StringBuilder RenderOne(Post post, StringBuilder sb)
        {
            return base
                .RenderOne(post, sb)
                .Append("<br>")
                .Append(Renderer.AscButton(path + "?ProjectId=" + post.Id.ToString()))
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
