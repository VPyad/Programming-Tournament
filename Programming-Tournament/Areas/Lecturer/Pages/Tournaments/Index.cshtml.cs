using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Programming_Tournament.CommonViewModels;
using Programming_Tournament.Data;
using Programming_Tournament.Data.Repositories.ApplicationUsers;
using Programming_Tournament.Data.Repositories.Tournaments;
using Programming_Tournament.Models.Domain.Tournaments;
using Programming_Tournament.Models.Domain.User;

namespace Programming_Tournament.Areas.Lecturer.Pages.Tournaments
{
    [Authorize(Roles = "Admin,Lecturer")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly TournamentRepository repository;

        public TournamentIndexViewModel ViewModel { get; set; }

        public IndexModel(ApplicationDbContext context)
        {
            this.context = context;
            this.repository = new TournamentRepository(this.context);
        }

        public IActionResult OnGet(int p = 1, TournamentSortState sortState = TournamentSortState.CreatedAtAsc)
        {            
            int size = 15;
            IEnumerable<Tournament> tournaments;

            if (User.IsInRole("Admin"))
                tournaments = repository.GetTournaments(p, size, sortState);
            else
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                tournaments = repository.GetTournaments(userId, p, size, sortState);
            }
            
            var count = tournaments.Count();

            ViewModel = new TournamentIndexViewModel
            {
                SortViewModel = new TournamentIndexSortViewModel(sortState),
                PageViewModel = new PageViewModel(count, p, size),
                Tournaments = tournaments
            };

            return Page();
        }

        public IActionResult OnPostCreate()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUserRepository userRepository = new ApplicationUserRepository(context);

            var user = userRepository.Get(userId);

            var tournament = new Tournament
            {
                Status = TournamentStatus.Draft,
                CreatedAt = DateTime.Now,
                DueDate = DateTime.Now.AddMonths(1),
                Owner = user,
                Name = "Draft " + DateTime.Now.ToShortDateString()
            };

            repository.Add(tournament);
            int tournamentId = tournament.TournamentId;

            return RedirectToPage("Edit", new { id = tournamentId });
        }
    }

    public class TournamentIndexViewModel
    {
        public IEnumerable<Tournament> Tournaments { get; set; }

        public TournamentIndexSortViewModel SortViewModel { get; set; }

        public PageViewModel PageViewModel { get; set; }
    }

    public class TournamentIndexSortViewModel
    {
        public TournamentSortState DueDate { get; private set; }
        public TournamentSortState CreatedAt { get; private set; }
        public TournamentSortState Status { get; private set; }
        public TournamentSortState Owner { get; private set; }
        public TournamentSortState Current { get; private set; }

        public TournamentIndexSortViewModel(TournamentSortState sortState)
        {
            DueDate = sortState == TournamentSortState.DueDateAsc ? TournamentSortState.DueDateDesc : TournamentSortState.DueDateAsc;
            CreatedAt = sortState == TournamentSortState.CreatedAtAsc ? TournamentSortState.CreatedAtDesc : TournamentSortState.CreatedAtAsc;
            Status = sortState == TournamentSortState.StatusAsc ? TournamentSortState.StatusDesc : TournamentSortState.StatusAsc;
            Owner = sortState == TournamentSortState.OwnerAsc ? TournamentSortState.OwnerDesc : TournamentSortState.OwnerAsc;

            Current = sortState;
        }
    }
}