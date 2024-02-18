using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Database;
using goodreads.Dtos.User;

namespace goodreads.Mappers
{
    public static class AuthMapper
    {
         public static AppUser ToUser(this RegisterDto registerDto)
        {
            return new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
            };
        }
    }
}