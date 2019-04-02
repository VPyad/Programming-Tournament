using ProcessManagment.BuildSystem;
using Programming_Tournament.Models.Domain.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Models.Domain.Tournaments
{
    public class TournamentTaskAssignmentResult
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public TournamentTask Task { get; set; }

        public int ProcessResultId { get; set; }
    }
}
