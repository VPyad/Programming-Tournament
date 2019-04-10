using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Data.Managers
{
    public interface IStorageManager
    {
        string CreateInputFile(string tournamentId, string taskId);

        string CreateSrcFile(string workDir, string fileExt);

        string CreateExpectedFile(string tournamentId, string taskId);

        string GetWorkDir(string userId, string tournamentId, string taskId);

        void CopyInputFileToWorkDir(string pathToInput, string pathToWorkDir);

        string CreateInputFileInWorkDir(string workDir);

        bool CompareFiles(string exptectedFilePath, string outputFilePath);

        string GetSrcFilePath(string workDir);
    }
}
