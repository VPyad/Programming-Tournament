using Programming_Tournament.Models.Domain.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Models.Domain.Tournaments
{
    public class TournamentTaskAssignment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public ApplicationUser User { get; set; }

        public bool IsPassed { get; set; }

        public int Attempts { get; set; }

        public TournamentTask Task { get; set; }

        public DateTime LastAttemptedAt { get; set; }

        public string WorkDir { get; set; }

        public TournamentTaskAssignmentResult Result { get; set; }
    }
}
