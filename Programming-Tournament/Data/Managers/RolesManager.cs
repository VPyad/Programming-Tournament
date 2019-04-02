using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Data.Managers
{
    public static class RolesManager
    {
        private static readonly string[] roles = { "Admin", "Lecturer", "Student" };

        public static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            IdentityResult roleResult;

            foreach (var role in roles)
            {
                var roleExist = await RoleManager.RoleExistsAsync(role);

                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
