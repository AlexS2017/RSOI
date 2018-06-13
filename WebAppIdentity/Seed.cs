
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppIdentity.Models;

namespace WebAppIdentity
{
    public static class Seed
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "User" };

            IdentityResult roleResult;

            foreach (string roleName in roleNames)
            {
                bool roleExist = await RoleManager.RoleExistsAsync(roleName);

                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            ApplicationUser poweruser = new ApplicationUser
            {
                UserName = configuration["SA:UserEmail"],
                Email = configuration["SA:UserEmail"]
            };

            string userPassword = configuration["SA:UserPwd"];

            ApplicationUser user = await UserManager.FindByEmailAsync(poweruser.Email);

            if (user == null)
            {
                IdentityResult createPowerUser = await UserManager.CreateAsync(poweruser, userPassword);

                if (createPowerUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(poweruser, "Admin");
                }
            }

        }
    }
}
