using System.Collections.Generic;
using System.Net.Http;

namespace AscSite.Core.WebInterface
{
    public static class WebFace
    {
        private static Dictionary<string, string> PageCache = new Dictionary<string, string>();
        public static string Get(string url)
        {
            if (PageCache.ContainsKey(url)) return PageCache[url];

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "C# console program");

            var content = client.GetStringAsync(url);

            PageCache[url] = content.Result;
            return PageCache[url];
        }

        public static void ResetCache() => PageCache.Clear();
    }
}
