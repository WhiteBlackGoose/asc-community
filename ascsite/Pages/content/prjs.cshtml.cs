using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ascsite.Core;
using AscSite.Core.WebInterface;
using AscSite.Pages.projects;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Processor;
using AscSite.Core.Interface.DbInterface;

namespace ascsite.Pages
{
    public class PrjsModel : PageModel
    {
        public string Result { get; set; }
        public string Title { get; set; }

        public PrjsModel()
        {
            
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
        public void OnGet()
        {
            if (Name == null)
                return;
            var Paths = GetPaths();
            if (Paths.ContainsKey(Name))
            {
                if (Paths[Name].Count == 1)
                {
                    Result = AscmdPage.LoadFrom(Paths[Name][0]).Render() + Const.ADD_LATEXSCRIPT;
                }
                else
                {
                    var res = new List<string>();
                    foreach (var prefix in Paths[Name])
                    {
                        var name = prefix;
                        var page = AscmdPage.LoadFrom(Paths[prefix][0]);
                        res.Add(page.RenderAnnotation("/content/prjs?name=" + name));
                    }
                    Result = string.Join("<hr>", res.ToArray());
                    Result += Const.ADD_LATEXSCRIPT;
                }
                
            }
        }
        
    }
}