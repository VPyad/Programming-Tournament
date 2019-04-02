using ProcessManagment.BuildSystem;
using Programming_Tournament.Models.Domain.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Models.Domain.Tournaments
{
    public class TournamentTaskAssignmentResult
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TournamentTaskAssigneeResultId { get; set; }

        public TournamentTask Task { get; set; }

        public TournamentTaskAssignment Assignee { get; set; }

        public ProcessResult ProcessResult { get; set; }
    }
}
