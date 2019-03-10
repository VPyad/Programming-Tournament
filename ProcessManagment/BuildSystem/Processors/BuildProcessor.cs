using ProcessManagment.Errors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace ProcessManagment.BuildSystem.Processors
{
    internal abstract class BuildProcessor
    {
        /// <summary>
        /// In megabybtes. May be omitted if building platform does not support applying limitations
        /// </summary>
        internal int MaxVirtualMemmory { get; set; } = 250;
        private long MaxVirtualMemoryKb { get => MaxVirtualMemmory * 1024; }

        /// <summary>
        /// In seconds.
        /// </summary>
        internal int Timeout { get; set; } = 15;

        internal abstract string BuildingParams { get; }
        internal abstract string ExecutionParams { get; }
        internal abstract string VersionCheckParams { get; }

        internal virtual string WorkingDir { get; set; }

        internal virtual string InputFileName { get; set; } = "input.txt";
        internal virtual string OutputFileName { get; set; } = "output.txt";
        internal abstract string SrcFileName { get; }
        internal string LogFileName { get; } = "log.txt";
        public FilesHelper FilesHelper { get; private set; }

        private Stopwatch stopwatch;
        internal BuildProcessorLogger logger;

        internal BuildProcessor(string workDir)
        {
            WorkingDir = workDir;
            stopwatch = new Stopwatch();
        }

        internal virtual ProcessResult Prepare(ProcessResult processResult)
        {
            processResult.Status = BuildStatus.Preparing;
            processResult.State = ProcessState.InProgress;

            if (string.IsNullOrEmpty(WorkingDir))
            {
                processResult.State = ProcessState.Error;
                processResult.Error = new WorkingDirNotSpecified();

                return processResult;
            }

            bool dirExists = FilesHelper.DirectoryExists(WorkingDir);
            if (!dirExists)
            {
                processResult.State = ProcessState.Error;
                processResult.Error = new InputFilesNotFound(InputFilesNotFound.InputFile.RootDir);

                return processResult;
            }
            logger = new BuildProcessorLogger(FilesHelper.AddPathSeparator(WorkingDir) + LogFileName);
            processResult.LogFilePath = FilesHelper.AddPathSeparator(WorkingDir) + LogFileName;

            logger.CheckInputFile();
            bool inputExist = FilesHelper.FileExists(WorkingDir, InputFileName);
            if (!inputExist)
            {
                processResult.State = ProcessState.Error;
                processResult.Error = new InputFilesNotFound(InputFilesNotFound.InputFile.Input);

                logger.InputFileOk(false);
                logger.EndWriting();

                return processResult;
            }
            logger.InputFileOk(true);

            logger.CheckSrcFile();
            bool srcExists = FilesHelper.FileExists(WorkingDir, SrcFileName);
            if (!srcExists)
            {
                processResult.State = ProcessState.Error;
                processResult.Error = new InputFilesNotFound(InputFilesNotFound.InputFile.Src);

                logger.SrcFileOk(false);
                logger.EndWriting();

                return processResult;
            }
            logger.SrcFileOk(true);

            logger.DeleteJunkFiles();
            FilesHelper.DeleteFilesExcept(WorkingDir, new string[] { SrcFileName, InputFileName, LogFileName, ".gitignore" });

            return processResult;
        }

        internal virtual ProcessResult TestBuildSystem(ProcessResult processResult)
        {
            logger.TestBuildSystem();
            processResult.Status = BuildStatus.BuildSystemTest;

            logger.CreateProcess(VersionCheckParams);
            Process proc = ProcessCreator.Create(WorkingDir, VersionCheckParams);
            processResult = ExecuteProcess<BuildSystemNotFound>(proc, processResult);

            if (processResult.State == ProcessState.Error)
            {
                logger.TestBuildSystem(false);
                logger.EndWriting();
            }
            else
                logger.TestBuildSystem(true);
            return processResult;
        }

        internal virtual ProcessResult Build(ProcessResult processResult)
        {
            logger.Build();
            processResult.Status = BuildStatus.Building;

            logger.CreateProcess(BuildingParams);
            Process proc = ProcessCreator.Create(WorkingDir, BuildingParams);
            processResult = ExecuteProcess<BuildFailed>(proc, processResult);

            if (processResult.State == ProcessState.Error)
            {
                logger.Build(false);
                logger.EndWriting();
            }
            else
                logger.Build(true);

            return processResult;
        }

        /// <summary>
        /// Override method if need custom build artifact processing, e.g. checking for executable files, etc. By default method do not anything
        /// </summary>
        /// <param name="processResult"></param>
        /// <returns></returns>
        internal virtual ProcessResult ProcessBuildingArtifacts(ProcessResult processResult)
        {
            logger.ProcessBuildingArtifacts();
            processResult.Status = BuildStatus.ProcessingBuildingArtifacts;

            logger.ProcessBuildingArtifacts(true);
            return processResult;
        }

        internal virtual ProcessResult Execute(ProcessResult processResult)
        {
            logger.Execute();
            processResult.Status = BuildStatus.Execution;

            logger.CreateProcess(ComposeExecutionParams());
            Process proc = ProcessCreator.Create(WorkingDir, ComposeExecutionParams());
            processResult = ExecuteProcess<ExecutionFailed>(proc, processResult);

            if (processResult.State == ProcessState.Error)
            {
                logger.Execute(false);
                logger.EndWriting();
            }
            else
                logger.Execute(true);

            processResult.ExecutionTime = (proc.ExitTime - proc.StartTime).TotalSeconds;
            processResult.ProcessorTimeUsage = proc.TotalProcessorTime.TotalSeconds;

            return processResult;
        }

        internal virtual ProcessResult ProcessExecutionArtifacts(ProcessResult processResult)
        {
            logger.ProcessBuildingArtifacts();
            processResult.Status = BuildStatus.ProcessingExecutionArtifacts;

            bool outputFileExists = FilesHelper.FileExists(WorkingDir, OutputFileName);
            if (!outputFileExists)
            {
                processResult.State = ProcessState.Error;
                processResult.Error = new OutputFileNotFound();

                logger.ProcessBuildingArtifacts(false);
                logger.EndWriting();

                return processResult;
            }

            processResult.OutputFilePath = FilesHelper.AddPathSeparator(WorkingDir) + OutputFileName;

            logger.ProcessResult(processResult);
            logger.EndWriting();

            return processResult;
        }

        /// <summary>
        /// Override if building platform does not support ulimit or procgov utils
        /// </summary>
        /// <returns></returns>
        internal virtual string ComposeExecutionParams()
        {
            StringBuilder sb = new StringBuilder();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                sb.Append("procgov --maxmem ")
                    .Append(MaxVirtualMemmory)
                    .Append("M ")
                    .Append(ExecutionParams);
            }
            else
            {
                sb.Append("(ulimit -v ")
                    .Append(MaxVirtualMemoryKb)
                    .Append("; ")
                    .Append(ExecutionParams)
                    .Append(")");
            }

            return sb.ToString();
        }

        internal virtual ProcessResult ExecuteProcess<T>(Process proc, ProcessResult processResult) where T : BaseError, new()
        {
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

                if (proc.ExitCode == 0 && string.IsNullOrEmpty(error))
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
                    defaultErrorType.Desc = output;

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

        private ProcessResult ExecuteProcess(Process proc, ProcessResult processResult)
        {
            return ExecuteProcess<BaseError>(proc, processResult);
        }
    }
}
