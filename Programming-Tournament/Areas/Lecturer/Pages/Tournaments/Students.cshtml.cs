using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Programming_Tournament.Areas.Admin.Pages.Users;
using Programming_Tournament.Data;
using Programming_Tournament.Data.Repositories.ApplicationUsers;
using Programming_Tournament.Data.Repositories.Tournaments;
using Programming_Tournament.Models.Domain.User;

namespace Programming_Tournament.Areas.Lecturer.Pages.Tournaments
{
    [Authorize(Roles = "Admin,Lecturer")]
    public class StudentsModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly TournamentRepository tournamentRepository;
        private readonly ApplicationUserRepository userRepository;

        [BindProperty]
        public StudentsViewModel ViewModel { get; set; }

        public StudentsModel(ApplicationDbContext context)
        {
            this.context = context;
            this.tournamentRepository = new TournamentRepository(this.context);
            this.userRepository = new ApplicationUserRepository(this.context);
        }

        public IActionResult OnGet(int? id, int p = 1, StudentSortState sortState = StudentSortState.FacultyNameAsc)
        {
            if (!id.HasValue)
                return NotFound();

            var tournament = tournamentRepository.GetTournament(id.Value);
            if (tournament == null)
                return NotFound();

            int size = 25;

            // TODO: Stubs!!
            var students = new List<ApplicationUser>();

            ViewModel = new StudentsViewModel
            {
                TournamentId = tournament.TournamentId,
                TournamentName = tournament.Name,
                SortViewModel = new StudentsSortViewModel(sortState),
                Students = students,
                PageViewModel = new PageViewModel(students.Count(), p, size)
            };

            return Page();
        }

        public IActionResult OnPostAdd(int? id, int p = 1, StudentSortState sortState = StudentSortState.FacultyNameAsc)
        {
            return OnGet(id, p, sortState);
        }

        public IActionResult OnPostDone(int? id, int p = 1, StudentSortState sortState = StudentSortState.FacultyNameAsc)
        {
            return OnGet(id, p, sortState);
        }
    }

    public class StudentsViewModel
    {
        public string TournamentName { get; set; }

        public int TournamentId { get; set; }

        public IEnumerable<ApplicationUser> Students { get; set; }

        public StudentsSortViewModel SortViewModel { get; set; }

        public PageViewModel PageViewModel { get; set; }
    }

    public class StudentsSortViewModel
    {
        public StudentSortState YearNo { get; set; }
        public StudentSortState FacultyName { get; set; }
        public StudentSortState LecternName { get; set; }
        public StudentSortState CurriculumName { get; set; }
        public StudentSortState Degree { get; set; }

        public StudentSortState Current { get; set; }

        public StudentsSortViewModel(StudentSortState sortState)
        {
            YearNo = sortState == StudentSortState.YearNoAsc ? StudentSortState.YearNoDesc : StudentSortState.YearNoAsc;
            FacultyName = sortState == StudentSortState.FacultyNameAsc ? StudentSortState.FacultyNameDesc : StudentSortState.FacultyNameAsc;
            LecternName = sortState == StudentSortState.LecternNameAsc ? StudentSortState.LecternNameDesc : StudentSortState.LecternNameAsc;
            CurriculumName = sortState == StudentSortState.CurriculumNameAsc ? StudentSortState.CurriculumNameDesc : StudentSortState.CurriculumNameAsc;
            Degree = sortState == StudentSortState.DegreeAsc ? StudentSortState.DegreeDesc : StudentSortState.DegreeAsc;

            Current = sortState;
        }
    }
}