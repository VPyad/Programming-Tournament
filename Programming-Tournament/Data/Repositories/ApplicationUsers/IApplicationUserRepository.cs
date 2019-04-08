using Programming_Tournament.Data.Repositories.Core;
using Programming_Tournament.Models.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Programming_Tournament.Data.Repositories.ApplicationUsers
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        ApplicationUser Get(string userId);

        ApplicationUser GetStudent(string userId);

        IEnumerable<ApplicationUser> GetStudents();

        IEnumerable<ApplicationUser> GetStudentsWithTournament(int tournamentId);

        IEnumerable<ApplicationUser> GetStudentsWhere(Expression<Func<ApplicationUser, bool>> exp);

        IEnumerable<ApplicationUser> GetStudents(StudentSortState sortState);

        IEnumerable<ApplicationUser> GetStudents(int p, int size, StudentSortState sortState);
    }

    public enum StudentSortState
    {
        YearNoAsc,
        YearNoDesc,
        FacultyNameAsc,
        FacultyNameDesc,
        LecternNameAsc,
        LecternNameDesc,
        CurriculumNameAsc,
        CurriculumNameDesc,
        DegreeAsc,
        DegreeDesc
    }
}
