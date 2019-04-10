﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Programming_Tournament.Data;

namespace ProgrammingTournament.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190409212118_AddExpectedFilePathFieldToTaskEntity")]
    partial class AddExpectedFilePathFieldToTaskEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Programming_Tournament.Models.Domain.Tournaments.StudentTournament", b =>
                {
                    b.Property<string>("ApplicationUserId");

                    b.Property<int>("TournamentId");

                    b.HasKey("ApplicationUserId", "TournamentId");

                    b.HasIndex("TournamentId");

                    b.ToTable("StudentTournaments");
                });

            modelBuilder.Entity("Programming_Tournament.Models.Domain.Tournaments.SupportedProgrammingLanguage", b =>
                {
                    b.Property<int>("SupportedProgrammingLanguageId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code");

                    b.Property<string>("Name");

                    b.Property<int?>("TournamentTaskId");

                    b.HasKey("SupportedProgrammingLanguageId");

                    b.HasIndex("TournamentTaskId");

                    b.ToTable("SupportedProgrammingLanguages");
                });

            modelBuilder.Entity("Programming_Tournament.Models.Domain.Tournaments.Tournament", b =>
                {
                    b.Property<int>("TournamentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Desc");

                    b.Property<DateTime>("DueDate");

                    b.Property<string>("Name");

                    b.Property<string>("OwnerId");

                    b.Property<int>("Status");

                    b.HasKey("TournamentId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Tournaments");
                });

            modelBuilder.Entity("Programming_Tournament.Models.Domain.Tournaments.TournamentTask", b =>
                {
                    b.Property<int>("TournamentTaskId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Desc");

                    b.Property<DateTime>("DueDate");

                    b.Property<string>("ExpectedFilePath");

                    b.Property<string>("InputFilePath");

                    b.Property<int>("MaxAttempt");

                    b.Property<string>("Name");

                    b.Property<string>("OwnerId");

                    b.Property<int?>("Timeout");

                    b.Property<int?>("TournamentId");

                    b.HasKey("TournamentTaskId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("TournamentId");

                    b.ToTable("TournamentTasks");
                });

            modelBuilder.Entity("Programming_Tournament.Models.Domain.Tournaments.TournamentTaskAssignment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Attempts");

                    b.Property<bool>("IsPassed");

                    b.Property<DateTime>("LastAttemptedAt");

                    b.Property<string>("ProcessResultId");

                    b.Property<int?>("TaskTournamentTaskId");

                    b.Property<string>("UserId");

                    b.Property<string>("WorkDir");

                    b.HasKey("Id");

                    b.HasIndex("TaskTournamentTaskId");

                    b.HasIndex("UserId");

                    b.ToTable("TournamentTaskAssignments");
                });

            modelBuilder.Entity("Programming_Tournament.Models.Domain.User.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("DegreeType");

                    b.Property<int?>("DocNo");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<long?>("FacultyId");

                    b.Property<string>("FirstName");

                    b.Property<long?>("LecternId");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecondName");

                    b.Property<string>("SecurityStamp");

                    b.Property<int>("Status");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<int>("Type");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<int?>("YearNo");

                    b.Property<int?>("СurriculumCurriculumId");

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.HasIndex("LecternId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("СurriculumCurriculumId");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Programming_Tournament.Models.Domain.User.Curriculum", b =>
                {
                    b.Property<int>("CurriculumId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("CurriculumId");

                    b.ToTable("Сurriculums");
                });

            modelBuilder.Entity("Programming_Tournament.Models.Domain.User.Faculty", b =>
                {
                    b.Property<long>("FacultyId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("FacultyId");

                    b.ToTable("Faculties");
                });

            modelBuilder.Entity("Programming_Tournament.Models.Domain.User.Lectern", b =>
                {
                    b.Property<long>("LecternId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("LecternId");

                    b.ToTable("Lecterns");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Programming_Tournament.Models.Domain.User.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Programming_Tournament.Models.Domain.User.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Programming_Tournament.Models.Domain.User.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Programming_Tournament.Models.Domain.User.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Programming_Tournament.Models.Domain.Tournaments.StudentTournament", b =>
                {
                    b.HasOne("Programming_Tournament.Models.Domain.User.ApplicationUser", "ApplicationUser")
                        .WithMany("Tournaments")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Programming_Tournament.Models.Domain.Tournaments.Tournament", "Tournament")
                        .WithMany("Assignees")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Programming_Tournament.Models.Domain.Tournaments.SupportedProgrammingLanguage", b =>
                {
                    b.HasOne("Programming_Tournament.Models.Domain.Tournaments.TournamentTask")
                        .WithMany("SupportedLanguages")
                        .HasForeignKey("TournamentTaskId");
                });

            modelBuilder.Entity("Programming_Tournament.Models.Domain.Tournaments.Tournament", b =>
                {
                    b.HasOne("Programming_Tournament.Models.Domain.User.ApplicationUser", "Owner")
                        .WithMany("OwnedTournaments")
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("Programming_Tournament.Models.Domain.Tournaments.TournamentTask", b =>
                {
                    b.HasOne("Programming_Tournament.Models.Domain.User.ApplicationUser", "Owner")
                        .WithMany("OwnedTasks")
                        .HasForeignKey("OwnerId");

                    b.HasOne("Programming_Tournament.Models.Domain.Tournaments.Tournament", "Tournament")
                        .WithMany("Tasks")
                        .HasForeignKey("TournamentId");
                });

            modelBuilder.Entity("Programming_Tournament.Models.Domain.Tournaments.TournamentTaskAssignment", b =>
                {
                    b.HasOne("Programming_Tournament.Models.Domain.Tournaments.TournamentTask", "Task")
                        .WithMany("Assignees")
                        .HasForeignKey("TaskTournamentTaskId");

                    b.HasOne("Programming_Tournament.Models.Domain.User.ApplicationUser", "User")
                        .WithMany("Assignments")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Programming_Tournament.Models.Domain.User.ApplicationUser", b =>
                {
                    b.HasOne("Programming_Tournament.Models.Domain.User.Faculty", "Faculty")
                        .WithMany()
                        .HasForeignKey("FacultyId");

                    b.HasOne("Programming_Tournament.Models.Domain.User.Lectern", "Lectern")
                        .WithMany()
                        .HasForeignKey("LecternId");

                    b.HasOne("Programming_Tournament.Models.Domain.User.Curriculum", "Сurriculum")
                        .WithMany()
                        .HasForeignKey("СurriculumCurriculumId");
                });
#pragma warning restore 612, 618
        }
    }
}
