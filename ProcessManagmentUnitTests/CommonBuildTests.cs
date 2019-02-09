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
    public class CommonBuildTests : IProcessStatusChanged
    {
        private ProcessManager processManager;
        private ProcessResult actualProcessResult;
        private string id = "";
        private const SupportedLanguage LANG = SupportedLanguage.CPP;

        public CommonBuildTests()
        {
            processManager = new ProcessManager(this);
        }

        public void StatusChanged(ProcessResult processResult)
        {
            actualProcessResult = processResult;
        }

        [TestCleanup]
        public void CleanUp()
        {
            processManager.RemoveProcessResult(id);
        }

        [TestMethod]
        public async Task TestNoInputWithNotificator()
        {
            id = Guid.NewGuid().ToString();

            ProcessCondition processCondition = new ProcessCondition
            {
                Language = LANG,
                Id = id,
                WorkingDirPath = WorkingDirPathsHelper.CommonNoInput()
            };

            await processManager.ProcessTask(processCondition);

            Assert.IsNotNull(actualProcessResult);
            Assert.AreEqual(id, actualProcessResult.Condition.Id);
            Assert.AreEqual(ProcessState.Error, actualProcessResult.State);
            Assert.AreEqual(BuildStatus.Preparing, actualProcessResult.Status);
            Assert.IsInstanceOfType(actualProcessResult.Error, typeof(InputFilesNotFound));
            Assert.AreEqual(InputFilesNotFound.InputFile.Input, (actualProcessResult.Error as InputFilesNotFound).FileType);
        }

        [TestMethod]
        public async Task TestNoOutputWithNotificator()
        {
            id = Guid.NewGuid().ToString();

            ProcessCondition processCondition = new ProcessCondition
            {
                Language = LANG,
                Id = id,
                WorkingDirPath = WorkingDirPathsHelper.CommonNoOutput()
            };

            await processManager.ProcessTask(processCondition);

            Assert.IsNotNull(actualProcessResult);
            Assert.AreEqual(id, actualProcessResult.Condition.Id);
            Assert.AreEqual(ProcessState.Error, actualProcessResult.State);
            Assert.AreEqual(BuildStatus.ProcessingExecutionArtifacts, actualProcessResult.Status);
            Assert.IsInstanceOfType(actualProcessResult.Error, typeof(OutputFileNotFound));
        }

        [TestMethod]
        public async Task TestNoSrcWithNotificator()
        {
            id = Guid.NewGuid().ToString();

            ProcessCondition processCondition = new ProcessCondition
            {
                Language = LANG,
                Id = id,
                WorkingDirPath = WorkingDirPathsHelper.CommonNoSrc()
            };

            await processManager.ProcessTask(processCondition);

            Assert.IsNotNull(actualProcessResult);
            Assert.AreEqual(id, actualProcessResult.Condition.Id);
            Assert.AreEqual(ProcessState.Error, actualProcessResult.State);
            Assert.AreEqual(BuildStatus.Preparing, actualProcessResult.Status);
            Assert.IsInstanceOfType(actualProcessResult.Error, typeof(InputFilesNotFound));
            Assert.AreEqual(InputFilesNotFound.InputFile.Src, (actualProcessResult.Error as InputFilesNotFound).FileType);
        }

        [TestMethod]
        public async Task TestNoWorkDirNotSpecifiedWithNotificator()
        {
            id = Guid.NewGuid().ToString();

            ProcessCondition processCondition = new ProcessCondition
            {
                Language = LANG,
                Id = id,
                WorkingDirPath = ""
            };

            await processManager.ProcessTask(processCondition);

            Assert.IsNotNull(actualProcessResult);
            Assert.AreEqual(id, actualProcessResult.Condition.Id);
            Assert.AreEqual(ProcessState.Error, actualProcessResult.State);
            Assert.AreEqual(BuildStatus.Preparing, actualProcessResult.Status);
            Assert.IsInstanceOfType(actualProcessResult.Error, typeof(WorkingDirNotSpecified));
        }
    }
}
