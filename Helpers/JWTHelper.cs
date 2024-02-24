using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using goodreads.Helpers.Contracts;
using Microsoft.IdentityModel.Tokens;

namespace goodreads.Helpers
{
    public class JWTHelper : IJWTHelper
    {

        private IConfiguration _configuration;
        private SymmetricSecurityKey key;
        private string? token;
        public JWTHelper(IConfiguration configuration,IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SigningKey"]));
            token = httpContextAccessor?.HttpContext?.Request.Headers.Authorization;
        }
        public TokenInfo? DecodeToken()
        {
            if (token == null)
                return null;

            token = token.Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler().ValidateToken(
                token,
                new TokenValidationParameters()
                {
                    IssuerSigningKey = key,
                    ValidIssuer = _configuration["JWT:Issuer"],
                    ValidAudience = _configuration["JWT:Audience"],
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                },
                out SecurityToken securityToken
            );
            string id = handler.Claims.First(claim => claim.Type == "UserId").Value;
            string role = handler.Claims
                .First(claim => claim.Type == "RoleName")
                .Value;

            return new TokenInfo { Id = id, Role = role };
        }

        public string GenerateToken(string email, string userId, string roleName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim("UserId", userId),
                new Claim("RoleName", roleName),
            };

            var signingCredentials = new SigningCredentials(
                this.key,
                SecurityAlgorithms.HmacSha256Signature
            );

            var description = new SecurityTokenDescriptor()
            {
                Issuer = _configuration["JWT:Issuer"],
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(30),
                Audience = _configuration["JWT:Audience"],
                SigningCredentials = signingCredentials,
                Subject = new ClaimsIdentity(claims),
            };

            var handler = new JwtSecurityTokenHandler();
            var securedToken = handler.CreateToken(description);

            return handler.WriteToken(securedToken);
        }
    }
}