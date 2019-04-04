using Programming_Tournament.Data.Repositories.Core;
using Programming_Tournament.Models.Domain.Tournaments;
using Programming_Tournament.Models.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Data.Repositories.StudentTournaments
{
    public interface IStudentTournamentRepository : IRepository<StudentTournament>
    {
        void Remove(string userId, int tournamentId);

        StudentTournament Get(string userId, int tournamentId);
    }
}
