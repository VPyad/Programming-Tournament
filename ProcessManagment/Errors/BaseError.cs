using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessManagment.Errors
{
    public class BaseError
    {
        public string Message { get; set; }

        public int? StatusCode { get; set; }

        public string Desc { get; set; }

        public BaseError() { }

        public BaseError(string message)
        {
            Message = message;
        }

        public BaseError(string message, int statusCode)
        {
            Message = message;
            StatusCode = statusCode;
        }

        public BaseError(string message, int statusCode, string desc)
        {
            Message = message;
            StatusCode = statusCode;
            Desc = desc;
        }
    }

    /// <summary>
    /// Throws if there are no 'program' file and input.txt in Working dir
    /// </summary>
    public class InputFilesNotFound : BaseError
    {
        public InputFile FileType { get; set; } = InputFile.Unknown;

        public InputFilesNotFound()
        {
        }

        public InputFilesNotFound(string message) : base(message)
        {
        }

        public InputFilesNotFound(string message, int statusCode) : base(message, statusCode)
        {
        }

        public InputFilesNotFound(string message, int statusCode, string desc) : base(message, statusCode, desc)
        {
        }

        public InputFilesNotFound(InputFile inputFile)
        {
            FileType = inputFile;
        }

        public InputFilesNotFound(string message, InputFile inputFile) : base(message)
        {
            FileType = inputFile;
        }

        public enum InputFile
        {
            RootDir,
            Input,
            Src,
            Unknown
        }
    }

    /// <summary>
    /// Throws if an error occured while process executed in shell (cmd or bash), i.e. process.Start() throw an error
    /// </summary>
    public class ShellExecutionFailed : BaseError
    {
        public ShellExecutionFailed()
        {
        }

        public ShellExecutionFailed(string message) : base(message)
        {
        }

        public ShellExecutionFailed(string message, int statusCode) : base(message, statusCode)
        {
        }

        public ShellExecutionFailed(string message, int statusCode, string desc) : base(message, statusCode, desc)
        {
        }
    }

    /// <summary>
    /// Throws if compiler and others tool are not installed in host OS
    /// </summary>
    public class BuildSystemNotFound : BaseError
    {
        public BuildSystemNotFound()
        {
        }

        public BuildSystemNotFound(string message) : base(message)
        {
        }

        public BuildSystemNotFound(string message, int statusCode) : base(message, statusCode)
        {
        }

        public BuildSystemNotFound(string message, int statusCode, string desc) : base(message, statusCode, desc)
        {
        }
    }

    /// <summary>
    /// Throws if execution of programs succeded, but output.txt was not found
    /// </summary>
    public class OutputFileNotFound : BaseError { }

    public class WorkingDirNotSpecified : BaseError { }

    public class BuildFailed : BaseError
    {
        public BuildFailed()
        {
        }

        public BuildFailed(string message) : base(message)
        {
        }

        public BuildFailed(string message, int statusCode) : base(message, statusCode)
        {
        }

        public BuildFailed(string message, int statusCode, string desc) : base(message, statusCode, desc)
        {
        }
    }

    public class ExecutionFailed : BaseError
    {
        public ExecutionFailed()
        {
        }

        public ExecutionFailed(string message) : base(message)
        {
        }

        public ExecutionFailed(string message, int statusCode) : base(message, statusCode)
        {
        }

        public ExecutionFailed(string message, int statusCode, string desc) : base(message, statusCode, desc)
        {
        }
    }

    public class ProcessExecutionTimeouted : BaseError { }

    public class CreateDotNetConsoleProjectFailed : BaseError { }
}
