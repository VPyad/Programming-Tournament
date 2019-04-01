﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Areas.Identity.Models
{
    public class Lectern
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LecternId { get; set; }

        public string Name { get; set; }
    }
}
