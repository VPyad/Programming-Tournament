using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Programming_Tournament.Areas.Admin.Models;
using Programming_Tournament.Areas.Identity.Models;
using Programming_Tournament.Data;

namespace Programming_Tournament.Areas.Identity.Pages.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext context;

        public IndexModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            user = context.Users.Where(x => x.Id == user.Id)
                .Include(x => x.Lectern)
                .Include(x => x.Faculty)
                .Include(x => x.Сurriculum)
                .First();

            var roles = await userManager.GetRolesAsync(user);

            if (roles.Contains("Admin"))
            {
                Input = new InputModel(true)
                {
                    Email = user.Email
                };
            }
            else
                Input = new InputModel(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (user != null)
                {
                    var result = await userManager.ChangePasswordAsync(user, Input.CurrentPassword, Input.NewPassword);
                    if (!result.Succeeded)
                        ModelState.AddModelError(string.Empty, "An error occuried");
                }
            }

            await OnGetAsync();
            return Page();
        }
    }

    public class InputModel : ApplicationUserDetailsPageModel
    {
        public InputModel() { }
        public InputModel(bool isAdmin)
        {
            IsAdmin = isAdmin;
        }

        public InputModel(ApplicationUser user) : base(user) { }

        public bool IsAdmin { get; private set; } = false;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }
    }
}