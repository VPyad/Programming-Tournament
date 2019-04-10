using ProcessManagment.BuildSystem.Processors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagment.BuildSystem
{
    public class ProcessManager
    {
        private bool Executing { get; set; } = false;

        private List<ProcessResult> globalProcessQueue;
        private List<ProcessResult> innerProcessQueue;

        private IProcessStatusChanged processStatusChanged;

        public ProcessManager(IProcessStatusChanged processStatusChanged)
        {
            this.processStatusChanged = processStatusChanged;
            globalProcessQueue = new List<ProcessResult>();
            innerProcessQueue = new List<ProcessResult>();
        }

        public ProcessResult RetrieveProcessResult(string conditionId) => FilesHelper.FindProcessResult(conditionId);

        public static ProcessResult GetProcessResult(string conditionId) => FilesHelper.FindProcessResult(conditionId);

        public void RemoveProcessResult(string conditionId) => FilesHelper.RemoveProcessResult(conditionId);

        public async Task ProcessTask(ProcessCondition processCondition)
        {
            ProcessResult processResult = new ProcessResult { Condition = processCondition };
            globalProcessQueue.Add(processResult);

            processStatusChanged.StatusChanged(processResult);
            FilesHelper.SaveProcessResult(processResult);

            if (!Executing)
                await Task.Factory.StartNew(() => ExecuteProcesses());
        }

        private void ExecuteProcesses()
        {
            Executing = true;

            foreach (var item in globalProcessQueue)
                innerProcessQueue.Add(item);

            globalProcessQueue.Clear();

            foreach (var process in innerProcessQueue)
            {
                var processor = GenerateProcessor(process.Condition);
                process.Status = BuildStatus.BuildStarting;
                processStatusChanged.StatusChanged(process);
                FilesHelper.UpdateProcessResult(process);

                var result = processor.Prepare(process);
                processStatusChanged.StatusChanged(result);
                if (result.State == ProcessState.Error)
                {
                    FilesHelper.UpdateProcessResult(process);
                    continue;
                }

                result = processor.TestBuildSystem(result);
                processStatusChanged.StatusChanged(result);
                if (result.State == ProcessState.Error)
                {
                    FilesHelper.UpdateProcessResult(process);
                    continue;
                }

                result = processor.Build(result);
                processStatusChanged.StatusChanged(result);
                if (result.State == ProcessState.Error)
                {
                    FilesHelper.UpdateProcessResult(process);
                    continue;
                }

                result = processor.ProcessBuildingArtifacts(result);
                processStatusChanged.StatusChanged(result);
                if (result.State == ProcessState.Error)
                {
                    FilesHelper.UpdateProcessResult(process);
                    continue;
                }

                result = processor.Execute(result);
                processStatusChanged.StatusChanged(result);
                if (result.State == ProcessState.Error)
                {
                    FilesHelper.UpdateProcessResult(process);
                    continue;
                }

                result = processor.ProcessExecutionArtifacts(result);
                processStatusChanged.StatusChanged(result);

                if (result.State == ProcessState.InProgress)
                {
                    result.Status = BuildStatus.Complete;
                    result.State = ProcessState.Completed;
                }

                processStatusChanged.StatusChanged(result);
                FilesHelper.UpdateProcessResult(process);
            }

            innerProcessQueue.Clear();
            Executing = false;

            if (globalProcessQueue.Count != 0)
                ExecuteProcesses();
        }

        private BuildProcessor GenerateProcessor(ProcessCondition processCondition)
        {
            switch (processCondition.Language)
            {
                case SupportedLanguage.CPP:
                    return new CPPBuildProcessor(processCondition.WorkingDirPath);
                case SupportedLanguage.C:
                    return new CBuildProcessor(processCondition.WorkingDirPath);
                case SupportedLanguage.Java:
                    return new JavaBuildProcessor(processCondition.WorkingDirPath);
                case SupportedLanguage.Delphi:
                    return new PascalBuildProcessor(processCondition.WorkingDirPath, PascalBuildProcessor.PascalCompiler.Delphi);
                case SupportedLanguage.ObjPascal:
                    return new PascalBuildProcessor(processCondition.WorkingDirPath, PascalBuildProcessor.PascalCompiler.ObjPascal);
                case SupportedLanguage.FreePascal:
                    return new PascalBuildProcessor(processCondition.WorkingDirPath, PascalBuildProcessor.PascalCompiler.FreePascal);
                case SupportedLanguage.CSharp:
                    return new CSharpBuildProcessor(processCondition.WorkingDirPath);
                default:
                    throw new Exception("Unsupported condition");
            }
        }
    }
}
