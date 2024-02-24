using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace goodreads.Helpers.Contracts
{
    public interface IJWTHelper
    {
        string GenerateToken(string email, string userId, string roleName);
        TokenInfo? DecodeToken();

    }
}