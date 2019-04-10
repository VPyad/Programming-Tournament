using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProcessManagment.BuildSystem;
using ProcessManagment.Errors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagmentUnitTests
{
    [TestClass]
    public class CSharpBuildTests : IProcessStatusChanged
    {
        private ProcessManager processManager;
        private ProcessResult actualProcessResult;
        private string id = "";

        private const SupportedLanguage LANG = SupportedLanguage.CSharp;
        private const string OUTPUT_FILE_CONTENT = "d3170a05-a505-4116-be1b-495fd6cdf525";

        public CSharpBuildTests()
        {
            processManager = new ProcessManager(this);
        }

        public async Task StatusChanged(ProcessResult processResult)
        {
            actualProcessResult = processResult;
        }

        [TestCleanup]
        public void CleanUp()
        {
            processManager.RemoveProcessResult(id);
        }

        [TestMethod]
        public async Task TestSuccessScenarioWithNotificator()
        {
            id = Guid.NewGuid().ToString();

            ProcessCondition processCondition = new ProcessCondition
            {
                Language = LANG,
                Id = id,
                WorkingDirPath = WorkingDirPathsHelper.CSharpSuccess()
            };

            await processManager.ProcessTask(processCondition);

            Assert.IsNotNull(actualProcessResult);
            Assert.AreEqual(id, actualProcessResult.Condition.Id);
            Assert.AreEqual(ProcessState.Completed, actualProcessResult.State);
            Assert.AreEqual(BuildStatus.Complete, actualProcessResult.Status);
        }

        [TestMethod]
        public async Task TestCompileErrorScenarioWithNotificator()
        {
            id = Guid.NewGuid().ToString();

            ProcessCondition processCondition = new ProcessCondition
            {
                Language = LANG,
                Id = id,
                WorkingDirPath = WorkingDirPathsHelper.CSharpCompileError()
            };

            await processManager.ProcessTask(processCondition);

            Assert.IsNotNull(actualProcessResult);
            Assert.AreEqual(id, actualProcessResult.Condition.Id);
            Assert.AreEqual(ProcessState.Error, actualProcessResult.State);
            Assert.AreEqual(BuildStatus.Building, actualProcessResult.Status);
            Assert.IsInstanceOfType(actualProcessResult.Error, typeof(BuildFailed));
        }

        [TestMethod]
        public async Task TestRuntimeErrorScenarioWithNotificator()
        {
            id = Guid.NewGuid().ToString();

            ProcessCondition processCondition = new ProcessCondition
            {
                Language = LANG,
                Id = id,
                WorkingDirPath = WorkingDirPathsHelper.CSharpRuntimeError()
            };

            await processManager.ProcessTask(processCondition);

            Assert.IsNotNull(actualProcessResult);
            Assert.AreEqual(id, actualProcessResult.Condition.Id);
            Assert.AreEqual(ProcessState.Error, actualProcessResult.State);
            Assert.AreEqual(BuildStatus.Execution, actualProcessResult.Status);
            Assert.IsInstanceOfType(actualProcessResult.Error, typeof(ExecutionFailed));
        }

        [TestMethod]
        public async Task TestTimeoutScenarioWithNotificator()
        {
            id = Guid.NewGuid().ToString();

            ProcessCondition processCondition = new ProcessCondition
            {
                Language = LANG,
                Id = id,
                WorkingDirPath = WorkingDirPathsHelper.CSharpTimeouted()
            };

            await processManager.ProcessTask(processCondition);

            Assert.IsNotNull(actualProcessResult);
            Assert.AreEqual(id, actualProcessResult.Condition.Id);
            Assert.AreEqual(ProcessState.Error, actualProcessResult.State);
            Assert.AreEqual(BuildStatus.Execution, actualProcessResult.Status);
            Assert.IsInstanceOfType(actualProcessResult.Error, typeof(ProcessExecutionTimeouted));
        }

        [TestMethod]
        public async Task TestOutOfMemoryScenarioWithNotificator()
        {
            id = Guid.NewGuid().ToString();

            ProcessCondition processCondition = new ProcessCondition
            {
                Language = LANG,
                Id = id,
                WorkingDirPath = WorkingDirPathsHelper.CSharpOutOfMemory()
            };

            await processManager.ProcessTask(processCondition);

            Assert.IsNotNull(actualProcessResult);
            Assert.AreEqual(id, actualProcessResult.Condition.Id);
            Assert.AreEqual(ProcessState.Error, actualProcessResult.State);
            Assert.AreEqual(BuildStatus.Execution, actualProcessResult.Status);

            // Depends on free memory CLR may throw OOF exception or may execute process till it exceeded timeout. Booth type of errors are correct
            bool errorCorrect = actualProcessResult.Error.GetType() == typeof(ProcessExecutionTimeouted)
                || actualProcessResult.Error.GetType() == typeof(ExecutionFailed);

            Assert.AreEqual(true, errorCorrect);
        }

        [TestMethod]
        public async Task TestCopyingSrcWithNotificator()
        {
            id = Guid.NewGuid().ToString();

            ProcessCondition processCondition = new ProcessCondition
            {
                Language = LANG,
                Id = id,
                WorkingDirPath = WorkingDirPathsHelper.CSharpSuccess()
            };

            await processManager.ProcessTask(processCondition);
            string outputContent = FilesHelper.ReadOutputFileContent(actualProcessResult?.OutputFilePath);

            Assert.IsNotNull(actualProcessResult);
            Assert.AreEqual(id, actualProcessResult.Condition.Id);
            Assert.AreEqual(ProcessState.Completed, actualProcessResult.State);
            Assert.AreEqual(BuildStatus.Complete, actualProcessResult.Status);
            Assert.AreEqual(OUTPUT_FILE_CONTENT, outputContent);
        }

        [TestMethod]
        public async Task TestSuccessScenarioWithRetriver()
        {
            id = Guid.NewGuid().ToString();

            ProcessCondition processCondition = new ProcessCondition
            {
                Language = LANG,
                Id = id,
                WorkingDirPath = WorkingDirPathsHelper.CSharpSuccess()
            };

            await processManager.ProcessTask(processCondition);

            ProcessResult processResult = processManager.RetrieveProcessResult(id);

            Assert.IsNotNull(processResult);
            Assert.AreEqual(id, processResult.Condition.Id);
            Assert.AreEqual(ProcessState.Completed, processResult.State);
            Assert.AreEqual(BuildStatus.Complete, processResult.Status);
        }

        [TestMethod]
        public async Task TestRuntimeErrorScenarioWithRetriever()
        {
            id = Guid.NewGuid().ToString();

            ProcessCondition processCondition = new ProcessCondition
            {
                Language = LANG,
                Id = id,
                WorkingDirPath = WorkingDirPathsHelper.CSharpRuntimeError()
            };

            await processManager.ProcessTask(processCondition);

            ProcessResult processResult = processManager.RetrieveProcessResult(id);

            Assert.IsNotNull(processResult);
            Assert.AreEqual(id, processResult.Condition.Id);
            Assert.AreEqual(ProcessState.Error, processResult.State);
            Assert.AreEqual(BuildStatus.Execution, processResult.Status);
            Assert.IsInstanceOfType(actualProcessResult.Error, typeof(ExecutionFailed));
        }
    }
}
