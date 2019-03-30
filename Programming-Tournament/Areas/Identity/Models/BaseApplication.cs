using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Areas.Identity.Models
{
    public class BaseApplication
    {
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int? DocNo { get; set; }

        public int? YearNo { get; set; }

        public Faculty Faculty { get; set; }

        public Lectern Lectern { get; set; }

        public DegreeType DegreeType { get; set; }

        public Curriculum Curriculum { get; set; }

        public UserType UserType { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
