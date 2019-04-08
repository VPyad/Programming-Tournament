using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Programming_Tournament.Areas.Student.Pages.Tasks
{
    [Authorize(Roles = "Admin,Lecturer,Student")]
    public class DetailsModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}