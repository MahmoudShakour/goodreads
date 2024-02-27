using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace goodreads.Database.Seeding
{
    public static class UserSeeding
    {
        public static async Task Seed(UserManager<AppUser> userManager)
        {
            string userPassword = "NewAdminPassword@123";
            AppUser adminUser = new AppUser
            {
                Email = "Admin123@admin.com",
                UserName = "mahmoudshakourali",
                EmailConfirmed = true,
                PhoneNumber = "01062591395",
            };
            
            var admin = await userManager.CreateAsync(adminUser, userPassword);

            if (admin.Succeeded)
                await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}