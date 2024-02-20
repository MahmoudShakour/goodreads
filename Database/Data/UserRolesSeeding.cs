using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace goodreads.Database.Data
{
    public static class UserRolesSeeding
    {

        public static async Task SeedAspNetRolesEntity(RoleManager<IdentityRole> roleManager)
        {

            if (await roleManager.FindByNameAsync("Admin") == null)
                await roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
    
            if (await roleManager.FindByNameAsync("User") == null)
                await roleManager.CreateAsync(new IdentityRole { Name = "User" });
        }
    }
}