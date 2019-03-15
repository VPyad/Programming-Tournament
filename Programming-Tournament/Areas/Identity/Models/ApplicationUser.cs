using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Areas.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public int? DocNo { get; set; }

        public int? YearNo { get; set; }

        public DegreeType DegreeType { get; set; }

        public Faculty Faculty { get; set; }

        public Lectern Lectern { get; set; }

        public Curriculum Сurriculum { get; set; }
    }
}
