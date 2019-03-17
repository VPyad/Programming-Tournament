﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Programming_Tournament.Areas.Identity.Models;
using Programming_Tournament.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Areas.Identity.Managers
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
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var appUser = new ApplicationUser(application);

            string userPassword = application.Password;
            string userEmail = application.Email;

            appUser.UserName = userEmail;
            appUser.Email = userEmail;
            var user = await UserManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                var createUser = await UserManager.CreateAsync(appUser, userPassword);
                if (createUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(appUser, GetRole(application.ApplicationType));
                }
            }
        }

        private static string GetRole(ApplicationType applicationType)
        {
            switch (applicationType)
            {
                case ApplicationType.Lecturer:
                    return "Lecturer";
                case ApplicationType.Student:
                    return "Student";
                default:
                    return "";
            }
        }

        public static bool CanSignIn(ApplicationDbContext context, string email)
        {
            return context.Users.Any(x => x.Email == email)
                && context.Users.FirstOrDefault(x => x.Email == email).Status == UserStatus.Active;
        }
    }
}
