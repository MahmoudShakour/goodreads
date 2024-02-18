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
            string userPassword = "123mM!456789";
            AppUser adminUser = new AppUser
            {
                Email = "admin@gmail.com",
                UserName = "MahmoudAbdulshakour",
                PhoneNumber = "0121323112",
            };

            IdentityResult user = await userManager.CreateAsync(adminUser, userPassword);

            if (user.Succeeded)
                await userManager.AddToRoleAsync(adminUser, "Admin");
        }

    }
}