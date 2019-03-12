using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Programming_Tournament.Areas.Identity.Models;

namespace Programming_Tournament.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Lectern> Lecterns { get; set; }
        public DbSet<Сurriculum> Сurriculums { get; set; }
    }
}
