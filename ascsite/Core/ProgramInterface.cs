using System.Diagnostics;
using System.Collections.Generic;

public abstract class ProgramInterface
{
    protected string CompilerPath { get; set; }

    protected List<string> LibraryPaths { get; } = new List<string>();

    protected ProcessStartInfo CreateProcessConfig()
    {
        return new ProcessStartInfo
        {
            FileName = CompilerPath,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = true
        };
    }
}