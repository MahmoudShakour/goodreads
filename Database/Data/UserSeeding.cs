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
            // eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6IkFkbWluMTIzQGFkbWluLmNvbSIsIlVzZXJJZCI6ImM0MzNkNjZmLTNmYmQtNDM4YS05ODIyLTY1NmZjNTgxYzI0MCIsIlJvbGVOYW1lIjoiQWRtaW4iLCJuYmYiOjE3MDg0MTA3MjcsImV4cCI6MTcxMTAwMjcyNywiaWF0IjoxNzA4NDEwNzI3LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUyNDYiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjUyNDYifQ.39LFLJfV5pTgodfUzvz7F_cSe8pETouMU4NYIgRtHq4
            
            var admin = await userManager.CreateAsync(adminUser, userPassword);

            if (admin.Succeeded)
                await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}