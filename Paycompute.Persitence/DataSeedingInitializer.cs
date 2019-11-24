using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Paycompute.Persitence
{
    public static class DataSeedingInitializer
    {
        public static async Task UserAndRoleSeedAsync(UserManager<IdentityUser> userManager,
                                                                                                                                                                                        RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Manager", "Staff" };
            foreach (var role in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    IdentityResult result = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Create Admin User
            if (userManager.FindByEmailAsync("vinh.hoang@gmail.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "vinh.hoang@gmail.com",
                    Email = "vinh.hoang@gmail.com"
                };
                IdentityResult identityResult = userManager.CreateAsync(user, "Password1!").Result;
                if (identityResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }

            // Create Manager User
            if (userManager.FindByEmailAsync("manager@gmail.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "manager@gmail.com",
                    Email = "manager@gmail.com"
                };
                IdentityResult identityResult = userManager.CreateAsync(user, "Password1!").Result;
                if (identityResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Manager").Wait();
                }
            }

            // Create Staff User
            if (userManager.FindByEmailAsync("john.doe@gmail.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "john.doe@gmail.com",
                    Email = "john.doe@gmail.com"
                };
                IdentityResult identityResult = userManager.CreateAsync(user, "Password1!").Result;
                if (identityResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Staff").Wait();
                }
            }

            // Create No-role User
            if (userManager.FindByEmailAsync("john.wick@gmail.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "john.wick@gmail.com",
                    Email = "john.wick@gmail.com"
                };
                IdentityResult identityResult = userManager.CreateAsync(user, "Password1!").Result;
                // No role assigned
                //if (identityResult.Succeeded)
                //{
                //				userManager.AddToRoleAsync(user, "Staff").Wait();
                //}
            }
        }
    }
}