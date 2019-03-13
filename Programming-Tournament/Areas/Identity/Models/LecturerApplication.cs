using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Areas.Identity.Models
{
    public class LecturerApplication
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LecturerApplicationId { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int? DocNo { get; set; }

        public long FacultyId { get; set; }

        public long LecternId { get; set; }
    }
}
