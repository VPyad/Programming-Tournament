using ProcessManagment.Errors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace ProcessManagment.BuildSystem.Processors
{
    internal class PascalBuildProcessor : BuildProcessor
    {
        private const string BUILD_ARGS = "fpc program";
        private const string VERSION_ARGS = "fpc -version";
        private const string WIN_RUN_ARGS = "program.exe";
        private const string UNIX_RUN_ARGS = "./program";
        private const string SRC_FILE_NAME = "program.pas";

        public PascalCompiler Compiler { get; set; }

        internal override string BuildingParams => CompilerArgs();

        internal override string ExecutionParams => GetRunArgs();

        internal override string VersionCheckParams => VERSION_ARGS;

        internal override string SrcFileName => SRC_FILE_NAME;

        internal PascalBuildProcessor(string workDir, PascalCompiler compiler) : base(workDir)
        {
            Compiler = compiler;
        }

        // In windows fpc -version returned ExitCode = 1, while version is successfully printed, and compilation works
        internal override ProcessResult TestBuildSystem(ProcessResult processResult)
        {
            logger.TestBuildSystem();
            processResult.Status = BuildStatus.BuildSystemTest;

            logger.CreateProcess(VersionCheckParams);
            Process proc = ProcessCreator.Create(WorkingDir, VersionCheckParams);
            processResult = ExecuteTestBuildProcess<BuildSystemNotFound>(proc, processResult);

            if (processResult.State == ProcessState.Error)
            {
                logger.TestBuildSystem(false);
                logger.EndWriting();
            }
            else
                logger.TestBuildSystem(true);
            return processResult;
        }

        private ProcessResult ExecuteTestBuildProcess<T>(Process proc, ProcessResult processResult) where T : BaseError, new()
        {
            Stopwatch stopwatch = new Stopwatch();
            try
            {
                logger.StartProcess();
                proc.Start();

                stopwatch.Reset();
                stopwatch.Start();

                while (stopwatch.Elapsed.TotalSeconds <= Timeout)
                {
                    if (proc.HasExited)
                    {
                        break;
                    }
                }

                stopwatch.Stop();

                if (!proc.HasExited)
                {
                    logger.ProcessTimedouted();
                    logger.KillProcess();

                    proc.KillTree();

                    processResult.Error = new ProcessExecutionTimeouted();
                    processResult.State = ProcessState.Error;

                    return processResult;
                }

                string error = proc.StandardError.ReadToEnd();
                string output = proc.StandardOutput.ReadToEnd();
                int exitCode = proc.ExitCode;

                if ((proc.ExitCode == 0 || proc.ExitCode == 1) && string.IsNullOrEmpty(proc.StandardError.ReadToEnd()))
                {
                    logger.SuccessfulStatusCode(exitCode, output);
                    return processResult;
                }
                else
                {
                    logger.UnsuccesssfulStatusCode(exitCode, error, output);

                    processResult.State = ProcessState.Error;

                    var defaultErrorType = new T();
                    defaultErrorType.Message = error;
                    defaultErrorType.StatusCode = proc.ExitCode;

                    processResult.Error = defaultErrorType;
                }
            }
            catch (Exception ex)
            {
                logger.ProcessFailed(ex.Message);
                processResult.Error = new ShellExecutionFailed(ex.Message, proc.ExitCode);
                processResult.State = ProcessState.Error;
            }

            return processResult;
        }

        private string CompilerArgs()
        {
            switch (Compiler)
            {
                case PascalCompiler.FreePascal:
                    return BUILD_ARGS;
                case PascalCompiler.Delphi:
                    return BUILD_ARGS + " -Mdelphi";
                case PascalCompiler.ObjPascal:
                    return BUILD_ARGS + " -Mobjfpc";
                default:
                    return BUILD_ARGS;
            }
        }

        private static string GetRunArgs()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return WIN_RUN_ARGS;
            else
                return UNIX_RUN_ARGS;
        }

        public enum PascalCompiler
        {
            FreePascal,
            Delphi,
            ObjPascal
        }
    }
}
