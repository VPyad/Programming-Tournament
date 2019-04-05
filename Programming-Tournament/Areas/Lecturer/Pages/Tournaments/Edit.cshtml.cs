using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Programming_Tournament.Data;
using Programming_Tournament.Data.Repositories.ApplicationUsers;
using Programming_Tournament.Data.Repositories.Tournaments;
using Programming_Tournament.Data.Repositories.TournamentTasks;
using Programming_Tournament.Models.Domain.Tournaments;
using Programming_Tournament.Models.Domain.User;

namespace Programming_Tournament.Areas.Lecturer.Pages.Tournaments
{
    [Authorize(Roles = "Admin,Lecturer")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly TournamentRepository tournamentRepository;
        private readonly ApplicationUserRepository userRepository;
        private readonly TournamentTaskRepository taskRepository;

        [BindProperty]
        public TournamentEditViewModel ViewModel { get; set; }

        public EditModel(ApplicationDbContext context)
        {
            this.context = context;
            tournamentRepository = new TournamentRepository(this.context);
            userRepository = new ApplicationUserRepository(this.context);
            taskRepository = new TournamentTaskRepository(this.context);
        }

        public IActionResult OnGet(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var tournament = tournamentRepository.GetTournament(id.Value);
            if (tournament == null)
                return NotFound();

            ViewModel = new TournamentEditViewModel
            {
                Id = tournament.TournamentId,
                Name = tournament.Name,
                Desc = tournament.Desc,
                LecturerName = tournament.Owner.FirstName + " " + tournament.Owner.SecondName,
                CreatedAt = tournament.CreatedAt,
                DueDate = tournament.DueDate,
                Status = tournament.Status
            };

            var students = userRepository.GetStudentsWithTournament(tournament.TournamentId);
            var tasks = taskRepository.GetTasksWithToutnament(id.Value);

            ViewModel.Students = students;
            ViewModel.Tasks = tasks;

            return Page();
        }

        public IActionResult OnPostSave(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var tournament = tournamentRepository.Get(id.Value);

            if (tournament == null)
                return NotFound();

            if (tournament.Name != ViewModel.Name)
                tournament.Name = ViewModel.Name;

            if (tournament.DueDate != ViewModel.DueDate)
                tournament.DueDate = ViewModel.DueDate;

            if (tournament.Desc != ViewModel.Desc)
                tournament.Desc = ViewModel.Desc;

            tournamentRepository.Update(tournament);

            return OnGet(id);
        }

        public IActionResult OnPostStart(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var tournament = tournamentRepository.Get(id.Value);

            if (tournament == null)
                return NotFound();

            tournament.Status = TournamentStatus.Active;

            tournamentRepository.Update(tournament);

            return OnGet(id);
        }

        public IActionResult OnPostFinish(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var tournament = tournamentRepository.Get(id.Value);

            if (tournament == null)
                return NotFound();

            tournament.Status = TournamentStatus.Completed;

            tournamentRepository.Update(tournament);

            return OnGet(id);
        }

        public IActionResult OnPostDeactivate(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var tournament = tournamentRepository.Get(id.Value);

            if (tournament == null)
                return NotFound();

            tournament.Status = TournamentStatus.Inactive;

            tournamentRepository.Update(tournament);

            return OnGet(id);
        }

        public IActionResult OnPostDraft(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var tournament = tournamentRepository.Get(id.Value);

            if (tournament == null)
                return NotFound();

            tournament.Status = TournamentStatus.Draft;

            tournamentRepository.Update(tournament);

            return OnGet(id);
        }

        public IActionResult OnPostAddTask(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var tournament = tournamentRepository.GetTournament(id.Value);
            if (tournament == null)
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = userRepository.Get(userId);
            if (user == null)
                return NotFound();

            TournamentTask task = new TournamentTask
            {
                Tournament = tournament,
                DueDate = tournament.DueDate,
                Owner = user,
                Name = "Draft " + DateTime.Now.ToShortDateString(),
                CreatedAt = DateTime.Now,
            };

            taskRepository.Add(task);

            return LocalRedirect($"/Lecturer/Tasks/Edit/{task.TournamentTaskId}");
        }
    }

    public class TournamentEditViewModel
    {
        public int Id { get; set; }

        [DisplayName("Tournament name")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string Desc { get; set; }

        [DisplayName("Lecturer name")]
        public string LecturerName { get; set; }

        [DisplayName("End date")]
        [Required]
        public DateTime DueDate { get; set; }

        [DisplayName("Created")]
        public DateTime CreatedAt { get; set; }

        [DisplayName("Current status")]
        public TournamentStatus Status { get; set; }

        [DisplayName("Tasks")]
        public IEnumerable<TournamentTask> Tasks { get; set; }

        [DisplayName("Students")]
        public IEnumerable<ApplicationUser> Students { get; set; }
    }
}