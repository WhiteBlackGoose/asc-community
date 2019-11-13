using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ascsite.Core;
using ascsite.Core.MSLInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using processor;

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
            "asc_interop.msl",
            "math.msl",
            "utils.msl"
        };

        public MSLModel()
        {
            CodeText = MSLInterface.GetSample("sample.msl");
        }

        [BindProperty]
        public string CodeText { get; set; }

        public string OutputMSL { get; set; }

        public string Samples { get; set; }

        [BindProperty(SupportsGet = true)]
        public string InputMSL { get; set; }
        [BindProperty(SupportsGet = true)]
        public string IfPull { get; set; }
        public void ResponseVoid()
        {
            this.HttpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(string.Empty));
        }
        public void OnGet()
        {
            if (!string.IsNullOrEmpty(InputMSL))
            {
                // INPUT INTO MSL TO DO
                return;
            }
            if (!string.IsNullOrEmpty(IfPull))
            {
                if (!string.IsNullOrEmpty(ReturnedId))
                {
                    string output = MSLProgramPool.GetById(ReturnedId).PullOutput();
                    if (!string.IsNullOrEmpty(output))
                    {
                        this.HttpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(output));
                        return;
                    }
                }
                ResponseVoid();
            }

            // add samples to list
            AddSamplesList();
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

        [BindProperty(SupportsGet = true)]
        public string ReturnedId { get; set; }
        public void OnPost()
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
    }
}
