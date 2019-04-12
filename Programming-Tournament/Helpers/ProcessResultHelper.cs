using ProcessManagment.BuildSystem;
using ProcessManagment.Errors;
using Programming_Tournament.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Helpers
{
    public class ProcessResultHelper
    {
        private readonly LocService locService;
        private readonly Dictionary<Type, ProceesResultErrorType> ERROR2ENUM_DIC;

        public ProcessResultHelper(LocService locService)
        {
            ERROR2ENUM_DIC = PopulateErrorToEnumDic();
            this.locService = locService;
        }

        public Tuple<string, string, string> GetResultsTexts(ProcessResult processResult)
        {
            string resultText = "";
            string errorText = "";
            string errorDesc = "";

            if (processResult.State == ProcessState.Completed && processResult.Status == BuildStatus.Complete)
                resultText = locService.GetLocalizedHtmlString("Src has been successfully compiled and executed. To see details download log file. If you overall result incorrect - contact your lecturer.");
            else if (processResult.State == ProcessState.Error)
            {
                resultText = locService.GetLocalizedHtmlString("An error occuried. See description, or download log file");
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
                    errorText = locService.GetLocalizedHtmlString("Internal service error");
                    errorDesc = locService.GetLocalizedHtmlString("Error does not coused by provided program. Please try later again");
                    break;
                case ProceesResultErrorType.OutputFileNotFound:
                    errorText = locService.GetLocalizedHtmlString("Output file not found");
                    errorDesc = locService.GetLocalizedHtmlString("Program does not produced output.txt file.");
                    break;
                case ProceesResultErrorType.BuildFailed:
                    errorText = locService.GetLocalizedHtmlString("Build failed");
                    var buildError = (BuildFailed)error;
                    errorDesc = locService.GetLocalizedHtmlString(string.Format("Build failed. Message: {0}. Desc: {1}", buildError.Message, buildError.Desc));
                    break;
                case ProceesResultErrorType.ExecutionFailed:
                    errorText = locService.GetLocalizedHtmlString("Execution failed");
                    var execFailed = (BuildFailed)error;
                    errorDesc = locService.GetLocalizedHtmlString(string.Format("Error occuried during programm execution. Message: {0}. Desc: {1}", execFailed.Message, execFailed.Desc));
                    break;
                case ProceesResultErrorType.ProcessExecutionTimeouted:
                    errorText = locService.GetLocalizedHtmlString("Timeout exceeded");
                    errorDesc = locService.GetLocalizedHtmlString("Program exceeded timeout. Programm has been terminated.");
                    break;
                case ProceesResultErrorType.Unknown:
                    errorText = locService.GetLocalizedHtmlString("Internal service error");
                    errorDesc = locService.GetLocalizedHtmlString("Error does not coused by provided program. Please try later again");
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
