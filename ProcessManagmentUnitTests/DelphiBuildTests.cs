﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProcessManagment.BuildSystem;
using ProcessManagment.Errors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagmentUnitTests
{
    [TestClass]
    public class DelphiBuildTests : IProcessStatusChanged
    {
        private ProcessManager processManager;
        private ProcessResult actualProcessResult;
        private string id = "";
        private const SupportedLanguage LANG = SupportedLanguage.Delphi;

        public DelphiBuildTests()
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
        public async Task TestSuccessScenarioWithNotificator()
        {
            id = Guid.NewGuid().ToString();

            ProcessCondition processCondition = new ProcessCondition
            {
                Language = LANG,
                Id = id,
                WorkingDirPath = WorkingDirPathsHelper.PascalSuccess()
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
                WorkingDirPath = WorkingDirPathsHelper.PascalCompileError()
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
                WorkingDirPath = WorkingDirPathsHelper.PascalRuntimeError()
            };

            await processManager.ProcessTask(processCondition);

            Assert.IsNotNull(actualProcessResult);
            Assert.AreEqual(id, actualProcessResult.Condition.Id);
            Assert.AreEqual(ProcessState.Error, actualProcessResult.State);
            Assert.AreEqual(BuildStatus.Execution, actualProcessResult.Status);
            Assert.IsInstanceOfType(actualProcessResult.Error, typeof(ExecutionFailed));
        }

        [TestMethod]
        public async Task TestSuccessScenarioWithRetriever()
        {
            id = Guid.NewGuid().ToString();

            ProcessCondition processCondition = new ProcessCondition
            {
                Language = LANG,
                Id = id,
                WorkingDirPath = WorkingDirPathsHelper.PascalSuccess()
            };

            await processManager.ProcessTask(processCondition);

            ProcessResult processResult = processManager.RetrieveProcessResult(id);

            Assert.IsNotNull(processResult);
            Assert.AreEqual(id, processResult.Condition.Id);
            Assert.AreEqual(ProcessState.Completed, processResult.State);
            Assert.AreEqual(BuildStatus.Complete, processResult.Status);
        }

        [TestMethod]
        public async Task TestCompileErrorScenarioWithRetriever()
        {
            id = Guid.NewGuid().ToString();

            ProcessCondition processCondition = new ProcessCondition
            {
                Language = LANG,
                Id = id,
                WorkingDirPath = WorkingDirPathsHelper.PascalCompileError()
            };

            await processManager.ProcessTask(processCondition);

            ProcessResult processResult = processManager.RetrieveProcessResult(id);

            Assert.IsNotNull(processResult);
            Assert.AreEqual(id, processResult.Condition.Id);
            Assert.AreEqual(ProcessState.Error, processResult.State);
            Assert.AreEqual(BuildStatus.Building, processResult.Status);
            Assert.IsInstanceOfType(processResult.Error, typeof(BuildFailed));
        }
    }
}
