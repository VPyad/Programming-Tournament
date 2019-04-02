using Programming_Tournament.Models.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Models.Domain.Tournaments
{
    public class TournamentTaskAssignment
    {
        public ApplicationUser User { get; set; }

        public bool IsPassed { get; set; }

        public int Attempts { get; set; }

        public TournamentTask Task { get; set; }

        public IEnumerable<DateTime> AttemptedAt { get; set; }

        public string WorkDir { get; set; }

        public TournamentTaskAssignmentResult Result { get; set; }
    }
}
