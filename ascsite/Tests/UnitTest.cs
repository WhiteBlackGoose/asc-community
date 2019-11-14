using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AscSite.Tests
{
    public class UnitTestReport
    {
        private List<string> conditions;
        private List<string> messages;
        public UnitTestReport()
        {
            conditions = new List<string>();
            messages = new List<string>();
        }
        public void Add(string condition, string message)
        {
            conditions.Add(condition);
            messages.Add(message);
        }
        public string Compile()
        {
            string res = "<table>";
            res += "<tr><td>";
            res += "Condition";
            res += "</td><td>";
            res += "Message";
            res += "</td></tr>";
            for(int i = 0; i < conditions.Count; i++)
            {
                res += "<tr>";
                res += "<td>";
                res += conditions[i];
                res += "</td><td>";
                res += messages[i];
                res += "</td></tr>";
            }
            res += "</table> <style>td{border: 1px solid gray; padding: 2px}</style>";
            return res;
        }
    }

    public abstract class UnitTest
    {
        public abstract UnitTestReport Test();
    }
}
