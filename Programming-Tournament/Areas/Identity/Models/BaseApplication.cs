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

        // Let`s pretend that app does not contains any privacy information and cannot be used against user.
        // After registration field will be set to empty
        public string Password { get; set; }

        public int? DocNo { get; set; }

        public long FacultyId { get; set; }

        public long LecternId { get; set; }

        public string UserId { get; set; }

        public bool IsRegistered { get; set; }
    }
}
