using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ascsite.Core;
using AscSite.Core.WebInterface;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using processor;

namespace ascsite.Pages
{
    public class PrjsModel : PageModel
    {
        public string Result { get; set; }
        public string Title { get; set; }
        private MarkdownPipeline Pipeline;

        public PrjsModel()
        {
            Pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        }

        public string GetMDFile(string path)
        {
            string pref = path.Substring(0, 6);
            string res = "";
            if (pref == "http:/" || pref == "https:")
                res = WebFace.Get(path);
            else
                res = System.IO.File.ReadAllText(path);
            return res;
        }

        [BindProperty(SupportsGet=true)]
        public string Name { set; get; }

        public Dictionary<string, List<string>> GetPaths()
        {
            var Paths = new Dictionary<string, List<string>>();
            string assoctext = System.IO.File.ReadAllText(Const.PATH_ASSOCPRJS);
            foreach (var line in assoctext.Split("\n"))
            {
                int pos = line.IndexOf("=");
                if (line != "" && pos != -1)
                {
                    string key = line.Substring(0, pos);
                    Paths[key] = new List<string>();
                    foreach (string path in line.Substring(pos + 1, line.Length - pos - 1).Split(Const.DEL_PRJSPAGES))
                    {
                        Paths[key].Add(path.Trim());
                    }
                }
            }
            return Paths;
        }
        public string TextPreprocess(string text)
        {
            text = text.Replace(Const.DEL_ANNSTART, "");
            text = text.Replace(Const.DEL_ANNEND, "");
            return text;
        }
        public string GetMd2Html(string path)
        {
            string mdtext = GetMDFile(path);
            mdtext = TextPreprocess(mdtext);
            return Markdown.ToHtml(mdtext, Pipeline);
        }
        public void OnGet()
        {
            if (Name == null)
                return;
            var Paths = GetPaths();
            if (Paths.ContainsKey(Name))
            {
                if (Paths[Name].Count == 1)
                    Result = GetMd2Html(Paths[Name][0]) + Const.ADD_LATEXSCRIPT;
                else
                {
                    var res = new List<string>();
                    foreach(var prefix in Paths[Name])
                    {
                        var name = prefix;
                        string mdtext = GetMDFile(Paths[prefix][0]);
                        int posstart = mdtext.IndexOf(Const.DEL_ANNSTART);
                        int posend = mdtext.IndexOf(Const.DEL_ANNEND);
                        string ann;
                        if (posstart != -1 && posend != -1 && posstart < posend)
                        {
                            ann = mdtext.Substring(posstart + Const.DEL_ANNSTART.Length, posend - posstart - Const.DEL_ANNSTART.Length);
                            ann += "<br><br><a class=\"asc-button big-asc-button\" href=\"/projects/prjs?name=" + name + "\">Read more →</a>";
                        }
                        else
                            ann = TextPreprocess(mdtext);
                        res.Add(Markdown.ToHtml(ann, Pipeline));
                    }
                    Result = string.Join("<hr>", res.ToArray());
                    Result += Const.ADD_LATEXSCRIPT;
                }
                
            }
        }
    }
}