using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Programming_Tournament.Areas.Identity.Managers;
using Programming_Tournament.Areas.Identity.Models;
using Programming_Tournament.Data;
using Programming_Tournament.Models.Domain.User;

namespace Programming_Tournament.Areas.Identity.Pages.Application
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly ApplicationsManager applicationsManager;
        private readonly IServiceProvider serviceProvider;
        private readonly IConfiguration configuration;

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IEnumerable<Faculty> Faculties { get; set; }
        public IEnumerable<Lectern> Lecterns { get; set; }
        public IEnumerable<Curriculum> Curriculums { get; set; }

        public IndexModel(ApplicationDbContext context, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
            this.serviceProvider = serviceProvider;

            applicationsManager = new ApplicationsManager(this.context);
        }

        public class InputModel
        {
            [Required]
            [Display(Name = "I am")]
            public UserType UserType { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "First name")]
            public string FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Second name")]
            public string SecondName { get; set; }

            [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Only numbers allowed")]
            [Display(Name = "Document number (optional)")]
            public int? DocNo { get; set; }

            [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Only numbers allowed")]
            [Display(Name = "Year of education (optional)")]
            public int? YearNo { get; set; }

            [Required]
            [Display(Name = "Faculty")]
            public long FacultyId { get; set; }

            [Required]
            [Display(Name = "Lectern")]
            public long LecternId { get; set; }

            [Required]
            [Display(Name = "Curriculum")]
            public int CurriculumId { get; set; }

            [Required]
            [Display(Name = "DegreeType")]
            public DegreeType DegreeType { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
        }

        public void OnGet()
        {
            Faculties = applicationsManager.GetFaculties();
            Lecterns = applicationsManager.GetLecterns();
            Curriculums = applicationsManager.GetCurriculums();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            // TODO: refactor: change to OnGet()
            Faculties = applicationsManager.GetFaculties();
            Lecterns = applicationsManager.GetLecterns();
            Curriculums = applicationsManager.GetCurriculums();

            if (ModelState.IsValid)
            {
                if (applicationsManager.ApplicationExist(Input.Email))
                {
                    ModelState.AddModelError(string.Empty, "User with this email has already submitted application");

                    return Page();
                }
                else
                {
                    var application = MapToApplication(Input);
                    await UsersManager.CreateUser(serviceProvider, configuration, application);
                    return RedirectToPage("./ApplicationSent");
                }
            }

            return Page();
        }

        private BaseApplication MapToApplication(InputModel inputModel)
        {
            var fac = Faculties.FirstOrDefault(x => x.FacultyId == inputModel.FacultyId);
            var lec = Lecterns.FirstOrDefault(x => x.LecternId == inputModel.LecternId);

            var application = new BaseApplication
            {
                Email = inputModel.Email,
                Password = inputModel.Password,
                UserType = inputModel.UserType,
                DocNo = inputModel.DocNo,
                FirstName = inputModel.FirstName,
                SecondName = inputModel.SecondName,
                Faculty = Faculties.FirstOrDefault(x => x.FacultyId == inputModel.FacultyId),
                Lectern = Lecterns.FirstOrDefault(x => x.LecternId == inputModel.LecternId),
                CreatedAt = DateTime.Now
            };

            if (inputModel.UserType == UserType.Student)
            {
                application.UserType = UserType.Student;
                application.DegreeType = inputModel.DegreeType;
                application.Curriculum = Curriculums.FirstOrDefault(x => x.CurriculumId == inputModel.CurriculumId);
                application.YearNo = inputModel.YearNo;
            }
            else
            {
                application.UserType = UserType.Lecturer;
                application.DegreeType = DegreeType.Unknown;
                application.Curriculum = null;
            }

            return application;
        }
    }
}

