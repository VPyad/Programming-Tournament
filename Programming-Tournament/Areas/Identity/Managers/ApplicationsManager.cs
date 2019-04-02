using Programming_Tournament.Areas.Identity.Models;
using Programming_Tournament.Data;
using Programming_Tournament.Models.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Areas.Identity.Managers
{
    public class ApplicationsManager
    {
        private ApplicationDbContext context;

        public ApplicationsManager(ApplicationDbContext applicationDbContext)
        {
            context = applicationDbContext;
        }

        public IEnumerable<Faculty> GetFaculties()
        {
            return context.Faculties.ToList();
        }

        public IEnumerable<Lectern> GetLecterns()
        {
            return context.Lecterns.ToList();
        }

        public IEnumerable<Curriculum> GetCurriculums()
        {
            return context.Сurriculums.ToList();
        }

        public bool ApplicationExist(string email) => context.Users.Any(x => x.Email == email);
    }
}
