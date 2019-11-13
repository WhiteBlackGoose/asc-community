using ascsite;
using ascsite.Core;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Security;
using ascsite.Core.AscSci.Calculator;
using processor;

namespace ascsite.Core.MSLInterface
{
    public class MSLInterface : ProgramInterface
    {
        public MSLInterface()
        {
            CompilerPath = Const.PATH_MSL;
            LibraryPaths.Add("math.msl");
            LibraryPaths.Add("asc_interop.msl");
            LibraryPaths.Add("utils.msl");
            
            var config = CreateProcessConfig();
            process = Process.Start(config);
        }

        Process process = null;

        public void Kill()
        {
            process.Kill();
            process.Close();
        }

        public void Execute(string code)
        {
            ImportLibraries(process);
            process.StandardInput.WriteLine(code);
            process.StandardInput.WriteLine(Const.PREFIX_MSL_END_FILEREAD);
            process.StandardInput.Flush();
        }

        public string Run(string code, string input)
        {
            try
            {
                StringBuilder builder = new StringBuilder();

                using (StreamReader reader = process.StandardOutput)
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (!ProcessMSLOutput(process, line))
                        {
                            builder.Append(line);
                            builder.Append('\n');
                        }
                    }
                }
                return builder.ToString();
            }
            catch (Exception e)
            {
                return Const.ERMSG_MSL_STARTERR + e.Message;
            }
        }

        public string PullOutput()
        {
            StringBuilder builder = new StringBuilder();
            while (!process.StandardOutput.EndOfStream)
            {
                string line = process.StandardOutput.ReadLine();
                if(!ProcessMSLOutput(process, line))
                {
                    builder.Append(line);
                    builder.Append('\n');
                }
            }
            return builder.ToString();
        }

        private void ImportLibraries(Process process)
        {
            LibraryPaths.ForEach(path =>
            {
                string code = GetSample(path);
                process.StandardInput.Write(code);
            });
        }

        // returns true if line was caught by function
        private bool ProcessMSLOutput(Process process, string line)
        {
            if (line.StartsWith(Const.PREFIX_MSL_CALC_EXECUTE))
            {
                line = line.Remove(0, Const.PREFIX_MSL_CALC_EXECUTE.Length);
                try
                {
                    if (!Security.ExprSecure(line))
                        throw new SecurityException();

                    string result = new AscCalc(line).Compute().SolidResult.Split('\n')[0] + '\n';
                    process.StandardInput.Write(result);
                    return true;
                }
                catch (Exception e)
                {
                    process.StandardInput.WriteLine(Const.ERMSG_PREOUTPUT + e.Message);
                    return true;
                }
            }
            return false;
        }

        public static string GetSample(string name)
        {
            using (StreamReader reader = new StreamReader(Const.PATH_SUBSAMPLES + "/" + name))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
