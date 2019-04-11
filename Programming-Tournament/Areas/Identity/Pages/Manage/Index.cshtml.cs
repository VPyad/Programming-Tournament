using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Programming_Tournament.Areas.Admin.Models;
using Programming_Tournament.Areas.Identity.Models;
using Programming_Tournament.Data;
using Programming_Tournament.Models.Domain.User;

namespace Programming_Tournament.Areas.Identity.Pages.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext context;
        private readonly IOptions<RequestLocalizationOptions> localizationOptions;

        public IndexModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IOptions<RequestLocalizationOptions> localizationOptions)
        {
            this.context = context;
            this.userManager = userManager;
            this.localizationOptions = localizationOptions;
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

            var cultures = localizationOptions.Value.SupportedUICultures.ToList();
            var rqf = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var currentCulture = rqf.RequestCulture.Culture;

            List<SelectListItem> langs = new List<SelectListItem>();

            foreach (var item in cultures)
            {
                var code = item.TwoLetterISOLanguageName;
                var name = code == "en" ? "English" : "Русский";
                langs.Add(new SelectListItem { Text = name, Value = code, Selected = code == currentCulture.TwoLetterISOLanguageName });
            }

            Input.LangsSelectList = langs;
            Input.LangCode = langs.FirstOrDefault(x => x.Selected).Value;

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

        public async Task<IActionResult> OnPostChangeLang()
        {
            var cultures = localizationOptions.Value.SupportedUICultures.ToList();
            var culture = cultures.FirstOrDefault(x => x.TwoLetterISOLanguageName == Input.LangCode);

            if (culture != null)
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return await OnGetAsync();
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

        [Display(Name = "Change language")]
        public IEnumerable<SelectListItem> LangsSelectList { get; set; }

        public string LangCode { get; set; }
    }
}