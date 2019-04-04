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

        public IEnumerable<ApplicationUser> GetStudents()
        {
            var students = context.Users
                .Where(x => x.Type == UserType.Student)
                .Include(x => x.Lectern).Include(x => x.Сurriculum).Include(x => x.Faculty)
                .ToList();

            return students;
        }

        public IEnumerable<ApplicationUser> GetStudentsWhere(Expression<Func<ApplicationUser, bool>> exp)
        {
            var students = context.Users
                .Where(x => x.Type == UserType.Student).Where(exp)
                .Include(x => x.Lectern).Include(x => x.Сurriculum).Include(x => x.Faculty)
                .ToList();

            return students;
        }
    }
}
