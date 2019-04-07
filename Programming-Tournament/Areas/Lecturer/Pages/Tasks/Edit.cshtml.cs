using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Programming_Tournament.Data;
using Programming_Tournament.Data.Managers;
using Programming_Tournament.Data.Repositories.ApplicationUsers;
using Programming_Tournament.Data.Repositories.SupportedLanguages;
using Programming_Tournament.Data.Repositories.TaskAssignments;
using Programming_Tournament.Data.Repositories.Tournaments;
using Programming_Tournament.Data.Repositories.TournamentTasks;
using Programming_Tournament.Models.Domain.Tournaments;
using Programming_Tournament.Models.Domain.User;
using Programming_Tournament.Utility.CustomValidators;

namespace Programming_Tournament.Areas.Lecturer.Pages.Tasks
{
    [Authorize(Roles = "Admin,Lecturer")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly TournamentTaskRepository taskRepository;
        private readonly ApplicationUserRepository userRepository;
        private readonly SupportedLanguageRepository languageRepository;
        private readonly TaskAssignmentRepository taskAssignmentRepository;

        [BindProperty]
        public TaskEditViewModel ViewModel { get; set; }

        public EditModel(ApplicationDbContext context)
        {
            this.context = context;
            taskRepository = new TournamentTaskRepository(this.context);
            userRepository = new ApplicationUserRepository(this.context);
            languageRepository = new SupportedLanguageRepository(this.context);
            taskAssignmentRepository = new TaskAssignmentRepository(this.context);
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
                Name = task.Name,
                InputFileSrc = task.InputFilePath
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

        public async Task<IActionResult> OnPostSave(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var task = taskRepository.GetTask(id.Value);
            if (task == null)
                return NotFound();

            if (string.IsNullOrEmpty(ViewModel.InputFileSrc) && ViewModel.InputFileUpload != null)
                ViewModel.InputFileSrc = ViewModel.InputFileUpload.ContentDisposition;

            if (!ModelState.IsValid)
                return OnGet(id);

            task.Name = ViewModel.Name;
            task.Desc = ViewModel.Desc;
            task.DueDate = ViewModel.DueDate;
            task.MaxAttempt = ViewModel.MaxAttempts;
            task.Desc = ViewModel.Desc;

            taskRepository.Update(task);

            // Change langs
            var taskLanguages = task.SupportedLanguages;
            var allLanguages = languageRepository.GetAll();

            if (ViewModel.LanguagesId != null && taskLanguages != null && allLanguages != null)
            {
                var taskLanguagesId = new List<int>();

                foreach (var item in taskLanguages)
                    taskLanguagesId.Add(item.SupportedProgrammingLanguageId);

                var langsDifference = FindLangDifference(taskLanguagesId, ViewModel.LanguagesId);

                foreach (var item in langsDifference)
                {
                    var lang = allLanguages.FirstOrDefault(x => x.SupportedProgrammingLanguageId == item);

                    if (task.SupportedLanguages.Contains(lang))
                        task.SupportedLanguages.Remove(lang);
                    else
                        task.SupportedLanguages.Add(lang);
                }

                taskRepository.Update(task);
            }

            // change students
            var taskStudents = task.Assignees;
            var tournamentStudents = userRepository.GetStudentsWithTournament(task.Tournament.TournamentId);

            if (ViewModel.StudentsId != null && taskStudents != null && tournamentStudents != null)
            {
                var taskStudentsId = new List<string>();

                foreach (var item in taskStudents)
                    taskStudentsId.Add(item.User.Id);

                var studentsDifference = FindStudentsDifference(taskStudentsId, ViewModel.StudentsId);

                foreach (var item in studentsDifference)
                {
                    var student = tournamentStudents.FirstOrDefault(x => x.Id == item);
                    var assignee = task.Assignees.FirstOrDefault(x => x.User == student);

                    if (assignee != null)
                        task.Assignees.Remove(assignee);
                    else
                    {
                        TournamentTaskAssignment assignment = new TournamentTaskAssignment
                        {
                            User = student,
                            Attempts = 0,
                            IsPassed = false,
                            Task = task
                        };

                        task.Assignees.Add(assignment);

                        taskRepository.Update(task);
                    }
                }
            }

            // upload file
            if (ViewModel.InputFileUpload != null)
            {
                if (ViewModel.InputFileUpload.Length != 0)
                {
                    StorageManager storageManager = new StorageManager();
                    TournamentRepository tournamentRepository = new TournamentRepository(context);

                    var tournament = tournamentRepository.GetTournamentByTask(task.TournamentTaskId);

                    if (tournament != null)
                    {
                        var inputFilePath = storageManager.CreateInputFile(tournament.TournamentId.ToString(), task.TournamentTaskId.ToString());
                        using (var stream = new FileStream(inputFilePath, FileMode.Create))
                        {
                            await ViewModel.InputFileUpload.CopyToAsync(stream);
                        }

                        task.InputFilePath = inputFilePath;
                        taskRepository.Update(task);
                    }
                }
            }

            return OnGet(id);
        }

        private IEnumerable<int> FindLangDifference(IEnumerable<int> list1, IEnumerable<int> list2)
        {
            var list1Set = list1.ToHashSet();
            list1Set.SymmetricExceptWith(list2);
            var resultList = list1Set.ToList();

            return resultList;
        }

        private IEnumerable<string> FindStudentsDifference(IEnumerable<string> list1, IEnumerable<string> list2)
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

        [DisplayName("Students")]
        public MultiSelectList StudentsSelectList { get; set; }

        [DisplayName("Available programming langs.")]
        public MultiSelectList LanguagesSelectList { get; set; }

        public IEnumerable<string> StudentsId { get; set; }

        public IEnumerable<int> LanguagesId { get; set; }

        [DisplayName("Select input file")]
        [Required]
        public string InputFileSrc { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Desc { get; set; }

        [DisplayName("Select input file")]
        [FileExtValidation("txt", "Incorrect file format", true)]
        public IFormFile InputFileUpload { get; set; }

        public string WasFileUploaded => string.IsNullOrEmpty(InputFileSrc) ? "No" : "Yes";
    }

    public class StudentViewModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string FullName => FirstName + " " + SecondName;
    }
}