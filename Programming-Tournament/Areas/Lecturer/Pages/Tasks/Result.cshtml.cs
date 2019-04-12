using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProcessManagment.BuildSystem;
using Programming_Tournament.Data;
using Programming_Tournament.Data.Managers;
using Programming_Tournament.Data.Repositories.SupportedLanguages;
using Programming_Tournament.Data.Repositories.TaskAssignments;
using Programming_Tournament.Helpers;
using Programming_Tournament.Models.Domain.Tournaments;
using Programming_Tournament.Resources;

namespace Programming_Tournament.Areas.Lecturer.Pages.Tasks
{
    [Authorize(Roles = "Admin,Lecturer")]
    public class ResultModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly TaskAssignmentRepository assignmentRepository;
        private readonly SupportedLanguageRepository languageRepository;
        
        private readonly StorageManager storageManager;
        private readonly ProcessResultHelper processResultHelper;

        public LecturerTaskResultViewModel ViewModel { get; set; }

        public ResultModel(ApplicationDbContext context, LocService locService)
        {
            this.context = context;
            assignmentRepository = new TaskAssignmentRepository(this.context);
            languageRepository = new SupportedLanguageRepository(this.context);

            storageManager = new StorageManager();
            processResultHelper = new ProcessResultHelper(locService);
        }

        public IActionResult OnGet(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var assignment = assignmentRepository.Get(id.Value);
            if (assignment == null)
                return NotFound();

            ViewModel = new LecturerTaskResultViewModel(assignment);
            ViewModel.SrcFilePath = storageManager.GetSrcFilePath(assignment.WorkDir);

            var result = ProcessManager.GetProcessResult(assignment.ProcessResultId);
            if (result != null)
            {
                ViewModel.OutputFilePath = result.OutputFilePath;
                ViewModel.LogFilePath = result.LogFilePath;
                ViewModel.LangName = GetLangName(result.Condition.Language);

                var resultsText = processResultHelper.GetResultsTexts(result);
                ViewModel.ResultText = resultsText.Item1;
                ViewModel.ErrorText = resultsText.Item2;
                ViewModel.ErrorDesc = resultsText.Item3;
            }

            return Page();
        }

        public IActionResult OnPostFileDownload(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return NotFound();

            var fileName = Path.GetFileName(filePath);

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "text/plain", fileName);
        }

        private string GetLangName(SupportedLanguage language)
        {
            var langs = languageRepository.GetAll();
            var lang = langs.FirstOrDefault(x => x.Code == language.ToString());

            if (lang == null)
                return "";

            return lang.Name;
        }
    }

    public class LecturerTaskResultViewModel
    {
        public LecturerTaskResultViewModel() { }

        public LecturerTaskResultViewModel(TournamentTaskAssignment taskAssignment)
        {
            Passed = taskAssignment.IsPassed;
            Attempts = taskAssignment.Attempts;
            LastAttempt = taskAssignment.LastAttemptedAt;
        }

        public bool Passed { get; set; } = false;

        [Display(Name = "Passed")]
        public string PassedText => Passed ? "Yes" : "No";

        [Display(Name = "Attempts")]
        public int Attempts { get; set; } = 0;

        public DateTime LastAttempt { get; set; }

        [Display(Name = "Last attempt")]
        public string LastAttemptText => LastAttempt == DateTime.MinValue ? "Never" : LastAttempt.ToLongDateString();

        public bool OutputFileAvailable => !string.IsNullOrEmpty(OutputFilePath);

        [Display(Name = "Output file")]
        public string OutputFilePath { get; set; }

        public bool SrcFileAvailable => !string.IsNullOrEmpty(SrcFilePath);

        [Display(Name = "Src file")]
        public string SrcFilePath { get; set; }

        public bool LogFileAvailable => !string.IsNullOrEmpty(LogFilePath);

        [Display(Name = "Log file")]
        public string LogFilePath { get; set; }

        [Display(Name = "Result")]
        [DataType(DataType.MultilineText)]
        public string ResultText { get; set; }

        [Display(Name = "Error")]
        public string ErrorText { get; set; }

        [Display(Name = "Error description")]
        [DataType(DataType.MultilineText)]
        public string ErrorDesc { get; set; }

        [Display(Name = "Language")]
        public string LangName { get; set; }
    }
}