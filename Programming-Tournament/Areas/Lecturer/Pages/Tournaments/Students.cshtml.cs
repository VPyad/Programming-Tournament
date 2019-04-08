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
using Programming_Tournament.Data.Repositories.StudentTournaments;
using Programming_Tournament.Data.Repositories.Tournaments;
using Programming_Tournament.Models.Domain.Tournaments;
using Programming_Tournament.Models.Domain.User;

namespace Programming_Tournament.Areas.Lecturer.Pages.Tournaments
{
    [Authorize(Roles = "Admin,Lecturer")]
    public class StudentsModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly TournamentRepository tournamentRepository;
        private readonly ApplicationUserRepository userRepository;
        private readonly StudentTournamentRepository studentTournamentRepository;

        [BindProperty]
        public StudentsViewModel ViewModel { get; set; }

        public StudentsModel(ApplicationDbContext context)
        {
            this.context = context;
            tournamentRepository = new TournamentRepository(this.context);
            userRepository = new ApplicationUserRepository(this.context);
            studentTournamentRepository = new StudentTournamentRepository(this.context);
        }

        public IActionResult OnGet(int? id, int p = 1, StudentSortState sortState = StudentSortState.FacultyNameAsc)
        {
            if (!id.HasValue)
                return NotFound();

            var tournament = tournamentRepository.GetTournament(id.Value);
            if (tournament == null)
                return NotFound();

            int size = 25;

            var students = userRepository.GetStudents(p, size, sortState);
            var studentsViewModel = new List<StudentEntityViewModel>();
            foreach (var item in students)
                studentsViewModel.Add(new StudentEntityViewModel(item, id.Value));

            ViewModel = new StudentsViewModel
            {
                TournamentId = tournament.TournamentId,
                TournamentName = tournament.Name,
                SortViewModel = new StudentsSortViewModel(sortState),
                Students = studentsViewModel,
                PageViewModel = new PageViewModel(studentsViewModel.Count(), p, size)
            };

            return Page();
        }

        public IActionResult OnPostApply(int? id, int p = 1, StudentSortState sortState = StudentSortState.FacultyNameAsc)
        {
            if (!id.HasValue)
                return NotFound();

            var tournament = tournamentRepository.GetTournament(id.Value);
            if (tournament == null)
                return NotFound();

            if (ViewModel.Students.Any(x => x.ValueChanged))
                foreach (var item in ViewModel.Students.Where(x => x.ValueChanged))
                {
                    var student = userRepository.GetStudent(item.Id);
                    if (item.HasBeenChacked)
                    {
                        StudentTournament st = new StudentTournament
                        {
                            ApplicationUser = student,
                            ApplicationUserId = student.Id,
                            Tournament = tournament,
                            TournamentId = tournament.TournamentId
                        };

                        studentTournamentRepository.Add(st);
                        student.Tournaments.Add(st);
                        userRepository.Update(student);
                        tournament.Assignees.Add(st);
                        tournamentRepository.Update(tournament);
                    }
                    else if (item.HasBeenUnChacked)
                    {
                        var st = studentTournamentRepository.Get(student.Id, tournament.TournamentId);

                        if (st != null)
                        {
                            studentTournamentRepository.Remove(st);
                        }
                    }
                }

            return OnGet(id, p, sortState);
        }

        public IActionResult OnPostDone(int? id, int p = 1, StudentSortState sortState = StudentSortState.FacultyNameAsc)
        {
            return OnGet(id, p, sortState);
        }
    }

    [BindProperties]
    public class StudentsViewModel
    {
        public string TournamentName { get; set; }

        public int TournamentId { get; set; }

        public IList<StudentEntityViewModel> Students { get; set; }

        public StudentsSortViewModel SortViewModel { get; set; }

        public PageViewModel PageViewModel { get; set; }

        public bool TestBool { get; set; } = false;
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

    [BindProperties]
    public class StudentEntityViewModel
    {
        public StudentEntityViewModel() { }

        public StudentEntityViewModel(ApplicationUser user, int tournamentId)
        {
            Value = user.Tournaments.FirstOrDefault(x => x.TournamentId == tournamentId) != null;
            SourceValue = Value;
            Id = user.Id;
            FirstName = user.FirstName;
            SecondName = user.SecondName;
            YearNo = user.YearNo.HasValue ? user.YearNo.Value.ToString() : "";
            FacultyName = user.Faculty.Name;
            LecternName = user.Lectern.Name;
            СurriculumName = user.Сurriculum.Name;
            DegreeType = user.DegreeType;
        }

        public bool Value { get; set; }

        public bool SourceValue { get; set; }

        public bool ValueChanged => Value != SourceValue;

        public bool HasBeenChacked => ValueChanged && (SourceValue == false && Value == true);

        public bool HasBeenUnChacked => ValueChanged && (SourceValue == true && Value == false);

        public string Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string FacultyName { get; set; }

        public string LecternName { get; set; }

        public string СurriculumName { get; set; }

        public string YearNo { get; set; }

        public DegreeType DegreeType { get; set; }
    }
}