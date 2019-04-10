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

        public ProceesResultErrorType GetErrorType(BaseError error)
        {
            KeyValuePair<Type, ProceesResultErrorType>? err = ERROR2ENUM_DIC.FirstOrDefault(x => x.Key == error.GetType());

            if (err == null)
                return ProceesResultErrorType.Unknown;
            else
                return err.Value.Value;
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
