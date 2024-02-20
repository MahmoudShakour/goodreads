using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Database.Data;
using goodreads.Database.Seeding;
using Microsoft.AspNetCore.Identity;

namespace goodreads.Database
{
    public class DbSeeding
    {
        public async static Task SeedData(IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

            var roleManager = serviceScope.ServiceProvider.GetRequiredService<
                RoleManager<IdentityRole>
            >();
            var userManager = serviceScope.ServiceProvider.GetRequiredService<
                UserManager<AppUser>
            >();

            // Seed data
            await UserRolesSeeding.SeedAspNetRolesEntity(roleManager);
            await UserSeeding.Seed(userManager);
            await AuthorSeeding.Seed(applicationBuilder);
            await BookSeeding.Seed(applicationBuilder);
        }
    }
}