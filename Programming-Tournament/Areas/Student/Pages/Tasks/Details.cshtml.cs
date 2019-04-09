using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProcessManagment.BuildSystem;
using Programming_Tournament.Data;
using Programming_Tournament.Data.Managers;
using Programming_Tournament.Data.Repositories.Tournaments;
using Programming_Tournament.Data.Repositories.TournamentTasks;
using Programming_Tournament.Models.Domain.Tournaments;
using Programming_Tournament.Utility.CustomValidators;

namespace Programming_Tournament.Areas.Student.Pages.Tasks
{
    [Authorize(Roles = "Admin,Lecturer,Student")]
    public class DetailsModel : PageModel, IProcessStatusChanged
    {
        private readonly ApplicationDbContext context;
        private readonly TournamentTaskRepository taskRepository;
        private readonly TournamentRepository tournamentRepository;
        private readonly ProcessManager processManager;
        private readonly StorageManager storageManager;

        public StudentTaskDetailsViewModel DetailsViewModel { get; set; }

        [BindProperty]
        public StudentTaskInputViewModel InputViewModel { get; set; }

        public DetailsModel(ApplicationDbContext context)
        {
            this.context = context;
            taskRepository = new TournamentTaskRepository(this.context);
            tournamentRepository = new TournamentRepository(this.context);
            processManager = new ProcessManager(this);
            storageManager = new StorageManager();
        }

        public IActionResult OnGet(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var task = taskRepository.GetTask(id.Value);
            if (task == null)
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            DetailsViewModel = new StudentTaskDetailsViewModel(task, userId);
            InputViewModel = new StudentTaskInputViewModel(task.SupportedLanguages);

            return Page();
        }

        public async Task<IActionResult> OnPostSubmit(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var task = taskRepository.GetTask(id.Value);
            if (task == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                var selectedFileExt = InputViewModel.SrcFilePath.Split('.').Last();
                var validSrcExt = GetFileExt(InputViewModel.LangCode);

                if (validSrcExt != selectedFileExt)
                {
                    ModelState.AddModelError("", "Select file with correct extension!");
                    return OnGet(id);
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var assignee = task.Assignees.FirstOrDefault(x => x.User.Id == userId);
                string workDir = "";

                // upload src to user work dir
                if (!string.IsNullOrEmpty(assignee.WorkDir))
                    workDir = assignee.WorkDir;
                else
                {
                    var tournament = tournamentRepository.GetTournamentByTask(task.TournamentTaskId);
                    workDir = storageManager.GetWorkDir(userId, tournament.TournamentId.ToString(), task.TournamentTaskId.ToString());

                    assignee.WorkDir = workDir;
                    taskRepository.Update(task);
                }

                var srcFilePath = storageManager.CreateSrcFile(workDir, validSrcExt);
                using (var stream = new FileStream(srcFilePath, FileMode.Create))
                {
                    await InputViewModel.SrcFile.CopyToAsync(stream);
                }

                var inputFilePath = storageManager.CreateInputFileInWorkDir(workDir);
                storageManager.CopyInputFileToWorkDir(task.InputFilePath, inputFilePath);

                var lang = SupportedProgrammingLanguage.Map(InputViewModel.LangCode, out bool langParseSuccess);

                if (!langParseSuccess)
                {
                    ModelState.AddModelError("", "Smth went wrong");
                    return OnGet(id);
                }

                string processConditionId = Guid.NewGuid().ToString();
                assignee.ProcessResultId = processConditionId;
                taskRepository.Update(task);

                ProcessCondition processCondition = new ProcessCondition
                {
                    Id = processConditionId,
                    WorkingDirPath = workDir,
                    Language = lang
                };

                processManager.ProcessTask(processCondition);
            }

            Debug.WriteLine("!!!EXITED ON_POST!!!");
            return OnGet(id);
        }

        public IActionResult OnPostFileDownload(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return NotFound();

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "text/plain", "input.txt");
        }

        public void StatusChanged(ProcessResult processResult)
        {
            Debug.WriteLine("!!!STATUS CHANGED!!!");
            Debug.WriteLine("State:");
            Debug.WriteLine(processResult.State);
            Debug.WriteLine("Status");
            Debug.WriteLine(processResult.Status);
        }

        private string GetFileExt(string code)
        {
            string ext = "";

            switch (code)
            {
                case "C":
                    ext = "c";
                    break;
                case "CPP":
                    ext = "cpp";
                    break;
                case "Java":
                    ext = "java";
                    break;
                case "CSharp":
                    ext = "cs";
                    break;
                case "FreePascal":
                    ext = "pas";
                    break;
                case "Delphi":
                    ext = "pas";
                    break;
                case "ObjPascal":
                    ext = "pas";
                    break;
            }

            return ext;
        }
    }

    public class StudentTaskDetailsViewModel
    {
        public StudentTaskDetailsViewModel() { }

        public StudentTaskDetailsViewModel(TournamentTask task) => InitBaseFields(task);

        public StudentTaskDetailsViewModel(TournamentTask task, string userId)
        {
            InitBaseFields(task);

            if (task.Assignees != null)
            {
                var assignee = task.Assignees.FirstOrDefault(x => x.User.Id == userId);
                if (assignee != null)
                {
                    Passed = assignee.IsPassed;
                    Attempts = assignee.Attempts;
                    LastAttempt = assignee.LastAttemptedAt;

                    if (assignee.IsPassed)
                    {
                        // TODO: retrieve task data from processManager
                    }
                }
            }

        }

        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Lecturer")]
        public string LecturerFullName { get; set; }

        [Display(Name = "End date")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Created at")]
        public DateTime CreatedAt { get; set; }

        public int? MaxAttempts { get; set; }

        [Display(Name = "Max attempts")]
        public string MaxAttemptsText => MaxAttempts.HasValue && MaxAttempts.Value != 0 ? MaxAttempts.Value.ToString() : "Unlimited";

        [Display(Name = "Input file")]
        public string InputFilePath { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Desc { get; set; }

        [Display(Name = "Available programming langs.")]
        public IEnumerable<SupportedProgrammingLanguage> Langs { get; set; }

        public bool Passed { get; set; } = false;

        [Display(Name = "Did you passed")]
        public string PassedText => Passed ? "Yes" : "No";

        [Display(Name = "Attempts")]
        public int Attempts { get; set; } = 0;

        public DateTime LastAttempt { get; set; }

        [Display(Name = "Last attempt")]
        public string LastAttemptText => LastAttempt == DateTime.MinValue ? "Never" : LastAttempt.ToLongDateString();

        public bool OutputFileAvailable { get; set; } = false;

        [Display(Name = "Output file")]
        public string OutputFilePath { get; set; }

        public bool SrcFileAvailable { get; set; } = false;

        [Display(Name = "Src file")]
        public string SrcFilePath { get; set; }

        public bool LogFileAvailable { get; set; } = false;

        [Display(Name = "Log file")]
        public string LogFilePath { get; set; }

        private void InitBaseFields(TournamentTask task)
        {
            Id = task.TournamentTaskId;
            Name = task.Name;
            LecturerFullName = task.Owner.FirstName + " " + task.Owner.SecondName;
            Desc = task.Desc;
            DueDate = task.DueDate;
            CreatedAt = task.CreatedAt;
            MaxAttempts = task.MaxAttempt;
            InputFilePath = task.InputFilePath;
            Langs = task.SupportedLanguages;
        }
    }

    public class StudentTaskInputViewModel
    {
        public StudentTaskInputViewModel() { }

        public StudentTaskInputViewModel(IEnumerable<SupportedProgrammingLanguage> langs)
        {
            if (langs != null)
            {
                List<SelectListItem> list = new List<SelectListItem>();
                foreach (var item in langs)
                    list.Add(new SelectListItem(item.Name, item.Code));

                if (list.Any())
                    list[0].Selected = true;

                LanguageSelectList = list;
            }
        }

        [DisplayName("Select programming langs.")]
        public ICollection<SelectListItem> LanguageSelectList { get; set; }

        [Required]
        public string SrcFilePath { get; set; }

        public string LangCode { get; set; }

        [DisplayName("Select src file")]
        public IFormFile SrcFile { get; set; }

        public string Test { get; set; }
    }
}