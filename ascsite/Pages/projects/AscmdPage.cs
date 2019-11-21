using ascsite.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Markdig;
using AscSite.Core.WebInterface;

namespace AscSite.Pages.projects
{
    public class AscmdPage
    {
        public string Body { get => text; }
        public string Annotation { get; internal set; }
        public string ProblemAuthor { get; internal set; }
        public string InvestigationAuthor { get; internal set; }
        public string SolutionAuthor { get; internal set; }
        private string text;
        private static MarkdownPipeline Pipeline;
        public static AscmdPage LoadFrom(string path)
        {
            string pref = path.Substring(0, 6);
            string res = "";
            if (pref == "http:/" || pref == "https:")
                res = WebFace.Get(path);
            else
                res = System.IO.File.ReadAllText(path);
            return new AscmdPage(res);
        }
        public AscmdPage(string text)
        {
            Pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            this.text = text;
            ProblemAuthor = GetTag(Const.DEL_PROBAUTHOR);
            InvestigationAuthor = GetTag(Const.DEL_INVAUTHOR);
            SolutionAuthor = GetTag(Const.DEL_SOLUTAUTHOR);
            RemoveT(Const.DEL_PROBAUTHOR);
            RemoveT(Const.DEL_INVAUTHOR);
            RemoveT(Const.DEL_SOLUTAUTHOR);
            Annotation = Unjail(Const.DEL_ANNSTART, Const.DEL_ANNEND);
            Remove(Const.DEL_ANNSTART);
            Remove(Const.DEL_ANNEND);
        }
        private void Remove(string tag)
        {
            text = text.Replace(tag, "");
        }
        private void RemoveT(string tag)
        {
            string finaltag = tag + "{";
            int pos = text.IndexOf(finaltag);
            int endpos = text.IndexOf("}", pos + 1);
            if (pos == -1 || endpos == -1)
                return;
            text = text.Substring(0, pos) + text.Substring(endpos + 1);
        }
        private string Unjail(string start, string end)
        {
            int posstart = text.IndexOf(start);
            int posend = text.IndexOf(end);
            if (posstart != -1 && posend != -1 && posstart < posend)
                return text.Substring(posstart + start.Length, posend - posstart - start.Length);
            else
                return null;
        }
        private string GetTag(string tag)
        {
            string finaltag = tag + "{";
            int pos = text.IndexOf(finaltag);
            if (pos == -1)
                return null;
            int endpos = text.IndexOf("}", pos + 1);
            if (endpos == -1)
                return null;
            int leng = endpos - pos - finaltag.Length;
            string tagcontent = text.Substring(pos + finaltag.Length, leng);
            return tagcontent;
        }
        private static string Md2Html(string mdtext)
        {
            return Markdown.ToHtml(mdtext, Pipeline);
        }
        public string Render()
        {
            string res = Md2Html(Body);
            if (ProblemAuthor != null)
                res += "<br>Author" + (ProblemAuthor.Contains(',') ? "s" : "") + 
                    " of the problem " + ProblemAuthor;
            if (InvestigationAuthor != null)
                res += "<br>Author" + (InvestigationAuthor.Contains(',') ? "s" : "") + 
                    " of the investigation " + InvestigationAuthor;
            if (SolutionAuthor != null)
                res += "<br>Author" + (SolutionAuthor.Contains(',') ? "s" : "") + 
                    " of the solution " +  SolutionAuthor;
            return res;
        }
        public string RenderAnnotation(string readMoreButton)
        {
            string res = "";
            if (Annotation != null)
                res = Annotation;
            else
                res = Body;
            return Md2Html(res) + "<br><a class=\"asc-button big-asc-button\" href=\"" + readMoreButton + "\">Read more →</a>";
        }
    }
}
