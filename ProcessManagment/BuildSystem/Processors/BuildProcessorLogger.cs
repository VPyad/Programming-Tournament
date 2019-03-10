using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProcessManagment.BuildSystem.Processors
{
    internal class BuildProcessorLogger
    {
        private const string OK = "ok";
        private const string NOT_FOUND = "not found";
        private const string FAILED = " failed";

        private const string CHECK_INPUT_FILE = "Checking input file: ";
        private const string CHECK_SRC_FILE = "Checking src file: ";
        private const string DELETING_JUNK_FILES = "Deleting junks files if any..";

        private const string CREATE_PROCESS = "Creating process. Args: ";
        private const string START_PROCESS = "Starting process";
        private const string TIMEOUT_EXCEEDED = "Process execution timeout exceeded";
        private const string KILL_PROCESS = "Killing process";
        private const string UNSUCCESSFUL_EXIT_CODE = "Process exited with unsuccessful exit code";
        private const string EXIT_CODE = "Exit code: ";
        private const string ERROR = "Error: ";
        private const string OUTPUT = "Output: ";
        private const string SUCCESSFUL_EXIT_CODE = "Process successfuly finished";
        private const string PROCESS_FAILED = "An error occured during process execution";

        private const string TEST_BUILD_SYSTEM = "\r\nSTEP: Test build system";
        private const string TEST_BUILD_SYSTEM_RESULT = "Test build system: ";

        private const string BUILD = "\r\nSTEP: Build";
        private const string BUILD_RESULT = "Build: ";

        private const string PROCESS_BUILD_ARTIFACTS = "\r\nSTEP: Process Building Artifacts";
        private const string PROCESS_BUILD_ARTIFACTS_RESULT = "Process Building Artifacts: ";

        private const string EXECUTE = "\r\nSTEP: Execute";
        private const string EXECUTE_RESULT = "Execute: ";

        private const string PROCESS_EXECUTION_ARTIFACTS = "\r\nSTEP: Process Execution Artifacts";
        private const string PROCESS_EXECTION_ARTIFACTS_RESULT = "Process execution artifacts: ";

        private const string EXECUTION_TIME = "Execution time: ";
        private const string CPU_USAGE_TIME = "CPU usage time: ";
        private const string PEAK_MEMORY_USAGE = "Peak memory usage: ";
        private const string OUTPUT_FILE_PATH = "Output file path: ";

        private const string MOVE_SRC_TO_TEMP = "Moving src file to temp directory";
        private const string CREATE_CONSOLE_APP = "Creating .net Core console project";
        private const string CREATE_CONSOLE_APP_RESULT = "Create .net Core console project: ";

        private StreamWriter writer;

        internal BuildProcessorLogger(string filePath)
        {
            writer = File.CreateText(filePath);
            writer.AutoFlush = true;
        }

        internal void EndWriting()
        {
            writer.Close();
            writer.Dispose();
        }

        internal void CheckInputFile() => writer.Write(CHECK_INPUT_FILE);

        internal void InputFileOk(bool ok) => WriteOkNotFound(ok);

        internal void CheckSrcFile() => writer.Write(CHECK_SRC_FILE);

        internal void SrcFileOk(bool ok) => WriteOkNotFound(ok);

        internal void DeleteJunkFiles() => writer.WriteLine(DELETING_JUNK_FILES);

        internal void TestBuildSystem() => writer.WriteLine(TEST_BUILD_SYSTEM);

        internal void CreateProcess(string args) => writer.WriteLine(CREATE_PROCESS + args);

        internal void StartProcess() => writer.WriteLine(START_PROCESS);

        internal void ProcessTimedouted() => writer.WriteLine(TIMEOUT_EXCEEDED);

        internal void KillProcess() => writer.WriteLine(KILL_PROCESS);

        internal void UnsuccesssfulStatusCode(int exitCode, string error, string output)
        {
            writer.WriteLine(UNSUCCESSFUL_EXIT_CODE);

            writer.Write(EXIT_CODE);
            writer.WriteLine(exitCode);

            writer.Write(ERROR);
            writer.WriteLine(error);

            writer.Write(OUTPUT);
            writer.WriteLine(output);
        }

        internal void SuccessfulStatusCode(int exitCode, string output)
        {
            writer.WriteLine(SUCCESSFUL_EXIT_CODE);

            writer.Write(EXIT_CODE);
            writer.WriteLine(exitCode);

            writer.Write(OUTPUT);
            writer.WriteLine(output);
        }

        internal void ProcessFailed(string error)
        {
            writer.WriteLine(PROCESS_FAILED);
            writer.Write(ERROR);
            writer.WriteLine(error);
        }

        internal void TestBuildSystem(bool ok)
        {
            writer.Write(TEST_BUILD_SYSTEM_RESULT);
            WriteOkFailed(ok);
        }

        internal void Build() => writer.WriteLine(BUILD);

        internal void Build(bool ok)
        {
            writer.Write(BUILD_RESULT);
            WriteOkFailed(ok);
        }

        internal void ProcessBuildingArtifacts() => writer.WriteLine(PROCESS_BUILD_ARTIFACTS);

        internal void ProcessBuildingArtifacts(bool ok)
        {
            writer.Write(PROCESS_BUILD_ARTIFACTS_RESULT);
            WriteOkFailed(ok);
        }

        internal void Execute() => writer.WriteLine(EXECUTE);

        internal void Execute(bool ok)
        {
            writer.Write(EXECUTE_RESULT);
            WriteOkFailed(ok);
        }

        internal void ProcessExecutionArtifacts() => writer.WriteLine(PROCESS_EXECUTION_ARTIFACTS);

        internal void ProcessExecutionArtifacts(bool ok)
        {
            writer.Write(PROCESS_EXECTION_ARTIFACTS_RESULT);
            WriteOkFailed(ok);
        }

        internal void ProcessResult(ProcessResult processResult)
        {
            writer.Write(EXECUTION_TIME);
            writer.WriteLine(processResult.ExecutionTime);

            writer.Write(CPU_USAGE_TIME);
            writer.WriteLine(processResult.ProcessorTimeUsage);

            writer.Write(OUTPUT_FILE_PATH);
            writer.WriteLine(processResult.OutputFilePath);
        }

        internal void MoveSrcToTempDir() => writer.WriteLine(MOVE_SRC_TO_TEMP);

        internal void CreateConsoleProject() => writer.WriteLine(CREATE_CONSOLE_APP);

        internal void CreateConsoleProject(bool ok)
        {
            writer.Write(CREATE_CONSOLE_APP_RESULT);
            WriteOkFailed(ok);
        }

        internal void MoveSrcToWorkingDir() => writer.WriteLine();

        private void WriteOkNotFound(bool ok)
        {
            if (ok)
                writer.WriteLine(OK);
            else
                writer.WriteLine(NOT_FOUND);
        }

        private void WriteOkFailed(bool ok)
        {
            if (ok)
                writer.WriteLine(OK);
            else
                writer.WriteLine(FAILED);
        }
    }
}
