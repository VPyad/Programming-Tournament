﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Areas.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() { }

        public ApplicationUser(BaseApplication application)
        {
            FirstName = application.FirstName;
            SecondName = application.SecondName;
            DocNo = application.DocNo;
            DegreeType = application.DegreeType;
            Faculty = application.Faculty;
            Lectern = application.Lectern;
            Сurriculum = application.Curriculum;
            Status = UserStatus.Submitted;
            YearNo = application.YearNo;
            CreatedAt = application.CreatedAt;
            Type = application.UserType;
        }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public int? DocNo { get; set; }

        public int? YearNo { get; set; }

        public DegreeType DegreeType { get; set; }

        public Faculty Faculty { get; set; }

        public Lectern Lectern { get; set; }

        public Curriculum Сurriculum { get; set; }

        public UserStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public UserType Type { get; set; }
    }

    public enum UserStatus
    {
        Active,
        Inactive,
        Rejected,
        Submitted
    }

    public enum UserType
    {
        Student,
        Lecturer
    }
}
