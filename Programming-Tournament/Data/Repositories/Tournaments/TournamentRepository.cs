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
            IQueryable<Tournament> tournaments = GetQuery();

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
            IQueryable<Tournament> tournaments = GetQuery();
            tournaments = ApplySort(tournaments, sortState);

            var items = Paginate(tournaments, p, size);

            return items;
        }

        public IEnumerable<Tournament> GetTournaments(string userId, int p, int size)
        {
            IQueryable<Tournament> tournaments = GetQuery(userId);

            var items = Paginate(tournaments, p, size);

            return items;
        }

        public IEnumerable<Tournament> GetTournaments(string userId, int p, int size, TournamentSortState sortState)
        {
            IQueryable<Tournament> tournaments = GetQuery(userId);
            tournaments = ApplySort(tournaments, sortState);

            var items = Paginate(tournaments, p, size);

            return items;
        }

        private IEnumerable<Tournament> Paginate(IQueryable<Tournament> query, int p, int size)
        {
            var items = query.Skip((p - 1) * size).Take(size).ToList();

            return items;
        }

        private IQueryable<Tournament> GetQuery(string userId)
        {
            IQueryable<Tournament> query = context.Tournaments.Include(x => x.Owner)
                .Where(x => x.Owner.Id == userId);

            return query;
        }

        private IQueryable<Tournament> GetQuery()
        {
            IQueryable<Tournament> query = context.Tournaments.Include(x => x.Owner);

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
