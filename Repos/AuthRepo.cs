using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Database;
using goodreads.Helpers.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace goodreads.Repos
{
    public class AuthRepo : IAuthRepo
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IJWTHelper jWTHelper;

        public AuthRepo(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJWTHelper jWTHelper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jWTHelper = jWTHelper;
        }

        public async Task<bool> IsDuplicate(string email, string username)
        {
            return await userManager.FindByEmailAsync(email) != null ||
                    await userManager.FindByNameAsync(username) != null;
        }

        public async Task<bool> CreateUser(AppUser appUser, string password)
        {
            var createdUser = await userManager.CreateAsync(appUser, password);
            if (!createdUser.Succeeded)
            {
                return false;
            }
            await userManager.AddToRoleAsync(appUser, "User");
            return true;
        }

        public async Task<string?> LoginUser(string username, string password)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                return null;
            }

            var checkPassword = await signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!checkPassword.Succeeded)
            {
                return null;
            }

            var token = jWTHelper.GenerateToken(user.Email, user.Id, "User");
            return token;
        }
    }
}