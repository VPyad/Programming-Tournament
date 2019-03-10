using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessManagment.BuildSystem.Processors
{
    internal class JavaBuildProcessor : BuildProcessor
    {
        private const string BUILD_ARGS = "javac program.java";
        private const string RUN_ARGS = "java program";
        private const string VERSION_ARGS = "javac --version";
        private const string SRC_FILE_NAME = "program.java";

        internal override string ExecutionParams => RUN_ARGS;

        // Executing java program does not support ulimit or procgov utils
        internal override string ComposeExecutionParams() => ExecutionParams;

        internal override string BuildingParams => BUILD_ARGS;

        internal override string VersionCheckParams => VERSION_ARGS;

        internal override string SrcFileName => SRC_FILE_NAME;

        internal JavaBuildProcessor(string workDir) : base(workDir) { }
    }
}
