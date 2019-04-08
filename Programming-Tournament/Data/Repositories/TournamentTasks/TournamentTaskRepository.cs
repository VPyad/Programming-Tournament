using Microsoft.EntityFrameworkCore;
using Programming_Tournament.Data.Repositories.Core;
using Programming_Tournament.Models.Domain.Tournaments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Programming_Tournament.Data.Repositories.TournamentTasks
{
    public class TournamentTaskRepository : Repository<TournamentTask>, ITournamentTaskRepository
    {
        public TournamentTaskRepository(ApplicationDbContext contetxt) : base(contetxt) { }

        public TournamentTask GetTask(int id)
        {
            var task = GetQuery(x => x.TournamentTaskId == id).FirstOrDefault();

            return task;
        }

        public IEnumerable<TournamentTask> GetTasksWithToutnament(int tournamentId)
        {
            var tasks = GetQuery(x => x.Tournament.TournamentId == tournamentId);

            return tasks;
        }

        private IQueryable<TournamentTask> GetQuery()
        {
            var tasks = context.TournamentTasks.Include(x => x.Owner)
                .Include(x => x.SupportedLanguages)
                .Include(x => x.Tournament);

            return tasks;
        }

        private IQueryable<TournamentTask> GetQuery(Expression<Func<TournamentTask, bool>> whereExp)
        {
            var tasks = context.TournamentTasks.Include(x => x.Owner)
                .Include(x => x.SupportedLanguages)
                .Include(x => x.Tournament)
                .Include(x => x.Assignees).ThenInclude(x => x.User)
                .Include(x => x.Assignees).ThenInclude(x => x.Result)
                .Where(whereExp);

            return tasks;
        }
    }
}
