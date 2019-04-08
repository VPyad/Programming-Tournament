using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Programming_Tournament.Data;
using Programming_Tournament.Data.Repositories.TournamentTasks;

namespace Programming_Tournament.Areas.Student.Pages.Tournaments
{
    [Authorize(Roles = "Admin,Lecturer,Student")]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly TournamentTaskRepository taskRepository;

        public DetailsModel(ApplicationDbContext context)
        {
            this.context = context;
            taskRepository = new TournamentTaskRepository(this.context);
        }

        public IActionResult OnGet(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            return Page();
        }
    }
}