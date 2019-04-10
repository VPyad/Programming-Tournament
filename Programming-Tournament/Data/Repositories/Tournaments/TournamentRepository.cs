using Microsoft.EntityFrameworkCore;
using Programming_Tournament.Data.Repositories.Core;
using Programming_Tournament.Models.Domain.Tournaments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Data.Repositories.Tournaments
{
    public class TournamentRepository : Repository<Tournament>, ITournamentRepository
    {
        public TournamentRepository(ApplicationDbContext context) : base(context) { }

        /// <summary>
        /// Returns all tournamnets. For Admin use
        /// </summary>
        /// <param name="p"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public IEnumerable<Tournament> GetTournaments(int p, int size)
        {
            IQueryable<Tournament> tournaments = GetLecturerQuery();

            var items = Paginate(tournaments, p, size);

            return items;
        }

        /// <summary>
        /// Returns all tournamnets. For Admin use
        /// </summary>
        /// <param name="p"></param>
        /// <param name="size"></param>
        /// <param name="sortState"></param>
        /// <returns></returns>
        public IEnumerable<Tournament> GetTournaments(int p, int size, TournamentSortState sortState)
        {
            IQueryable<Tournament> tournaments = GetLecturerQuery();
            tournaments = ApplySort(tournaments, sortState);

            var items = Paginate(tournaments, p, size);

            return items;
        }

        public IEnumerable<Tournament> GetActiveStudentTournament(string userId, TournamentSortState sortState)
        {
            IQueryable<Tournament> tournaments = GetStudentQuery(userId);

            tournaments = tournaments.Where(x => x.Status == TournamentStatus.Active || x.Status == TournamentStatus.Completed);
            tournaments = ApplySort(tournaments, sortState);

            return tournaments.ToList();
        }

        public IEnumerable<Tournament> GetTournaments(string userId, int p, int size)
        {
            IQueryable<Tournament> tournaments = GetLecturerQuery(userId);

            var items = Paginate(tournaments, p, size);

            return items;
        }

        public IEnumerable<Tournament> GetTournaments(string userId, int p, int size, TournamentSortState sortState)
        {
            IQueryable<Tournament> tournaments = GetLecturerQuery(userId);
            tournaments = ApplySort(tournaments, sortState);

            var items = Paginate(tournaments, p, size);

            return items;
        }

        public Tournament GetTournament(int id)
        {
            var tournament = context.Tournaments.Where(x => x.TournamentId == id)
                .Include(x => x.Owner)
                .Include(x => x.Assignees).ThenInclude(x => x.ApplicationUser)
                .Include(x => x.Tasks).ThenInclude(x => x.Assignees).ThenInclude(x => x.User)
                .FirstOrDefault();

            return tournament;
        }

        public Tournament GetTournamentByTask(int taskId)
        {
            var tournament = context.Tournaments.Include(x => x.Tasks)
                .Where(x => x.Tasks.FirstOrDefault(z => z.TournamentTaskId == taskId) != null)
                .FirstOrDefault();

            return tournament;
        }

        private IEnumerable<Tournament> Paginate(IQueryable<Tournament> query, int p, int size)
        {
            var items = query.Skip((p - 1) * size).Take(size).ToList();

            return items;
        }

        private IQueryable<Tournament> GetLecturerQuery(string userId)
        {
            IQueryable<Tournament> query = context.Tournaments.Include(x => x.Owner)
                .Where(x => x.Owner.Id == userId);

            return query;
        }

        private IQueryable<Tournament> GetLecturerQuery()
        {
            IQueryable<Tournament> query = context.Tournaments.Include(x => x.Owner);

            return query;
        }

        private IQueryable<Tournament> GetStudentQuery(string userId)
        {;

            IQueryable<Tournament> query = GetStudentQuery();

            query = query.Where(x => x.Assignees.Any(z => z.ApplicationUserId == userId));

            return query;
        }

        private IQueryable<Tournament> GetStudentQuery()
        {
            IQueryable<Tournament> query = context.Tournaments
                .Include(x => x.Owner)
                .Include(x => x.Assignees)
                .Include(x => x.Tasks).ThenInclude(x => x.Assignees).ThenInclude(x => x.User);

            return query;
        }

        private IQueryable<Tournament> ApplySort(IQueryable<Tournament> tournaments, TournamentSortState sortState)
        {

            switch (sortState)
            {
                case TournamentSortState.DueDateAsc:
                    tournaments = tournaments.OrderBy(x => x.DueDate);
                    break;
                case TournamentSortState.DueDateDesc:
                    tournaments = tournaments.OrderByDescending(x => x.DueDate);
                    break;
                case TournamentSortState.CreatedAtAsc:
                    tournaments = tournaments.OrderBy(x => x.CreatedAt);
                    break;
                case TournamentSortState.CreatedAtDesc:
                    tournaments = tournaments.OrderByDescending(x => x.CreatedAt);
                    break;
                case TournamentSortState.StatusAsc:
                    tournaments = tournaments.OrderBy(x => x.Status);
                    break;
                case TournamentSortState.StatusDesc:
                    tournaments = tournaments.OrderByDescending(x => x.Status);
                    break;
                case TournamentSortState.OwnerAsc:
                    tournaments = tournaments.OrderBy(x => x.Owner.FirstName);
                    break;
                case TournamentSortState.OwnerDesc:
                    tournaments = tournaments.OrderByDescending(x => x.Owner.FirstName);
                    break;
            }

            return tournaments;
        }
    }
}
