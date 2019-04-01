using Programming_Tournament.Areas.Identity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Areas.Admin.Models
{
    public class ApplicationUserEditPageModel
    {
        public ApplicationUserEditPageModel() { }

        public ApplicationUserEditPageModel(ApplicationUser user)
        {
            InitFiels(user);

            if (user.Type == UserType.Student)
            {
                UserType = UserType.Student;
                Curriculum = user.Сurriculum;
                CurriculumId = user.Сurriculum.CurriculumId;
                YearNo = user.YearNo;
                DegreeType = user.DegreeType;
            }
        }

        private void InitFiels(ApplicationUser user)
        {
            Email = user.Email;
            FirstName = user.FirstName;
            SecondName = user.SecondName;
            DocNo = user.DocNo;
            Faculty = user.Faculty;
            FacultyId = user.Faculty.FacultyId;
            Lectern = user.Lectern;
            LecternId = user.Lectern.LecternId;
            UserStatus = user.Status;
            UserType = UserType.Lecturer;
        }

        [Required]
        [Display(Name = "Email")]
        public virtual string Email { get; set; }

        [Required]
        [Display(Name = "First name")]
        public virtual string FirstName { get; set; }

        [Required]
        [Display(Name = "Second name")]
        public virtual string SecondName { get; set; }

        [Display(Name = "Document number")]
        public int? DocNo { get; set; }

        [Display(Name = "Year of education")]
        public int? YearNo { get; set; }

        [Display(Name = "Degree")]
        public DegreeType DegreeType { get; set; }

        [Display(Name = "Faculty")]
        public long FacultyId { get; set; }
        
        public Faculty Faculty { get; set; }

        [Display(Name = "Lectern")]
        public long LecternId{ get; set; }

        public Lectern Lectern { get; set; }

        [Display(Name = "Curriculum")]
        public int CurriculumId { get; set; }

        public Curriculum Curriculum { get; set; }

        [Display(Name = "Status")]
        public UserStatus UserStatus { get; set; }

        public UserType UserType { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        public static void ApplyChanges(ApplicationUser user, ApplicationUserEditPageModel editPageModel, IEnumerable<Faculty> faculties, IEnumerable<Lectern> lecterns, IEnumerable<Curriculum> curriculums)
        {
            if (user.FirstName != editPageModel.FirstName)
                user.FirstName = editPageModel.FirstName;

            if (user.SecondName != editPageModel.SecondName)
                user.SecondName = editPageModel.SecondName;

            if (user.DocNo != editPageModel.DocNo)
                user.DocNo = editPageModel.DocNo;

            if (user.YearNo != editPageModel.YearNo)
                user.YearNo = editPageModel.YearNo;

            if (user.DegreeType != editPageModel.DegreeType)
                user.DegreeType = editPageModel.DegreeType;

            if (user.Faculty.FacultyId != editPageModel.FacultyId)
            {
                var faculty = faculties.FirstOrDefault(x => x.FacultyId == editPageModel.FacultyId);
                if (faculty != null)
                    user.Faculty = faculty;
            }

            if (user.Lectern.LecternId != editPageModel.LecternId)
            {
                var lectern = lecterns.FirstOrDefault(x => x.LecternId == editPageModel.LecternId);
                if (lectern != null)
                    user.Lectern = lectern;
            }

            if (user.Сurriculum != null && user.Сurriculum.CurriculumId != editPageModel.CurriculumId)
            {
                var cur = curriculums.FirstOrDefault(x => x.CurriculumId == editPageModel.CurriculumId);
                if (cur != null)
                    user.Сurriculum = cur;
            }

            if (user.Status != editPageModel.UserStatus)
                user.Status = editPageModel.UserStatus;
        }

        public static ApplicationUser ComposeApplicationUser(ApplicationUserEditPageModel editPageModel, IEnumerable<Faculty> faculties, IEnumerable<Lectern> lecterns, IEnumerable<Curriculum> curriculums)
        {
            ApplicationUser user = new ApplicationUser
            {
                FirstName = editPageModel.FirstName,
                SecondName = editPageModel.SecondName,
                DocNo = editPageModel.DocNo,
                Status = editPageModel.UserStatus,
                CreatedAt = DateTime.Now
            };

            var faculty = faculties.FirstOrDefault(x => x.FacultyId == editPageModel.FacultyId);
            if (faculty != null)
                user.Faculty = faculty;

            var lectern = lecterns.FirstOrDefault(x => x.LecternId == editPageModel.LecternId);
            if (lectern != null)
                user.Lectern = lectern;

            switch (editPageModel.UserType)
            {
                case UserType.Student:
                    user.DegreeType = editPageModel.DegreeType;
                    user.YearNo = editPageModel.YearNo;
                    user.Type = UserType.Student;

                    var curriculum = curriculums.FirstOrDefault(x => x.CurriculumId == editPageModel.CurriculumId);
                    if (curriculum != null)
                        user.Сurriculum = curriculum;
                    break;
                case UserType.Lecturer:
                    user.DegreeType = DegreeType.Unknown;
                    user.Type = UserType.Lecturer;
                    break;
            }

            return user;
        }
    }
}
