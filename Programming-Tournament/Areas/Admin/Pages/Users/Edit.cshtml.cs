using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Programming_Tournament.Areas.Admin.Models;
using Programming_Tournament.Areas.Identity.Models;
using Programming_Tournament.Data;
using Programming_Tournament.Data.Managers;
using Programming_Tournament.Models.Domain.User;

namespace Programming_Tournament.Areas.Admin.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly IServiceProvider serviceProvider;
        private readonly IConfiguration configuration;
        private readonly ApplicationsManager applicationsManager;

        public IEnumerable<Faculty> Faculties { get; set; }
        public IEnumerable<Lectern> Lecterns { get; set; }
        public IEnumerable<Curriculum> Curriculums { get; set; }

        [BindProperty]
        public ApplicationUserEditPageModel Input { get; set; }

        public string ReturnUrl { get; set; }
        public string UserId { get; set; }

        public EditModel(IServiceProvider serviceProvider, IConfiguration configuration, ApplicationDbContext context)
        {
            this.context = context;
            this.serviceProvider = serviceProvider;
            this.configuration = configuration;

            applicationsManager = new ApplicationsManager(this.context);
        }

        public async Task<IActionResult> OnGet(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = UsersManager.GetUser(context, id);
            UserId = id;

            if (user == null)
                return NotFound();

            var roles = await UsersManager.GetRolesAsync(serviceProvider, configuration, user);
            var role = roles.FirstOrDefault();
            bool isAdmin = !string.IsNullOrEmpty(role) && role == "Admin";

            if (isAdmin)
                return NotFound();

            PopulateData();

            Input = new ApplicationUserEditPageModel(user);

            return Page();
        }

        public async Task<IActionResult> OnPost(string id, string returnUrl = null)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = UsersManager.GetUser(context, id);
            UserId = id;

            if (user == null)
                return NotFound();

            returnUrl = returnUrl ?? Url.Content("~/");

            PopulateData();
            ApplicationUserEditPageModel.ApplyChanges(user, Input, Faculties, Lecterns, Curriculums);
            UsersManager.UpdateUser(context, user);

            await OnGet(id);
            return Page();
        }

        private void PopulateData()
        {
            Faculties = applicationsManager.GetFaculties();
            Lecterns = applicationsManager.GetLecterns();
            Curriculums = applicationsManager.GetCurriculums();
        }
    }
}