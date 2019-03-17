using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Programming_Tournament.Areas.Identity.Managers;
using Programming_Tournament.Areas.Identity.Models;
using Programming_Tournament.Data;

namespace Programming_Tournament.Areas.Identity.Pages.Application
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly ApplicationsManager applicationsManager;

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IEnumerable<Faculty> Faculties { get; set; }

        public IEnumerable<Lectern> Lecterns { get; set; }

        public IEnumerable<Curriculum> Curriculums { get; set; }

        public IndexModel(ApplicationDbContext context)
        {
            this.context = context;
            applicationsManager = new ApplicationsManager(this.context);
        }

        public class InputModel
        {
            [Required]
            [Display(Name = "I am")]
            public ApplicationType ApplicationType { get; set; }

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

            return Page();
        }
    }
}

public enum ApplicationType
{
    Student,
    Lecturer
}