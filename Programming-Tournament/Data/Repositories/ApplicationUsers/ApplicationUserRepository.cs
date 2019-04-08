using Microsoft.EntityFrameworkCore;
using Programming_Tournament.Data.Repositories.Core;
using Programming_Tournament.Models.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Programming_Tournament.Data.Repositories.ApplicationUsers
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext context) : base(context) { }

        public ApplicationUser Get(string userId)
        {
            var user = context.Users.FirstOrDefault(x => x.Id == userId);

            return user;
        }

        public ApplicationUser GetStudent(string userId)
        {
            var student = GetStudentsQuery(x => x.Id == userId).FirstOrDefault();

            return student;
        }

        public IEnumerable<ApplicationUser> GetStudents()
        {
            var students = GetStudentsQuery();

            return students.ToList();
        }

        public IEnumerable<ApplicationUser> GetStudentsWithTournament(int tournamentId)
        {
            var students = GetStudentsQuery().ToList();
            List<ApplicationUser> result = new List<ApplicationUser>();

            foreach (var item in students)
                if (item.Tournaments.Any(x => x.TournamentId == tournamentId))
                    result.Add(item);

            return result;
        }

        public IEnumerable<ApplicationUser> GetStudentsWhere(Expression<Func<ApplicationUser, bool>> exp)
        {
            var students = GetStudentsQuery();
            students = students.Where(exp);

            return students.ToList();
        }

        public IEnumerable<ApplicationUser> GetStudents(StudentSortState sortState)
        {
            var students = GetStudentsQuery();
            students = ApplySort(students, sortState);

            return students.ToList();
        }

        public IEnumerable<ApplicationUser> GetStudents(int p, int size, StudentSortState sortState)
        {
            var students = GetStudentsQuery();
            students = ApplySort(students, sortState);

            var items = Paginate(students, p, size);

            return items;
        }

        private IEnumerable<ApplicationUser> Paginate(IQueryable<ApplicationUser> query, int p, int size)
        {
            var items = query.Skip((p - 1) * size).Take(size).ToList();

            return items;
        }

        private IQueryable<ApplicationUser> GetStudentsQuery()
        {
            var students = context.Users
                .Where(x => x.Type == UserType.Student && x.Status == UserStatus.Active)
                .Include(x => x.Lectern).Include(x => x.Сurriculum)
                .Include(x => x.Faculty).Include(x => x.Tournaments);

            return students;
        }

        private IQueryable<ApplicationUser> GetStudentsQuery(Expression<Func<ApplicationUser, bool>> whereExp)
        {
            var students = context.Users
                .Where(x => x.Type == UserType.Student && x.Status == UserStatus.Active)
                .Where(whereExp)
                .Include(x => x.Lectern).Include(x => x.Сurriculum)
                .Include(x => x.Faculty).Include(x => x.Tournaments);

            return students;
        }

        private IQueryable<ApplicationUser> ApplySort(IQueryable<ApplicationUser> users, StudentSortState sortState)
        {
            switch (sortState)
            {
                case StudentSortState.YearNoAsc:
                    users = users.OrderBy(x => x.YearNo);
                    break;
                case StudentSortState.YearNoDesc:
                    users = users.OrderByDescending(x => x.YearNo);
                    break;
                case StudentSortState.FacultyNameAsc:
                    users = users.OrderBy(x => x.Faculty.Name);
                    break;
                case StudentSortState.FacultyNameDesc:
                    users = users.OrderByDescending(x => x.Faculty.Name);
                    break;
                case StudentSortState.LecternNameAsc:
                    users = users.OrderBy(x => x.Lectern.Name);
                    break;
                case StudentSortState.LecternNameDesc:
                    users = users.OrderByDescending(x => x.Lectern.Name);
                    break;
                case StudentSortState.CurriculumNameAsc:
                    users = users.OrderBy(x => x.Сurriculum.Name);
                    break;
                case StudentSortState.CurriculumNameDesc:
                    users = users.OrderByDescending(x => x.Сurriculum.Name);
                    break;
                case StudentSortState.DegreeAsc:
                    users = users.OrderBy(x => x.DegreeType);
                    break;
                case StudentSortState.DegreeDesc:
                    users = users.OrderByDescending(x => x.DegreeType);
                    break;
            }

            return users;
        }
    }
}
