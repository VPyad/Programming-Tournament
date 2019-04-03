using Programming_Tournament.Data.Repositories.Core;
using Programming_Tournament.Models.Domain.Tournaments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Data.Repositories.Tournaments
{
    public interface ITournamentRepository : IRepository<Tournament>
    {
        IEnumerable<Tournament> GetTournaments(int p, int size);
        IEnumerable<Tournament> GetTournaments(string userId, int p, int size);

        IEnumerable<Tournament> GetTournaments(int p, int size, TournamentSortState sortState);
        IEnumerable<Tournament> GetTournaments(string userId, int p, int size, TournamentSortState sortState);
    }

    public enum TournamentSortState
    {
        DueDateAsc,
        DueDateDesc,
        CreatedAtAsc,
        CreatedAtDesc,
        StatusAsc,
        StatusDesc,
        OwnerAsc,
        OwnerDesc
    }
}
