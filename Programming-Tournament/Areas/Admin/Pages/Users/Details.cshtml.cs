using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Programming_Tournament.Areas.Identity.Managers;
using Programming_Tournament.Areas.Identity.Models;
using Programming_Tournament.Data;

namespace Programming_Tournament.Areas.Admin.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly IServiceProvider serviceProvider;
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment hostingEnviroment;

        [BindProperty]
        public UserModel Model { get; set; }

        public string ReturnUrl { get; set; }

        [BindProperty]
        public string UserId { get; private set; }

        public bool IsDevelopment { get; private set; }

        public DetailsModel(IServiceProvider serviceProvider, IConfiguration configuration, ApplicationDbContext context, IHostingEnvironment hostingEnv)
        {
            this.context = context;
            this.serviceProvider = serviceProvider;
            this.configuration = configuration;
            this.hostingEnviroment = hostingEnv;

            IsDevelopment = hostingEnviroment.IsDevelopment();
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

            Model = new UserModel(user, role);

            return Page();
        }

        public async Task<IActionResult> OnPostApprove(string id, string returnUrl = null)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            returnUrl = returnUrl ?? Url.Content("~/");

            UsersManager.ChangeUserStatus(context, id, UserStatus.Active);

            await OnGet(id);
            return Page();
        }

        public async Task<IActionResult> OnPostReject(string id, string returnUrl = null)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            returnUrl = returnUrl ?? Url.Content("~/");

            UsersManager.ChangeUserStatus(context, id, UserStatus.Rejected);

            await OnGet(id);
            return Page();
        }

        public async Task<IActionResult> OnPostDeactivate(string id, string returnUrl = null)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            returnUrl = returnUrl ?? Url.Content("~/");

            UsersManager.ChangeUserStatus(context, id, UserStatus.Inactive);

            await OnGet(id);
            return Page();
        }

        public async Task<IActionResult> OnPostActivate(string id, string returnUrl = null)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            returnUrl = returnUrl ?? Url.Content("~/");

            UsersManager.ChangeUserStatus(context, id, UserStatus.Active);

            await OnGet(id);
            return Page();
        }

        public async Task<IActionResult> OnPostSubmit(string id, string returnUrl = null)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            returnUrl = returnUrl ?? Url.Content("~/");

            if (!IsDevelopment)
                return Page();

            UsersManager.ChangeUserStatus(context, id, UserStatus.Submitted);

            await OnGet(id);
            return Page();
        }

        public class UserModel
        {
            public UserModel() { }

            public UserModel(ApplicationUser user, string role)
            {
                FirstName = user.FirstName;
                SecondName = user.SecondName;
                DocNo = user.DocNo;
                DegreeType = user.DegreeType;
                FacultyName = user.Faculty.Name;
                LecternName = user.Lectern.Name;
                CurriculumName = user.Сurriculum.Name;
                UserStatus = user.Status;
                YearNo = user.YearNo;
                Email = user.Email;
                CreatedAt = user.CreatedAt;

                switch (role)
                {
                    case "Student":
                        Type = UserType.Student;
                        break;
                    default:
                        Type = UserType.Lecturer;
                        break;
                }
            }

            [Display(Name = "Email")]
            public string Email { get; set; }

            [Display(Name = "First name")]
            public string FirstName { get; set; }

            [Display(Name = "Second name")]
            public string SecondName { get; set; }

            [Display(Name = "Document number")]
            public int? DocNo { get; set; }

            [Display(Name = "Year of education")]
            public int? YearNo { get; set; }

            [Display(Name = "Degree")]
            public DegreeType DegreeType { get; set; }

            [Display(Name = "Faculty")]
            public string FacultyName { get; set; }

            [Display(Name = "Lectern")]
            public string LecternName { get; set; }

            [Display(Name = "Curriculum")]
            public string CurriculumName { get; set; }

            [Display(Name = "Status")]
            public UserStatus UserStatus { get; set; }

            [Display(Name = "First name")]
            public DateTime CreatedAt { get; set; }

            public UserType Type { get; set; }

            public enum UserType
            {
                Student,
                Lecturer
            }
        }
    }
}