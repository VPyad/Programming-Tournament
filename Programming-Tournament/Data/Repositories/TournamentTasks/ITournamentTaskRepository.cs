using Programming_Tournament.Data.Repositories.Core;
using Programming_Tournament.Models.Domain.Tournaments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Data.Repositories.TournamentTasks
{
    public interface ITournamentTaskRepository : IRepository<TournamentTask>
    {
        IEnumerable<TournamentTask> GetTasksWithToutnament(int tournamentId);

        TournamentTask GetTask(int id);
    }
}
