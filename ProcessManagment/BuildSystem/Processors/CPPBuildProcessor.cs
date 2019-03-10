using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ProcessManagment.BuildSystem.Processors
{
    internal class CPPBuildProcessor : BuildProcessor
    {
        private const string BUILD_ARGS = "g++ program.cpp";
        private const string VERSION_ARGS = "g++ --version";
        private const string WIN_RUN_ARGS = "a.exe";
        private const string UNIX_RUN_ARGS = "./a.out";
        private const string SRC_FILE_NAME = "program.cpp";

        internal override string BuildingParams => BUILD_ARGS;

        internal override string ExecutionParams => GetRunArgs();

        internal override string VersionCheckParams => VERSION_ARGS;

        internal override string SrcFileName => SRC_FILE_NAME;

        internal CPPBuildProcessor(string workingDir) : base(workingDir) { }

        private static string GetRunArgs()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return WIN_RUN_ARGS;
            else
                return UNIX_RUN_ARGS;
        }
    }
}
