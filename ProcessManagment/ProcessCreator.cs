using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace ProcessManagment
{
    internal class ProcessCreator
    {
        internal static Process Create(string dir, string args)
        {
            Process proc = new Process();

            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.WorkingDirectory = dir; // Ignored in Linux :(
            proc.StartInfo.FileName = GetShell();
            proc.StartInfo.Arguments = ComposeArguments(args, dir);

            return proc;
        }

        private static string GetShell()
        {
            string shell;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                shell = "cmd";
            else
                shell = "/bin/bash";

            return shell;
        }

        private static string ComposeArguments(string args, string workDir)
        {
            string result;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                result = "/c " + args;
            else
                result = "-c \"" + $"cd {workDir}; " + args + "\""; // have to manually cd working dir

            return result;
        }
    }
}
