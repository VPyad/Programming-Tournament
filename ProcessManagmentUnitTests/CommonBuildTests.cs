using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProcessManagment.BuildSystem;
using ProcessManagment.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private List<BuildStatus> buildStatuses;

        public CommonBuildTests()
        {
            processManager = new ProcessManager(this);
            buildStatuses = new List<BuildStatus>();
        }

        public void StatusChanged(ProcessResult processResult)
        {
            actualProcessResult = processResult;
            buildStatuses.Add(actualProcessResult.Status);
        }

        [TestCleanup]
        public void CleanUp()
        {
            processManager.RemoveProcessResult(id);
            buildStatuses.Clear();
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

        [TestMethod]
        public async Task TestDeletingWithNotificator()
        {
            FilesHelper.CreateJunkFiles(WorkingDirPathsHelper.CommonNoOutput());

            id = Guid.NewGuid().ToString();

            ProcessCondition processCondition = new ProcessCondition
            {
                Language = LANG,
                Id = id,
                WorkingDirPath = WorkingDirPathsHelper.CommonNoOutput()
            };

            await processManager.ProcessTask(processCondition);

            Assert.IsTrue(FilesHelper.JunkFilesDeleted(WorkingDirPathsHelper.CommonNoOutput()));
        }

        [TestMethod]
        public async Task TestStatusChangesWithNotificator()
        {
            id = Guid.NewGuid().ToString();

            ProcessCondition processCondition = new ProcessCondition
            {
                Language = LANG,
                Id = id,
                WorkingDirPath = WorkingDirPathsHelper.CppSuccess()
            };

            await processManager.ProcessTask(processCondition);

            List<bool> statuses = new List<bool>
            {
                buildStatuses[0] == BuildStatus.WaitingToBuild,
                buildStatuses[1] == BuildStatus.BuildStarting,
                buildStatuses[2] == BuildStatus.Preparing,
                buildStatuses[3] == BuildStatus.BuildSystemTest,
                buildStatuses[4] == BuildStatus.Building,
                buildStatuses[5] == BuildStatus.ProcessingBuildingArtifacts,
                buildStatuses[6] == BuildStatus.Execution,
                buildStatuses[7] == BuildStatus.ProcessingExecutionArtifacts,
                buildStatuses[8] == BuildStatus.Complete
            };

            Assert.IsNotNull(actualProcessResult);
            Assert.AreEqual(id, actualProcessResult.Condition.Id);
            Assert.AreEqual(ProcessState.Completed, actualProcessResult.State);
            Assert.AreEqual(BuildStatus.Complete, actualProcessResult.Status);
            Assert.AreEqual(true, statuses.All(x => x == true));
        }
    }
}
