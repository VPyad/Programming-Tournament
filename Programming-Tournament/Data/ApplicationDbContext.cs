using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Programming_Tournament.Areas.Identity.Models;
using Programming_Tournament.Models.Domain.Tournaments;
using Programming_Tournament.Models.Domain.User;

namespace Programming_Tournament.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Lectern> Lecterns { get; set; }
        public DbSet<Curriculum> Сurriculums { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<TournamentTask> TournamentTasks { get; set; }
        public DbSet<TournamentTaskAssignment> TournamentTaskAssignments { get; set; }
        public DbSet<TournamentTaskAssignmentResult> TournamentTaskAssignmentResults { get; set; }
        public DbSet<StudentTournament> StudentTournaments { get; set; }
        public DbSet<SupportedProgrammingLanguage> SupportedProgrammingLanguages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<StudentTournament>()
                .HasKey(st => new { st.ApplicationUserId, st.TournamentId });

            builder.Entity<StudentTournament>()
                .HasOne(st => st.ApplicationUser)
                .WithMany(x => x.Tournaments)
                .HasForeignKey(x => x.ApplicationUserId);

            builder.Entity<StudentTournament>()
                .HasOne(x => x.Tournament)
                .WithMany(x => x.Assignees)
                .HasForeignKey(x => x.TournamentId);

            // Tournament
            builder.Entity<Tournament>()
                .HasOne(x => x.Owner)
                .WithMany(x => x.OwnedTournaments);

            base.OnModelCreating(builder);
        }
    }
}
