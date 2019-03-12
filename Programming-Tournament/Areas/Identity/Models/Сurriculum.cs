using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Areas.Identity.Models
{
    public class Сurriculum
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int СurriculumId { get; set; }

        public string Name { get; set; }
    }
}
