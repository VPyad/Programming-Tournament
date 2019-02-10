using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProcessManagment.BuildSystem;
using ProcessManagment.Errors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace ProcessManagmentUnitTests
{
    [TestClass]
    public class SequentialBuildTests : IProcessStatusChanged
    {
        private ProcessManager processManager;

        // timeout scenario
        private ProcessResult processResultCSharp;
        private string idCSharp = "";
        private bool csharpExecutionCompleted = false;

        // compile error scenario
        private ProcessResult processResultCPP;
        private string idCPP = "";
        private bool cppExecutionCompleted = false;

        // success scenario
        private ProcessResult processResultJava;
        private string idJava = "";
        private bool javaExecutionCompleted = false;

        public SequentialBuildTests()
        {
            processManager = new ProcessManager(this);
        }

        public void StatusChanged(ProcessResult processResult)
        {
            if (processResult.Condition.Id == idCSharp && processResult.State == ProcessState.Error && processResult.Status == BuildStatus.Execution)
            {
                csharpExecutionCompleted = true;
                processResultCSharp = processResult;
                return;
            }

            if (processResult.Condition.Id == idCPP && processResult.State == ProcessState.Error && processResult.Status == BuildStatus.Building)
            {
                cppExecutionCompleted = true;
                processResultCPP = processResult;
                return;
            }

            if (processResult.Condition.Id == idJava && processResult.State == ProcessState.Completed && processResult.Status == BuildStatus.Complete)
            {
                javaExecutionCompleted = true;
                processResultJava = processResult;
                return;
            }
        }

        [TestCleanup]
        public void CleanUp()
        {
            processManager.RemoveProcessResult(idCSharp);
            processManager.RemoveProcessResult(idCPP);
            processManager.RemoveProcessResult(idJava);
        }

        [TestMethod]
        public void TestBuildingSeveralProcesses()
        {
            idCSharp = Guid.NewGuid().ToString();
            ProcessCondition csharpCondition = new ProcessCondition
            {
                Id = idCSharp,
                Language = SupportedLanguage.CSharp,
                WorkingDirPath = WorkingDirPathsHelper.CSharpTimeouted()
            };

            idCPP = Guid.NewGuid().ToString();
            ProcessCondition cppCondition = new ProcessCondition
            {
                Id = idCPP,
                Language = SupportedLanguage.CPP,
                WorkingDirPath = WorkingDirPathsHelper.CppCompileError()
            };

            idJava = Guid.NewGuid().ToString();
            ProcessCondition javaCondition = new ProcessCondition
            {
                Id = idJava,
                Language = SupportedLanguage.Java,
                WorkingDirPath = WorkingDirPathsHelper.JavaSuccess()
            };

            processManager.ProcessTask(csharpCondition);

            Thread.Sleep(1500);

            processManager.ProcessTask(cppCondition);
            processManager.ProcessTask(javaCondition);

            // use timer to guaranteed exit the loop
            Stopwatch stopwatch = new Stopwatch();
            int stopwathTimeout = 50; // 50 sec       

            while (stopwatch.Elapsed.TotalSeconds <= stopwathTimeout)
            {
                if (AllProcessCompleted())
                    break;
            }

            if (!AllProcessCompleted())
            {
                Assert.Fail("Execution stoped by timeout. Some or all procees did not completed. Execution stoped");
                CleanUp();
            }

            Assert.IsNotNull(processResultCSharp);
            Assert.AreEqual(idCSharp, processResultCSharp.Condition.Id);
            Assert.AreEqual(ProcessState.Error, processResultCSharp.State);
            Assert.AreEqual(BuildStatus.Execution, processResultCSharp.Status);
            Assert.IsInstanceOfType(processResultCSharp.Error, typeof(ProcessExecutionTimeouted));

            Assert.IsNotNull(processResultCPP);
            Assert.AreEqual(idCPP, processResultCPP.Condition.Id);
            Assert.AreEqual(ProcessState.Error, processResultCPP.State);
            Assert.AreEqual(BuildStatus.Building, processResultCPP.Status);
            Assert.IsInstanceOfType(processResultCPP.Error, typeof(BuildFailed));

            Assert.IsNotNull(processResultJava);
            Assert.AreEqual(idJava, processResultJava.Condition.Id);
            Assert.AreEqual(ProcessState.Completed, processResultJava.State);
            Assert.AreEqual(BuildStatus.Complete, processResultJava.Status);
        }

        private bool AllProcessCompleted()
        {
            return csharpExecutionCompleted && cppExecutionCompleted && javaExecutionCompleted;
        }
    }
}
