using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace AscSite.Core.WebInterface
{
    public class WebFace
    {
        public static string Get(string url)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "C# console program");

            var content = client.GetStringAsync(url);

            return content.Result;
        }
    }
}
