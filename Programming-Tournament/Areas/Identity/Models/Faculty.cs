using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Areas.Identity.Models
{
    public class Faculty
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long FacultyId { get; set; }

        public string Name { get; set; }
    }

    public enum DegreeType
    {
        Bachelor,
        Master,
        PhD,
        Unknown
    }
}
