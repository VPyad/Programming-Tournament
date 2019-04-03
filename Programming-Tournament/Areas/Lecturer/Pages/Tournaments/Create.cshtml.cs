using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Programming_Tournament.Areas.Lecturer.Pages.Tournaments
{
    [Authorize(Roles = "Admin,Lecturer")]
    public class CreateModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}