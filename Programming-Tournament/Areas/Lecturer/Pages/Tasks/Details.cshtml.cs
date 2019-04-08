using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Programming_Tournament.Data;
using Programming_Tournament.Data.Repositories.TournamentTasks;
using Programming_Tournament.Models.Domain.Tournaments;

namespace Programming_Tournament.Areas.Lecturer.Pages.Tasks
{
    [Authorize(Roles = "Admin,Lecturer")]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly TournamentTaskRepository taskRepository;
        
        [BindProperty]
        public TaskDetailModel ViewModel { get; set; }

        public DetailsModel(ApplicationDbContext context)
        {
            this.context = context;
            taskRepository = new TournamentTaskRepository(this.context);
        }

        public IActionResult OnGet(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var task = taskRepository.GetTask(id.Value);
            if (task == null)
                return NotFound();

            ViewModel = new TaskDetailModel
            {
                Id = task.TournamentTaskId,
                Desc = task.Desc,
                DueDate = task.DueDate,
                CreatedAt = task.CreatedAt,
                LecturerFullName = task.Owner.FirstName + " " + task.Owner.SecondName,
                Name = task.Name,
                InputFileSrc = task.InputFilePath,
                MaxAttempts = task.MaxAttempt,
                Langs = task.SupportedLanguages
            };

            List<AssigneesViewModel> assignees = new List<AssigneesViewModel>();

            if (task.Assignees != null)
                foreach (var item in task.Assignees)
                    assignees.Add(new AssigneesViewModel
                    {
                        Id = item.Id,
                        FirstName = item.User.FirstName,
                        SecondName = item.User.SecondName,
                        Passed = item.IsPassed,
                        LastAttemptAt = item.LastAttemptedAt
                    });

            if (task.SupportedLanguages == null)
                ViewModel.Langs = new List<SupportedProgrammingLanguage>();

            ViewModel.Students = assignees;

            return Page();
        }

        public IActionResult OnPostFileDownload(int? id, string filePath)
        {
            // TODO: remove id from request params
            if (!id.HasValue || string.IsNullOrEmpty(filePath))
                return NotFound();

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "text/plain", "input.txt");
        }
    }

    public class TaskDetailModel
    {
        public int Id { get; set; }

        [DisplayName("Created")]
        public DateTime CreatedAt { get; set; }

        [DisplayName("Lecturer name")]
        public string LecturerFullName { get; set; }

        [DisplayName("Input file")]
        public string InputFileSrc { get; set; }

        [DisplayName("End date")]
        public DateTime DueDate { get; set; }

        [DisplayName("Max attempts")]
        public int? MaxAttempts { get; set; }
        
        [DisplayName("Task name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string Desc { get; set; }

        [DisplayName("Available programming langs.")]
        public IEnumerable<SupportedProgrammingLanguage> Langs { get; set; }

        [DisplayName("Students")]
        public IEnumerable<AssigneesViewModel> Students { get; set; }

        public bool WasFileUploaded => !string.IsNullOrEmpty(InputFileSrc);
    }

    public class AssigneesViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string FullName => FirstName + " " + SecondName;

        public bool Passed { get; set; }

        public string PassedText => Passed ? "Yes" : "No";

        public DateTime LastAttemptAt { get; set; }

        public string LastAttemptAtText => LastAttemptAt == DateTime.MinValue ? "" : LastAttemptAt.ToLongDateString();
    }
}