using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Programming_Tournament.Data;
using Programming_Tournament.Data.Repositories.ApplicationUsers;
using Programming_Tournament.Data.Repositories.SupportedLanguages;
using Programming_Tournament.Data.Repositories.TournamentTasks;
using Programming_Tournament.Models.Domain.Tournaments;
using Programming_Tournament.Models.Domain.User;

namespace Programming_Tournament.Areas.Lecturer.Pages.Tasks
{
    [Authorize(Roles = "Admin,Lecturer")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly TournamentTaskRepository taskRepository;
        private readonly ApplicationUserRepository userRepository;
        private readonly SupportedLanguageRepository languageRepository;

        [BindProperty]
        public TaskEditViewModel ViewModel { get; set; }

        public EditModel(ApplicationDbContext context)
        {
            this.context = context;
            taskRepository = new TournamentTaskRepository(this.context);
            userRepository = new ApplicationUserRepository(this.context);
            languageRepository = new SupportedLanguageRepository(this.context);
        }

        public IActionResult OnGet(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var task = taskRepository.GetTask(id.Value);
            if (task == null)
                return NotFound();

            ViewModel = new TaskEditViewModel
            {
                Id = task.TournamentTaskId,
                Desc = task.Desc,
                DueDate = task.DueDate,
                CreatedAt = task.CreatedAt,
                LecturerName = task.Owner.FirstName + " " + task.Owner.SecondName,
                MaxAttempts = task.MaxAttempt,
                Name = task.Name
            };

            var tournamentStudents = userRepository.GetStudentsWithTournament(task.Tournament.TournamentId);
            var allLanguages = languageRepository.GetAll();
            var allStudents = new List<StudentViewModel>();

            if (tournamentStudents != null)
                foreach (var item in tournamentStudents)
                    allStudents.Add(new StudentViewModel { Id = item.Id, FirstName = item.FirstName, SecondName = item.SecondName });

            var studentsId = new List<string>();
            var languagesId = new List<int>();

            if (task.SupportedLanguages != null && task.SupportedLanguages.Count() != 0)
                foreach (var item in task.SupportedLanguages)
                    languagesId.Add(item.SupportedProgrammingLanguageId);

            if (task.Assignees != null && task.Assignees.Count() != 0)
                foreach (var item in task.Assignees)
                    studentsId.Add(item.User.Id);

            ViewModel.LanguagesSelectList = new MultiSelectList(allLanguages, "SupportedProgrammingLanguageId", "Name", languagesId);
            ViewModel.StudentsSelectList = new MultiSelectList(allStudents, "Id", "FullName", studentsId);

            return Page();
        }

        public IActionResult OnPostSave(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var task = taskRepository.GetTask(id.Value);
            if (task == null)
                return NotFound();

            var taskLanguages = task.SupportedLanguages;
            var allLanguages = languageRepository.GetAll();
            var taskLanguagesId = new List<int>();

            if (ViewModel.LanguagesId == null || taskLanguages == null || allLanguages == null)
                return OnGet(id);

            foreach (var item in taskLanguages)
                taskLanguagesId.Add(item.SupportedProgrammingLanguageId);

            var langsDifference = FindDifference(taskLanguagesId, ViewModel.LanguagesId);

            foreach (var item in langsDifference)
            {
                var lang = allLanguages.FirstOrDefault(x => x.SupportedProgrammingLanguageId == item);

                if (task.SupportedLanguages.Contains(lang))
                    task.SupportedLanguages.Remove(lang);
                else
                    task.SupportedLanguages.Add(lang);
            }

            taskRepository.Update(task);

            return OnGet(id);
        }

        private IEnumerable<int> FindDifference(IEnumerable<int> list1, IEnumerable<int> list2)
        {
            var list1Set = list1.ToHashSet();
            list1Set.SymmetricExceptWith(list2);
            var resultList = list1Set.ToList();

            return resultList;
        }
    }

    public class TaskEditViewModel
    {
        public int Id { get; set; }

        [DisplayName("Lecturer name")]
        public string LecturerName { get; set; }

        [DisplayName("Created")]
        public DateTime CreatedAt { get; set; }

        [DisplayName("Task name")]
        [Required]
        public string Name { get; set; }

        [DisplayName("End date")]
        [Required]
        public DateTime DueDate { get; set; }

        [DisplayName("Max attempts")]
        public int MaxAttempts { get; set; }

        [DisplayName("Available programming langs.")]
        public IEnumerable<SupportedProgrammingLanguage> SupportedLanguage { get; set; }

        [DisplayName("Students")]
        public IEnumerable<ApplicationUser> Students { get; set; }

        [DisplayName("Students")]
        public MultiSelectList StudentsSelectList { get; set; }

        [DisplayName("Available programming langs.")]
        public MultiSelectList LanguagesSelectList { get; set; }

        public IEnumerable<string> StudentsId { get; set; }

        public IEnumerable<int> LanguagesId { get; set; }

        public string InputFileSrc { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Desc { get; set; }

        [DisplayName("Select input file")]
        [Required, FileExtensions(Extensions = ".txt", ErrorMessage = "Incorrect file format")]
        public IFormFile InputFileUpload { get; set; }
    }

    public class StudentViewModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string FullName => FirstName + " " + SecondName;
    }
}