using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Data.Managers
{
    public class StorageManager : IStorageManager
    {
        private const string USERS_FILE_DIR = "UsersFiles";
        private const string TOURNAMENT_DIR = "Tournaments";
        private const string STUDENTS_DIR = "Students";

        private const string INPUT_FILE_NAME = "input.txt";
        private const string EXPECTED_FILE_NAME = "expected.txt";

        private string CreateDir(string path, string folderName) => Directory.CreateDirectory(Path.Combine(path, folderName)).FullName;

        private string CreateFile(string path, string fileName)
        {
            string filePath = Path.Combine(path, fileName);

            File.Create(filePath).Close();

            return filePath;
        }

        private string GetFilesDir() => CreateDir(AppContext.BaseDirectory, USERS_FILE_DIR);

        private string GetTournamentsDir() => CreateDir(GetFilesDir(), TOURNAMENT_DIR);

        private string GetStudentsDir() => CreateDir(GetFilesDir(), STUDENTS_DIR);

        public void CopyInputFileToWorkDir(string pathToInput, string pathToWorkDir) => File.Copy(pathToInput, pathToWorkDir, true);

        public string CreateInputFile(string tournamentId, string taskId)
        {
            string tournamentDirPath = CreateDir(GetTournamentsDir(), tournamentId);
            string taskDirPath = CreateDir(tournamentDirPath, taskId);

            string filePath = CreateFile(taskDirPath, INPUT_FILE_NAME);

            return filePath;
        }

        public string CreateExpectedFile(string tournamentId, string taskId)
        {
            string tournamentDirPath = CreateDir(GetTournamentsDir(), tournamentId);
            string taskDirPath = CreateDir(tournamentDirPath, taskId);

            string filePath = CreateFile(taskDirPath, EXPECTED_FILE_NAME);

            return filePath;
        }

        public string CreateSrcFile(string workDir, string fileExt)
        {
            if (fileExt.StartsWith('.'))
                fileExt = fileExt.Remove(0, 1);

            string path = CreateFile(workDir, "program." + fileExt);

            return path;
        }

        public string GetWorkDir(string userId, string tournamentId, string taskId)
        {
            string userDir = CreateDir(GetStudentsDir(), userId);
            string tournamentDir = CreateDir(userDir, tournamentId);
            string taskDir = CreateDir(tournamentDir, taskId);

            return taskDir;
        }

        public string CreateInputFileInWorkDir(string workDir) => CreateFile(workDir, INPUT_FILE_NAME);

        public bool CompareFiles(string exptectedFilePath, string outputFilePath)
        {
            bool isEqual = File.ReadAllLines(exptectedFilePath).SequenceEqual(File.ReadAllLines(outputFilePath));

            return isEqual;
        }
    }
}
