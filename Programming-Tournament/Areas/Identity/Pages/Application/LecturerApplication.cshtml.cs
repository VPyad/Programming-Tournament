using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
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
    public class LecturerApplicationModel : PageModel
    {
        private readonly ApplicationDbContext context;

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IEnumerable<Faculty> Faculties { get; set; }

        public IEnumerable<Lectern> Lecterns { get; set; }

        public LecturerApplicationModel(ApplicationDbContext context)
        {
            this.context= context;
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
            var applicationsManager = new ApplicationsManager(context);

            Faculties = applicationsManager.GetFaculties();
            Lecterns = applicationsManager.GetLecterns();
        }

        public void OnPost()
        {

        }
    }
}