using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AscSite.Core
{
    public class LinkGenerator
    {
        private const string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public int Integer { get; private set; }
        public string Text { get; private set; }
        public static LinkGenerator FromString(string text)
        {
            int integer = 0;
            foreach (char c in text)
            {
                int idx = alphabet.IndexOf(c);
                if (idx == -1)
                    throw new ArgumentException("invalig text was passed to LinkGenerator: " + text);

                integer *= alphabet.Length;
                integer += idx;
            }
            return new LinkGenerator(text, integer);
        }
        public static LinkGenerator FromInteger(int integer)
        {
            string text = "";
            int intCopy = integer;
            while (integer > 0)
            {
                text += alphabet[integer % alphabet.Length];
                integer /= alphabet.Length;
            }
            return new LinkGenerator(text, intCopy);
        }
        private LinkGenerator(string text, int integer)
        {
            Text = text;
            Integer = integer;
        }
    }
}
