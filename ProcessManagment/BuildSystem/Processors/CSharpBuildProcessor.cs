using ProcessManagment.Errors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ProcessManagment.BuildSystem.Processors
{
    internal class CSharpBuildProcessor : BuildProcessor
    {
        private const string BUILD_ARGS = "dotnet build";
        private const string RUN_ARGS = "dotnet run";
        private const string VERSION_ARGS = "dotnet --version";
        private const string SRC_FILE_NAME = "program.cs";
        private const string CREATE_ARGS = "dotnet new console";
        private const string SRC_EXT = ".cs";

        internal override string BuildingParams => BUILD_ARGS;

        internal override string ExecutionParams => RUN_ARGS;

        internal override string VersionCheckParams => VERSION_ARGS;

        internal override string SrcFileName => SRC_FILE_NAME;

        internal CSharpBuildProcessor(string workDir) : base(workDir) { }

        internal override string ComposeExecutionParams() => ExecutionParams;

        internal override ProcessResult Prepare(ProcessResult processResult)
        {
            processResult = base.Prepare(processResult);

            if (processResult.State == ProcessState.Error)
                return processResult;

            var udidName = Guid.NewGuid().ToString();

            logger.MoveSrcToTempDir();
            FilesHelper.MoveToTemp(WorkingDir, SRC_FILE_NAME, udidName + SRC_EXT);

            logger.CreateProcess(CREATE_ARGS);
            Process proc = ProcessCreator.Create(WorkingDir, CREATE_ARGS);

            logger.CreateConsoleProject();
            processResult = ExecuteProcess<CreateDotNetConsoleProjectFailed>(proc, processResult);

            if (processResult.State == ProcessState.Error)
            {
                logger.CreateConsoleProject(false);
                logger.EndWriting();

                return processResult;
            }

            logger.CreateConsoleProject(true);
            logger.MoveSrcToWorkingDir();
            FilesHelper.MoveFromTemp(WorkingDir, udidName + SRC_EXT, SRC_FILE_NAME);

            return processResult;
        }
    }
}
