using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using AscSite.Core.Interface.DbInterface;
using Markdig;

namespace ascsite.Core
{
    public class Functions
    {

        public static List<string> MakeUnique(List<string> v)
        {
            var unique_items = new HashSet<string>(v);
            var result = new List<string>();
            foreach (string s in unique_items)
                result.Add(s);
            return result;
        }

        public static string Md2Html(string mdtext)
        {
            // some things should be preprocessed to html anyway
            return HtmlTextPreprocess(Markdown.ToHtml(mdtext));
        }

        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string FormatDbField(string field)
        {
            if (string.IsNullOrEmpty(field))
                field = "NULL";
            field = field
                .Replace('\n', ' ')
                .Replace("\r", "")
                .Replace('\t', ' ')
                .Trim(); 
            if (field.Length >= 20)
                field = field.Substring(0, 16) + "... ";
            return field;
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static string HtmlTextPreprocess(string text)
        {
            return text
                .Replace("\r\n", "<br>")
                .Replace("<!--", "< !--")
                .Replace("-->", "-- >");
        }

        public static string RepeatString(string str, int count)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < count; i++)
                builder.Append(str);
            return builder.ToString();
        }
        public static string MakeRelationString(string relationName, IEnumerable<DbInterface.PostUserEntry> entries)
        {
            if (entries.Count() == 0) return string.Empty;

            relationName += (entries.Count() < 2 ? ": " : "s: ");
            return "<br>" + relationName + string.Join(", ", entries.Select(e => e.userData.Name));
        }
    }
}
