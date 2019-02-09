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
