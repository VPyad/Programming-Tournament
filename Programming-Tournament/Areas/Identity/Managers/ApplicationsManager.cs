using Programming_Tournament.Areas.Identity.Models;
using Programming_Tournament.Data;
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
    }
}
