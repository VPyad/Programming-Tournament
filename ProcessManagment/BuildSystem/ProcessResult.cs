using ProcessManagment.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessManagment.BuildSystem
{
    public class ProcessResult
    {
        public int Id { get; set; }

        public BuildStatus Status { get; internal set; } = BuildStatus.WaitingToBuild;

        public ProcessCondition Condition { get; internal set; }

        public string LogFilePath { get; internal set; }

        public string OutputFilePath { get; internal set; }

        public BaseError Error { get; internal set; }

        public ProcessState State { get; internal set; } = ProcessState.Awaiting;

        public double ProcessorTimeUsage { get; internal set; }

        public double ExecutionTime { get; internal set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Status: ").AppendLine(Status.ToString())
                //.AppendLine("Condition: ").AppendLine(Condition.ToString())
                .Append("Log file: ").AppendLine(LogFilePath)
                .Append("Output file: ").AppendLine(OutputFilePath)
                .Append("Error: ").AppendLine(Error.ToString())
                .Append("State: ").AppendLine(State.ToString())
                .Append("Processor time usage: ").AppendLine(ProcessorTimeUsage.ToString())
                .Append("Execution time: ").AppendLine(ExecutionTime.ToString());

            return sb.ToString();
        }
    }

    /// <summary>
    /// Reflects current process state throught lifecycle stages
    /// </summary>
    public enum ProcessState
    {
        /// <summary>
        /// Process does not begin transfering throught lifecycle stages, i.e. BuildStatus = WaitingToBuild
        /// </summary>
        Awaiting,

        /// <summary>
        /// Process is in one of the lifecycle stages, except BuildStatus = Complete
        /// </summary>
        InProgress,

        /// <summary>
        /// Happened if an error occured in one of the lifecycle stages. This state is compatible with any BuildStatus. If happened futher execution is terminated, show error to user
        /// </summary>
        Error,

        /// <summary>
        /// Happened then process successfully executed, i.e. no error occured throught lifecycle stages, output.txt exists
        /// </summary>
        Completed
    }
}
