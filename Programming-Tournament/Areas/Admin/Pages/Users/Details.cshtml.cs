﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        [BindProperty]
        public UserModel Model { get; set; }

        public string ReturnUrl { get; set; }

        public string UserId { get; private set; }

        public DetailsModel(IServiceProvider serviceProvider, IConfiguration configuration, ApplicationDbContext context)
        {
            this.context = context;
            this.serviceProvider = serviceProvider;
            this.configuration = configuration;
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

        public void OnPostApprove(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
        }

        public void OnPostReject(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
        }

        public void OnPostDeactivate(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
        }

        public void OnPostActivate(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
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

            [Display(Name = "First name")]
            public string FirstName { get; set; }

            [Display(Name = "Second name")]
            public string SecondName { get; set; }

            [Display(Name = "Document number")]
            public int? DocNo { get; set; }

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

            public UserType Type { get; set; }

            public enum UserType
            {
                Student,
                Lecturer
            }
        }
    }
}