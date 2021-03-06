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
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly ApplicationsManager applicationsManager;
        private readonly IServiceProvider serviceProvider;
        private readonly IConfiguration configuration;

        [BindProperty]
        public ApplicationUserEditPageModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IEnumerable<Faculty> Faculties { get; set; }
        public IEnumerable<Lectern> Lecterns { get; set; }
        public IEnumerable<Curriculum> Curriculums { get; set; }

        public CreateModel(ApplicationDbContext context, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
            this.serviceProvider = serviceProvider;

            applicationsManager = new ApplicationsManager(this.context);
        }

        public IActionResult OnGet()
        {
            PopulateData();

            return Page();
        }

        private void PopulateData()
        {
            Faculties = applicationsManager.GetFaculties();
            Lecterns = applicationsManager.GetLecterns();
            Curriculums = applicationsManager.GetCurriculums();
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            PopulateData();

            if (ModelState.IsValid)
            {
                if (applicationsManager.ApplicationExist(Input.Email))
                {
                    ModelState.AddModelError(string.Empty, "User with this email has already existed");

                    return Page();
                }
                else
                {
                    var user = ApplicationUserEditPageModel.ComposeApplicationUser(Input, Faculties, Lecterns, Curriculums);
                    await UsersManager.CreateUser(serviceProvider, configuration, user, Input.Password, Input.Email, Input.UserType);

                    return LocalRedirect("~/Admin/Users");
                }
            }

            return Page();
        }
    }
}