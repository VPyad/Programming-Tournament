using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Programming_Tournament.Data;
using Programming_Tournament.Data.Repositories.Tournaments;
using Programming_Tournament.Data.Repositories.TournamentTasks;
using Programming_Tournament.Models.Domain.Tournaments;

namespace Programming_Tournament.Areas.Student.Pages.Tournaments
{
    [Authorize(Roles = "Admin,Lecturer,Student")]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly TournamentRepository tournamentRepository;

        [BindProperty]
        public StudentTournamentEditViewModel ViewModel { get; set; }

        public DetailsModel(ApplicationDbContext context)
        {
            this.context = context;
            tournamentRepository = new TournamentRepository(this.context);
        }

        public IActionResult OnGet(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var tournament = tournamentRepository.GetTournament(id.Value);
            if (tournament == null)
                return NotFound();

            ViewModel = new StudentTournamentEditViewModel
            {
                Name = tournament.Name,
                Desc = tournament.Desc,
                Status = tournament.Status,
                DueDate = tournament.DueDate,
                CreatedAt = tournament.CreatedAt,
                LecturerFullName = tournament.Owner.FirstName + " " + tournament.Owner.SecondName
            };

            List<StudentTaskModel> tasks = new List<StudentTaskModel>();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (tournament.Tasks != null)
                foreach (var item in tournament.Tasks)
                    if (item.Assignees.FirstOrDefault(x => x.User.Id == userId) != null)
                        tasks.Add(new StudentTaskModel
                        {
                            Id = item.TournamentTaskId,
                            Name = item.Name,
                            DueTo = item.DueDate,
                            MaxAttempts = item.MaxAttempt
                        });

            ViewModel.Tasks = tasks;

            return Page();
        }
    }

    public class StudentTournamentEditViewModel
    {
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Lecturer")]
        public string LecturerFullName { get; set; }

        [Display(Name = "Status")]
        public TournamentStatus Status { get; set; }

        [Display(Name = "End date")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Created at")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Desc { get; set; }

        public IEnumerable<StudentTaskModel> Tasks { get; set; }
    }

    public class StudentTaskModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DueTo { get; set; }

        public int? MaxAttempts { get; set; }

        public string MaxAttemptsText => MaxAttempts.HasValue && MaxAttempts.Value != 0 ? MaxAttempts.Value.ToString() : "Unlimited";
    }
}