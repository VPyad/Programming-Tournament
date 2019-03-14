﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Areas.Identity.Models
{
    public class StudentApplication : BaseApplication
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long StudentApplicationId { get; set; }

        public DegreeType DegreeType { get; set; }

        public Сurriculum Сurriculum { get; set; }
    }
}
