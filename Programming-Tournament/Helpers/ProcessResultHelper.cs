using ProcessManagment.BuildSystem;
using ProcessManagment.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Helpers
{
    public class ProcessResultHelper
    {

        private readonly Dictionary<Type, ProceesResultErrorType> ERROR2ENUM_DIC;

        public ProcessResultHelper()
        {
            ERROR2ENUM_DIC = PopulateErrorToEnumDic();
        }

        public Tuple<string, string, string> GetResultsTexts(ProcessResult processResult)
        {
            string resultText = "";
            string errorText = "";
            string errorDesc = "";

            if (processResult.State == ProcessState.Completed && processResult.Status == BuildStatus.Complete)
                resultText = "Src has been successfully compiled and executed. To see details download log file. If you overall result incorrect - contact your lecturer.";
            else if (processResult.State == ProcessState.Error)
            {
                resultText = "An error occuried. See description, or download log file";
                var errors = GetErrorsText(GetErrorType(processResult.Error), processResult.Error);
                errorText = errors.Item1;
                errorDesc = errors.Item2;
            }

            return new Tuple<string, string, string>(resultText, errorText, errorDesc);
        }

        public ProceesResultErrorType GetErrorType(BaseError error)
        {
            KeyValuePair<Type, ProceesResultErrorType>? err = ERROR2ENUM_DIC.FirstOrDefault(x => x.Key == error.GetType());

            if (err == null)
                return ProceesResultErrorType.Unknown;
            else
                return err.Value.Value;
        }

        private Tuple<string, string> GetErrorsText(ProceesResultErrorType errorType, BaseError error)
        {
            string errorText = "";
            string errorDesc = "";

            switch (errorType)
            {
                case ProceesResultErrorType.Internal:
                    errorText = "Internal service error";
                    errorDesc = "Error does not coused by provided program. Please try later again";
                    break;
                case ProceesResultErrorType.OutputFileNotFound:
                    errorText = "Output file not found";
                    errorDesc = "Program does not produced output.txt file.";
                    break;
                case ProceesResultErrorType.BuildFailed:
                    errorText = "Build failed";
                    var buildError = (BuildFailed)error;
                    errorDesc = $"Build failed. Message: {buildError.Message}. Desc: {buildError.Desc}";
                    break;
                case ProceesResultErrorType.ExecutionFailed:
                    errorText = "Execution failed";
                    var execFailed = (BuildFailed)error;
                    errorDesc = $"Error occuried during programm execution. Message: {execFailed.Message}. Desc: {execFailed.Desc}";
                    break;
                case ProceesResultErrorType.ProcessExecutionTimeouted:
                    errorText = "Timeout exceeded";
                    errorDesc = "Program exceeded timeout. Programm has been terminated.";
                    break;
                case ProceesResultErrorType.Unknown:
                    errorText = "Internal service error";
                    errorDesc = "Error does not coused by provided program. Please try later again";
                    break;
            }

            return new Tuple<string, string>(errorText, errorDesc);
        }

        private Dictionary<Type, ProceesResultErrorType> PopulateErrorToEnumDic()
        {
            Dictionary<Type, ProceesResultErrorType> dic = new Dictionary<Type, ProceesResultErrorType>();

            dic.Add(typeof(InputFilesNotFound), ProceesResultErrorType.Internal);
            dic.Add(typeof(ShellExecutionFailed), ProceesResultErrorType.Internal);
            dic.Add(typeof(BuildSystemNotFound), ProceesResultErrorType.Internal);
            dic.Add(typeof(WorkingDirNotSpecified), ProceesResultErrorType.Internal);
            dic.Add(typeof(CreateDotNetConsoleProjectFailed), ProceesResultErrorType.Internal);

            dic.Add(typeof(OutputFileNotFound), ProceesResultErrorType.OutputFileNotFound);
            dic.Add(typeof(BuildFailed), ProceesResultErrorType.BuildFailed);
            dic.Add(typeof(ExecutionFailed), ProceesResultErrorType.ExecutionFailed);
            dic.Add(typeof(ProcessExecutionTimeouted), ProceesResultErrorType.ProcessExecutionTimeouted);

            return dic;
        }
    }

    public enum ProceesResultErrorType
    {
        Internal, // no user fault
        OutputFileNotFound,
        BuildFailed,
        ExecutionFailed,
        ProcessExecutionTimeouted,
        Unknown
    }
}
