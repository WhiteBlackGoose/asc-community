using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ascsite.Core;
using ascsite.Core.MSLInterface;
using AscSite.Core;
using AscSite.Core.Interface.Database;
using AscSite.Core.Interface.DbInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Processor;

namespace ascsite.Pages
{
    public static class MSLProgramPool
    {
        static Dictionary<string, MSLInterface> pool = new Dictionary<string, MSLInterface>();

        public static string CreateProgram()
        {
            string id = Guid.NewGuid().ToString();
            pool.Add(id, new MSLInterface());
            return id;
        }

        public static MSLInterface GetById(string id)
        {
            if (!pool.ContainsKey(id))
                throw new KeyNotFoundException();
            else
                return pool[id];
        }

        internal static void KillById(string id)
        {
            var process = GetById(id);
            process.Kill();
            pool.Remove(id);
        }
    }
    public class MSLModel : PageModel
    {
        private List<string> SampleList = new List<string>()
        {
            "vectors.msl",
            "arrays.msl",
            "methods.msl",
            "gc.msl",
            "exceptions.msl",
        };

        public MSLModel()
        {
            CodeText = MSLInterface.GetSample("sample.msl");
            AddSamplesList();
        }
        [BindProperty] public string CodeText { get; set; }
        public string OutputMSL { get; set; }
        public string Samples { get; set; }
        [BindProperty(SupportsGet = true)] public string Link { get; set; }
        [BindProperty(SupportsGet = true)] public string ReturnedId { get; set; } // currently unsupported

        public void OnGet()
        {
            if (Link != null)
            {
                try
                {
                    int linkId = LinkGenerator.FromString(Link).Integer;
                    var codeEntry = DbInterface.GetCodeLinkById(linkId);
                    if (codeEntry != null)
                        CodeText = codeEntry.Code;
                }
                catch (Exception e) { }
            }
        }

        private void AddSamplesList()
        {
            Samples = string.Empty;
            StringBuilder builder = new StringBuilder();
            foreach (var file in SampleList)
            {
                string fileName = file.Remove(file.IndexOf(".msl"), ".msl".Length);
                builder
                    .Append("<li class=\"asc-button huge-asc-button\" onclick=\"loadSample('")
                    .Append(fileName)
                    .Append("')\">")
                    .Append(fileName)
                    .Append("</li>")
                    .Append("<p id=\"")
                    .Append(fileName)
                    .Append("\" style=\"display: none\">")
                    .Append(MSLInterface.GetSample(file))
                    .Append("</p>");
            }
            Samples = builder.ToString();
        }
        public void OnPostCompileCode()
        {
            string id = MSLProgramPool.CreateProgram();
            MSLProgramPool.GetById(id).Execute(CodeText);
            ReturnedId = id;
            string output = string.Empty;
            var task = Task.Run(() => output = MSLProgramPool.GetById(id).PullOutput());
            bool timeout = !task.Wait(millisecondsTimeout: Const.LIMIT_MSL_EXECUTE_MS);
                
            MSLProgramPool.KillById(id);
            if (timeout)
                OutputMSL = Const.ERMSG_EXECUTE_TIMEOUT + ": " + Const.LIMIT_MSL_EXECUTE_MS.ToString() + "ms passed";
            else
                OutputMSL = output;

            AddSamplesList();
        }

        public void OnPostGenerateLink()
        {
            if (string.IsNullOrEmpty(CodeText)) return;
            int linkId = DbInterface.CreateCodeLink(CodeText);
            Link = LinkGenerator.FromInteger(linkId).Text;
            Response.Redirect("/msl?Link=" + Link); 
        }
    }
}
