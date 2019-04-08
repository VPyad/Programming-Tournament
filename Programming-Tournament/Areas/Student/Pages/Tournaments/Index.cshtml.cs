using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Programming_Tournament.Data;
using Programming_Tournament.Data.Repositories.Tournaments;
using Programming_Tournament.Models.Domain.Tournaments;

namespace Programming_Tournament.Areas.Student.Pages.Tournaments
{
    [Authorize(Roles = "Admin,Lecturer,Student")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly TournamentRepository tournamentRepository;

        [BindProperty]
        public StudentTournamentIndexViewModel ViewModel { get; set; }

        public IndexModel(ApplicationDbContext context)
        {
            this.context = context;
            tournamentRepository = new TournamentRepository(this.context);
        }

        public IActionResult OnGet(TournamentSortState sortState = TournamentSortState.StatusAsc)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var tournaments = tournamentRepository.GetActiveStudentTournament(userId, sortState);

            List<StudentTournamentModel> studentTournaments = new List<StudentTournamentModel>();

            if (tournaments != null)
                foreach (var item in tournaments)
                    studentTournaments.Add(new StudentTournamentModel
                    {
                        Id = item.TournamentId,
                        Name = item.Name,
                        DueDate = item.DueDate,
                        LecturerFullName = item.Owner.FirstName + " " + item.Owner.SecondName,
                        Status = item.Status
                    });

            ViewModel = new StudentTournamentIndexViewModel
            {
                Tournaments = studentTournaments,
                SortViewModel = new TournamentIndexSortViewModel(sortState)
            };

            return Page();
        }
    }

    public class StudentTournamentIndexViewModel
    {
        public IEnumerable<StudentTournamentModel> Tournaments { get; set; }

        public TournamentIndexSortViewModel SortViewModel { get; set; }
    }

    public class StudentTournamentModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DueDate { get; set; }

        public string LecturerFullName { get; set; }

        public TournamentStatus Status { get; set; }
    }

    public class TournamentIndexSortViewModel
    {
        public TournamentSortState DueDate { get; private set; }
        public TournamentSortState Status { get; private set; }
        public TournamentSortState Current { get; private set; }

        public TournamentIndexSortViewModel(TournamentSortState sortState)
        {
            DueDate = sortState == TournamentSortState.DueDateAsc ? TournamentSortState.DueDateDesc : TournamentSortState.DueDateAsc;
            Status = sortState == TournamentSortState.StatusAsc ? TournamentSortState.StatusDesc : TournamentSortState.StatusAsc;

            Current = sortState;
        }
    }
}