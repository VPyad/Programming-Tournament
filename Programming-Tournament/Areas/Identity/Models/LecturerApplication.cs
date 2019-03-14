using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Areas.Identity.Models
{
    public class LecturerApplication : BaseApplication
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LecturerApplicationId { get; set; }
    }
}
