using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ascsite.Core.MSLInterface
{
    public class MSLInterface
    {
        public static string Run(string code)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\Users\Momo\source\repos\MSL\Release\MSL.exe";
            start.UseShellExecute = false;
            start.CreateNoWindow = true; 
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            start.RedirectStandardInput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamWriter writer = process.StandardInput)
                {
                    writer.Write(code);
                }

                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    return result;
                }
            }
        }
    }
}
