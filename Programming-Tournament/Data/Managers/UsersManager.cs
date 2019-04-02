using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Programming_Tournament.Areas.Admin.Models;
using Programming_Tournament.Areas.Identity.Models;
using Programming_Tournament.Data;
using Programming_Tournament.Models.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Programming_Tournament.Areas.Admin.Pages.Users.DetailsModel.UserModel;

namespace Programming_Tournament.Data.Managers
{
    public class UsersManager
    {
        public static async Task CreateSuperUser(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var poweruser = new ApplicationUser
            {
                UserName = configuration.GetSection("UserSettings")["UserEmail"],
                Email = configuration.GetSection("UserSettings")["UserEmail"],
                Status = UserStatus.Active
            };
            
            string userPassword = configuration.GetSection("UserSettings")["UserPassword"];
            var user = await UserManager.FindByEmailAsync(configuration.GetSection("UserSettings")["UserEmail"]);
            
            if (user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, userPassword);
                if (createPowerUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(poweruser, "Admin");
                }
            }
        }

        public static async Task CreateUser(IServiceProvider serviceProvider, IConfiguration configuration, BaseApplication application)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var appUser = new ApplicationUser(application);

            string userPassword = application.Password;
            string userEmail = application.Email;

            appUser.UserName = userEmail;
            appUser.Email = userEmail;
            var user = await userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                var createUser = await userManager.CreateAsync(appUser, userPassword);
                if (createUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(appUser, GetRole(application.UserType));
                }
            }
        }

        public static async Task CreateUser(IServiceProvider serviceProvider, IConfiguration configuration, ApplicationUser appUser, string password, string email, UserType userType)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            appUser.Email = email;
            appUser.UserName = email;
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                var createUser = await userManager.CreateAsync(appUser, password);
                if (createUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(appUser, GetRole(userType));
                }
            }
        }

        private static string GetRole(UserType userType)
        {
            switch (userType)
            {
                case UserType.Lecturer:
                    return "Lecturer";
                case UserType.Student:
                    return "Student";
                default:
                    return "Student";
            }
        }

        public static bool CanSignIn(ApplicationDbContext context, string email)
        {
            return context.Users.Any(x => x.Email == email)
                && context.Users.FirstOrDefault(x => x.Email == email).Status == UserStatus.Active;
        }

        public static ApplicationUser GetUser(ApplicationDbContext context, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            var user = context.Users.Include(x => x.Faculty)
                .Include(x => x.Сurriculum)
                .Include(x => x.Lectern)
                .FirstOrDefault(x => x.Id == userId);

            return user;
        }

        public static async Task<IEnumerable<string>> GetRolesAsync(IServiceProvider serviceProvider, IConfiguration configuration, ApplicationUser user)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var roles = await userManager.GetRolesAsync(user);

            return roles;
        }

        public static void ChangeUserStatus(ApplicationDbContext context, string userId, UserStatus userStatus)
        {
            if (string.IsNullOrEmpty(userId))
                return;

            var user = context.Users.FirstOrDefault(x => x.Id == userId);

            if (user == null)
                return;

            user.Status = userStatus;

            context.Users.Update(user);
            context.SaveChanges();
        }

        public static void UpdateUser(ApplicationDbContext context, ApplicationUser user)
        {
            if (user == null)
                return;

            var dbUser = context.Users.FirstOrDefault(x => x.Id == user.Id);

            if (dbUser == null)
                return;

            context.Users.Update(user);
            context.SaveChanges();
        }
    }
}
