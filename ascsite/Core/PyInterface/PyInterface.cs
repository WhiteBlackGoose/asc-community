using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ascsite.Core.PyInterface
{
    public class PyInterface
    {
        private string code;

        public void Import(string LibName)
        {
            Append("import " + LibName);
        }

        public void ImportAllFrom(string LibName)
        {
            Append("from " + LibName + " import *");
        }

        public void Append(string line)
        {
            if (line.Contains("\n"))
                throw new ArgumentException("new line prohibited");
            code += line + "\n";
        }

        public string Exec(string code="")
        {
            return Run(this.code + "\n" + code);
        }

        public static string Run(string code)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"D:\files\Anaconda\python.exe";
            start.UseShellExecute = false;
            start.CreateNoWindow = true;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            start.RedirectStandardInput = true;
            using (Process process = Process.Start(start))
            {
                using(StreamWriter writer = process.StandardInput)
                {
                    foreach (var line in code.Split('\n'))
                    {
                        writer.WriteLine(line);
                    }
                }
          
                using (StreamReader reader = process.StandardOutput)
                {
                    string stderr = process.StandardError.ReadToEnd(); // Here are the exceptions from our Python script
                    string result = reader.ReadToEnd(); // Here is the result of stdout(for example: print "test")
                    if (!string.IsNullOrEmpty(stderr))
                        throw new Exception(stderr);
                    return result;
                }
            }
        }

    }
}
