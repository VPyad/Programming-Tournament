using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Programming_Tournament.Areas.Identity.Managers;
using Programming_Tournament.Areas.Identity.Models;
using Programming_Tournament.Data;

namespace Programming_Tournament.Areas.Identity.Pages.Application
{
    [AllowAnonymous]
    public class StudentApplicationModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly ApplicationsManager applicationsManager;

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IEnumerable<Faculty> Faculties { get; set; }

        public IEnumerable<Lectern> Lecterns { get; set; }

        public IEnumerable<Curriculum> Curriculums { get; set; }

        public StudentApplicationModel(ApplicationDbContext context)
        {
            this.context = context;
            applicationsManager = new ApplicationsManager(this.context);
        }

        public class InputModel
        {
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

        public IActionResult OnPost(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var application = MapModel(Input);
                applicationsManager.SaveApplication(application);

                return LocalRedirect("/Identity/Application/ApplicationSent");
            }

            return Page();
        }

        private StudentApplication MapModel(InputModel inputModel)
        {
            StudentApplication application = new StudentApplication
            {
                DocNo = inputModel.DocNo,
                Email = inputModel.Email,
                FacultyId = inputModel.FacultyId,
                LecternId = inputModel.LecternId,
                Password = inputModel.Password,
                FirstName = inputModel.FirstName,
                SecondName = inputModel.SecondName,
                IsRegistered = false,
                DegreeType = inputModel.DegreeType,
                CurriculumId = inputModel.CurriculumId
            };

            return application;
        }
    }
}