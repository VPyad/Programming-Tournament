using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Programming_Tournament.Areas.Identity.Models;
using Programming_Tournament.Data;
using Programming_Tournament.Models.Domain.User;

namespace Programming_Tournament.Areas.Admin.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly IServiceProvider serviceProvider;
        private readonly IConfiguration configuration;

        public IndexViewModel ViewModel { get; set; }

        public IndexModel(IServiceProvider serviceProvider, IConfiguration configuration, ApplicationDbContext context)
        {
            this.context = context;
            this.serviceProvider = serviceProvider;
            this.configuration = configuration;
        }

        public async Task<IActionResult> OnGet(int p = 1, SortState sortState = SortState.CreatedAtDesc, string q = null)
        {
            int pageSize = 5;

            IQueryable<ApplicationUser> users = context.Users.Include(x => x.Faculty)
                .Include(x => x.Lectern).Include(x => x.Сurriculum);

            if (!string.IsNullOrEmpty(q))
            {
                q = q.ToLower();
                users = users.Where(x => x.FirstName.ToLower().Contains(q)
                                    || x.SecondName.ToLower().Contains(q)
                                    || x.Faculty.Name.ToLower().Contains(q)
                                    || x.Lectern.Name.ToLower().Contains(q)
                                    || x.Сurriculum.Name.ToLower().Contains(q)
                                    || x.Email.ToLower().Contains(q)
                                    || x.DocNo.Value.ToString().Contains(q));
            }

            switch (sortState)
            {
                case SortState.StatusAsc:
                    users = users.OrderBy(x => x.Status);
                    break;
                case SortState.StatusDesc:
                    users = users.OrderByDescending(x => x.Status);
                    break;
                case SortState.TypeAsc:
                    users = users.OrderBy(x => x.Type);
                    break;
                case SortState.TypeDesc:
                    users = users.OrderByDescending(x => x.Type);
                    break;
                case SortState.CreatedAtAsc:
                    users = users.OrderBy(x => x.CreatedAt);
                    break;
                case SortState.CreatedAtDesc:
                    users = users.OrderByDescending(x => x.CreatedAt);
                    break;
            }

            int count = users.Count();
            var items = await users.Skip((p - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewModel = new IndexViewModel
            {
                SortViewModel = new SortViewModel(sortState),
                PageViewModel = new PageViewModel(count, p, pageSize),
                Users = items,
                SearchTerm = q
            };

            return Page();
        }
    }

    public enum SortState
    {
        StatusAsc,
        StatusDesc,
        TypeAsc,
        TypeDesc,
        CreatedAtAsc,
        CreatedAtDesc
    }

    public class SortViewModel
    {
        public SortState StatusSort { get; private set; }
        public SortState TypeSort { get; private set; }
        public SortState CreatedAtSort { get; private set; }
        public SortState Current { get; private set; }

        public SortViewModel(SortState sortState)
        {
            StatusSort = sortState == SortState.StatusAsc ? SortState.StatusDesc : SortState.StatusAsc;
            TypeSort = sortState == SortState.TypeAsc ? SortState.TypeDesc : SortState.TypeAsc;
            CreatedAtSort = sortState == SortState.CreatedAtAsc ? SortState.CreatedAtDesc : SortState.CreatedAtAsc;

            Current = sortState;
        }
    }

    public class PageViewModel
    {
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }

        public PageViewModel(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public bool HasPreviousPage => (PageNumber > 1);

        public bool HasNextPage => (PageNumber < TotalPages);
    }

    public class IndexViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }

        public SortViewModel SortViewModel { get; set; }

        public PageViewModel PageViewModel { get; set; }

        [Display(Name = "Search term")]
        public string SearchTerm { get; set; }
    }
}