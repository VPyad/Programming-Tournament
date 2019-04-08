using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Programming_Tournament.Areas.Student.Pages.Tournaments
{
    [Authorize(Roles = "Admin,Lecturer,Student")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}