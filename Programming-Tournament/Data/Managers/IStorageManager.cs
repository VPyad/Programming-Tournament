using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Data.Managers
{
    public interface IStorageManager
    {
        string CreateInputFile(string tournamentId, string taskId);

        string CreateSrcFile(string userId, string tournamentId, string taskId, string fileName);

        string GetWorkDir(string userId, string tournamentId, string taskId);

        string CopyInputFileToWorkDir(string pathToInput, string pathToWorkDir);


    }
}
