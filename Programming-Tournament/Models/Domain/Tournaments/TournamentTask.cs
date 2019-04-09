using ProcessManagment.BuildSystem;
using Programming_Tournament.Models.Domain.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Models.Domain.Tournaments
{
    public class TournamentTask
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TournamentTaskId { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public int MaxAttempt { get; set; }

        public Tournament Tournament { get; set; }

        public string InputFilePath { get; set; }

        public string ExpectedFilePath { get; set; }

        public ApplicationUser Owner { get; set; }

        public ICollection<SupportedProgrammingLanguage> SupportedLanguages { get; set; }

        public int? Timeout { get; set; }

        public ICollection<TournamentTaskAssignment> Assignees { get; set; }
    }
}
