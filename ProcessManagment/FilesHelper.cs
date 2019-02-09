using LiteDB;
using ProcessManagment.BuildSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ProcessManagment
{
    internal class FilesHelper
    {
        private const string TEMP_FOLDER = "Temp";
        private const string DATABASE_NAME = "LocalProcessResults.db";
        private const string TABLE_NAME = "ProcessResults";

        internal static string GetTempFolder() => Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, TEMP_FOLDER)).FullName;

        internal static string GetPathSeparator()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return @"\";
            else
                return "/";

        }

        internal static string AddPathSeparator(string input)
        {
            if (EndsWithSeparator(input))
                return input;

            return input + GetPathSeparator();
        }

        internal static bool EndsWithSeparator(string input)
        {
            if (input.EndsWith("/") || input.EndsWith(@"\"))
                return true;
            else
                return false;

        }

        internal static bool FileExists(string rootPath, string fileName)
        {
            return File.Exists(AddPathSeparator(rootPath) + fileName);
        }

        internal static bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="expetpFiles">files name with ext. that must be kept</param>
        /// <returns></returns>
        internal static void DeleteFilesExcept(string dirPath, IEnumerable<string> exceptFiles)
        {
            var files = Directory.GetFiles(dirPath);

            foreach (var file in files)
            {
                var name = new FileInfo(file).Name;
                name = name.ToLower();
                if (!exceptFiles.Contains(name))
                    File.Delete(file);
            }
        }

        internal static void MoveFile(string sourcePath, string targetPath, string fileName, string udidName)
        {
            var sourceFile = Path.Combine(sourcePath, fileName);
            var targetFile = Path.Combine(targetPath, udidName);

            File.Move(sourceFile, targetFile);
        }

        internal static void ReplaceFile(string sourcePath, string targetPath, string fileName, string udidName)
        {
            var sourceFile = Path.Combine(sourcePath, fileName);
            var targetFile = Path.Combine(targetPath, udidName);

            File.Replace(sourceFile, targetFile, "program", true);
        }

        internal static void DeleteFile(string path, string fileName)
        {
            path = Path.Combine(path, fileName);

            File.Delete(path);
        }

        internal static void MoveToTemp(string sourcePath, string fileName, string udidName) => MoveFile(sourcePath, GetTempFolder(), fileName, udidName);

        internal static void MoveFromTemp(string targetPath, string udidName, string fileName)
        {
            DeleteFile(targetPath, fileName);
            MoveFile(GetTempFolder(), targetPath, udidName, fileName);
        }

        internal static void SaveProcessResult(ProcessResult processResult)
        {
            using (var db = new LiteDatabase(DATABASE_NAME))
            {
                var processResults = db.GetCollection<ProcessResult>(TABLE_NAME);

                processResults.Insert(processResult);
            }
        }

        internal static ProcessResult FindProcessResult(string conditionId)
        {
            if (string.IsNullOrEmpty(conditionId))
                return null;

            ProcessResult processResult;

            using (var db = new LiteDatabase(DATABASE_NAME))
            {
                var processResults = db.GetCollection<ProcessResult>(TABLE_NAME);

                processResult = processResults.Find(x => x.Condition.Id == conditionId).FirstOrDefault();
            }

            return processResult;
        }

        internal static void RemoveProcessResult(string conditionId)
        {
            if (string.IsNullOrEmpty(conditionId))
                return;

            using (var db = new LiteDatabase(DATABASE_NAME))
            {
                var processResults = db.GetCollection<ProcessResult>(TABLE_NAME);

                processResults.Delete(x => x.Condition.Id == conditionId);
            }
        }

        internal static void UpdateProcessResult(ProcessResult processResult)
        {
            if (processResult == null)
                return;

            var existedProcessResult = FindProcessResult(processResult.Condition.Id);

            if (existedProcessResult == null)
                return;

            processResult.Id = existedProcessResult.Id;

            using (var db = new LiteDatabase(DATABASE_NAME))
            {
                var processResults = db.GetCollection<ProcessResult>(TABLE_NAME);

                processResults.Update(processResult);
            }
        }

        internal static int CountProcessResults()
        {
            int count = 0;

            using (var db = new LiteDatabase(DATABASE_NAME))
            {
                var processResults = db.GetCollection<ProcessResult>(TABLE_NAME);

                count = processResults.Count();
            }

            return count;
        }
    }
}
