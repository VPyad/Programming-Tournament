using Programming_Tournament.Models.Domain.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Models.Domain.Tournaments
{
    public class Tournament
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TournamentId { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<TournamentTask> Tasks { get; set; }

        public ApplicationUser Owner { get; set; }

        public ICollection<StudentTournament> Assignees { get; set; }

        public TournamentStatus Status { get; set; }
    }

    public enum TournamentStatus
    {
        Draft,
        Active,
        Completed,
        Inactive
    }
}
