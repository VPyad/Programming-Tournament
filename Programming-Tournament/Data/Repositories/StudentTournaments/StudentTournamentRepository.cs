using Programming_Tournament.Data.Repositories.Core;
using Programming_Tournament.Models.Domain.Tournaments;
using Programming_Tournament.Models.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Data.Repositories.StudentTournaments
{
    public class StudentTournamentRepository : Repository<StudentTournament>, IStudentTournamentRepository
    {
        public StudentTournamentRepository(ApplicationDbContext context) : base(context) { }

        public StudentTournament Get(string userId, int tournamentId)
        {
            var st = context.StudentTournaments.FirstOrDefault(x => x.ApplicationUserId == userId && x.TournamentId == tournamentId);

            return st;
        }

        public void Remove(string userId, int tournamentId)
        {
            var st = context.StudentTournaments.FirstOrDefault(x => x.ApplicationUserId == userId && x.TournamentId == tournamentId);

            if (st != null)
                Remove(st);
        }
    }
}
