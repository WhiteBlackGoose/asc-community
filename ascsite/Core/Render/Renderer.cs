using AscSite.Core.Interface.Database;
using AscSite.Core.Interface.DbInterface;
using AscSite.Pages.projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscSite.Core.Render
{
    using PostType = AscSite.Core.Interface.Database.Post.TYPE;

    abstract public class Renderer
    {
        abstract public string Render();
        public static string AscButton(string src, string title = "Read more →")
        {
            return "<a class=\"asc-button big-asc-button\" href=\"" + src + "\">" + title + "</a>";
        }
    }

    abstract public class ListRenderer : Renderer
    {
        private PostType type;
        public int Limit { get; set; } = 10;
        public int Offset { get; set; } = 0;
        public ListRenderer(PostType type)
        {
            this.type = type;
        }

        public virtual StringBuilder RenderOne(Post post, StringBuilder sb)
        {
            sb
                .Append("<h1>")
                .Append(post.Name)
                .Append("</h1>")
                .Append("<br>")
                .Append(AscmdPage.Md2Html(post.Announcement));
            return sb;
        }

        public override string Render()
        {
            var posts = DbInterface.GetPosts(type, Offset, Limit);
            var sb = new StringBuilder();
            foreach (var post in posts)
                RenderOne(post, sb);
            return sb.ToString();
        }
    }

    abstract public class PageRenderer : Renderer
    {
        private int postId;
        public PageRenderer(int postId)
        {
            this.postId = postId;
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
