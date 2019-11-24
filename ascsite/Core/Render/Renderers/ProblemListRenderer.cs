﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AscSite.Core.Interface.Database;

namespace AscSite.Core.Render.Renderers
{
    using System.Text;
    using PostType = AscSite.Core.Interface.Database.Post.TYPE;
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
            return base
                .RenderOne(post, sb)
                .Append("<br>")
                .Append(Renderer.AscButton(path + "?ProblemId=" + post.Id.ToString()))
                .Append("<hr>");
        }
    }
}