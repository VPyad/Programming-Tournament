using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessManagment.BuildSystem
{
    public class ProcessCondition
    {
        public SupportedLanguage Language { get; set; }

        public string WorkingDirPath { get; set; }

        public int? Timeout { get; set; } = null;

        public string Id { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Language: ").AppendLine(Language.ToString())
                .Append("Working dir path").AppendLine(WorkingDirPath)
                .Append("Id: ").AppendLine(Id);

            return sb.ToString();
        }
    }

    public enum SupportedLanguage
    {
        C,
        CPP,
        Java,
        CSharp,
        FreePascal,
        Delphi,
        ObjPascal
    }

    /// <summary>
    /// Reflects current state of process throught process lifecycle
    /// </summary>
    public enum BuildStatus
    {
        WaitingToBuild,
        BuildStarting,
        BuildSystemTest,
        Preparing,
        Building,
        ProcessingBuildingArtifacts,
        Execution,
        ProcessingExecutionArtifacts,
        Complete
    }
}
