using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace ProcessManagmentUnitTests
{
    internal class WorkingDirPathsHelper
    {
        public const string ROOT_PROJECTS_REPO_DIR = LAPTOP_ROOT_DIR;

        private const string LAPTOP_ROOT_DIR = @"D:\Repos\in-rush-sample-projects\";
        private const string WINDOWS_PC_ROOT_DIR = @"C:\Users\vpyad\Documents\Repos\in-rush-sample-projects-project-structure\";
        private const string UBUNTU_PC_ROOT_DIR = @"/home/vadim/Desktop/in-rush-sample-projects-project-structure/";

        private const string COMMON_TESTS_DIR = "general_tests";
        private const string NO_INPUT_DIR = "no_input";
        private const string NO_SRC_DIR = "no_src";
        private const string NO_OUTPUT_DIR = "no_output";

        public static string ComposePath(string rootPath, Lang lang, Scenario scenario)
        {
            StringBuilder sb = new StringBuilder();

            string separator = GetPathSeparator();

            sb.Append(rootPath)
                .Append(lang.ToDescriptionString())
                .Append(separator)
                .Append(scenario.ToDescriptionString());

            return sb.ToString();
        }

        public static string CppSuccess(string rootDir = ROOT_PROJECTS_REPO_DIR) => ComposePath(rootDir, Lang.CPP, Scenario.Success);
        public static string CppCompileError(string rootDir = ROOT_PROJECTS_REPO_DIR) => ComposePath(rootDir, Lang.CPP, Scenario.CompileError);
        public static string CppRuntimeError(string rootDir = ROOT_PROJECTS_REPO_DIR) => ComposePath(rootDir, Lang.CPP, Scenario.RuntimeError);
        public static string CppOutOfMemory(string rootDir = ROOT_PROJECTS_REPO_DIR) => ComposePath(rootDir, Lang.CPP, Scenario.OutOfMemory);
        public static string CppTimeouted(string rootDir = ROOT_PROJECTS_REPO_DIR) => ComposePath(rootDir, Lang.CPP, Scenario.Timeouted);
        public static string CppNoInput(string rootDir = ROOT_PROJECTS_REPO_DIR) => ComposePath(rootDir, Lang.CPP, Scenario.NoInput);
        public static string CppNoSrc(string rootDir = ROOT_PROJECTS_REPO_DIR) => ComposePath(rootDir, Lang.CPP, Scenario.NoSrc);

        public static string CSuccess(string rootDir = ROOT_PROJECTS_REPO_DIR) => ComposePath(rootDir, Lang.C, Scenario.Success);
        public static string CCompileError(string rootDir = ROOT_PROJECTS_REPO_DIR) => ComposePath(rootDir, Lang.C, Scenario.CompileError);
        public static string CRuntimeError(string rootDir = ROOT_PROJECTS_REPO_DIR) => ComposePath(rootDir, Lang.C, Scenario.RuntimeError);
        public static string COutOfMemory(string rootDir = ROOT_PROJECTS_REPO_DIR) => ComposePath(rootDir, Lang.C, Scenario.OutOfMemory);

        public static string JavaSuccess(string rootDir = ROOT_PROJECTS_REPO_DIR) => ComposePath(rootDir, Lang.Java, Scenario.Success);
        public static string JavaCompileError(string rootDir = ROOT_PROJECTS_REPO_DIR) => ComposePath(rootDir, Lang.Java, Scenario.CompileError);
        public static string JavaRuntimeError(string rootDir = ROOT_PROJECTS_REPO_DIR) => ComposePath(rootDir, Lang.Java, Scenario.RuntimeError);
        public static string JavaOutOfMemory(string rootDir = ROOT_PROJECTS_REPO_DIR) => ComposePath(rootDir, Lang.Java, Scenario.OutOfMemory);

        public static string CSharpSuccess(string rootDir = ROOT_PROJECTS_REPO_DIR) => ComposePath(rootDir, Lang.CSharp, Scenario.Success);
        public static string CSharpCompileError(string rootDir = ROOT_PROJECTS_REPO_DIR) => ComposePath(rootDir, Lang.CSharp, Scenario.CompileError);
        public static string CSharpRuntimeError(string rootDir = ROOT_PROJECTS_REPO_DIR) => ComposePath(rootDir, Lang.CSharp, Scenario.RuntimeError);
        public static string CSharpOutOfMemory(string rootDir = ROOT_PROJECTS_REPO_DIR) => ComposePath(rootDir, Lang.CSharp, Scenario.OutOfMemory);
        public static string CSharpTimeouted(string rootDir = ROOT_PROJECTS_REPO_DIR) => ComposePath(rootDir, Lang.CSharp, Scenario.Timeouted);

        public static string PascalSuccess(string rootDir = ROOT_PROJECTS_REPO_DIR) => ComposePath(rootDir, Lang.Pascal, Scenario.Success);
        public static string PascalCompileError(string rootDir = ROOT_PROJECTS_REPO_DIR) => ComposePath(rootDir, Lang.Pascal, Scenario.CompileError);
        public static string PascalRuntimeError(string rootDir = ROOT_PROJECTS_REPO_DIR) => ComposePath(rootDir, Lang.Pascal, Scenario.RuntimeError);

        public static string CommonNoInput(string rootDir = ROOT_PROJECTS_REPO_DIR)
        {
            StringBuilder sb = new StringBuilder();

            string separator = GetPathSeparator();

            sb.Append(rootDir)
                .Append(COMMON_TESTS_DIR)
                .Append(separator)
                .Append(NO_INPUT_DIR);

            return sb.ToString();
        }

        public static string CommonNoOutput(string rootDir = ROOT_PROJECTS_REPO_DIR)
        {
            StringBuilder sb = new StringBuilder();

            string separator = GetPathSeparator();

            sb.Append(rootDir)
                .Append(COMMON_TESTS_DIR)
                .Append(separator)
                .Append(NO_OUTPUT_DIR);

            return sb.ToString();
        }

        public static string CommonNoSrc(string rootDir = ROOT_PROJECTS_REPO_DIR)
        {
            StringBuilder sb = new StringBuilder();

            string separator = GetPathSeparator();

            sb.Append(rootDir)
                .Append(COMMON_TESTS_DIR)
                .Append(separator)
                .Append(NO_SRC_DIR);

            return sb.ToString();
        }

        private static string GetPathSeparator()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return @"\";
            else
                return "/";

        }
    }

    public enum Lang
    {
        [Description("c")]
        C,
        [Description("cpp")]
        CPP,
        [Description("pascal")]
        Pascal,
        [Description("java")]
        Java,
        [Description("csharp")]
        CSharp
    }

    public enum Scenario
    {
        [Description("success")]
        Success,
        [Description("out_of_memory")]
        OutOfMemory,
        [Description("runtime_error")]
        RuntimeError,
        [Description("compile_error")]
        CompileError,
        [Description("timeouted")]
        Timeouted,
        [Description("no_input")]
        NoInput,
        [Description("no_src")]
        NoSrc,
        [Description("existed_project")]
        CSharpExisted
    }

    public static class EnumDescExtension
    {
        public static string ToDescriptionString(this Lang val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string ToDescriptionString(this Scenario val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
