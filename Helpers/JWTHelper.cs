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

        private IConfiguration configuration;
        private SymmetricSecurityKey key;
        public JWTHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
            key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SigningKey"]));
        }
        public TokenInfo DecodeToken(string? token)
        {
            if (token == null)
            {
                return new TokenInfo();
            }
            token = token.Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler().ValidateToken(
                token,
                new TokenValidationParameters()
                {
                    IssuerSigningKey = key,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                },
                out SecurityToken securityToken
            );
            string id = handler.Claims.First(claim => claim.Type == "UserId").Value;
            Console.WriteLine("token:" + id);
            string role = handler.Claims
                .First(claim => claim.Type == "RoleName")
                .Value;
            Console.WriteLine("token:" + role);


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
                Issuer = configuration["JWT:Issuer"],
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(30),
                Audience = configuration["JWT:Audience"],
                SigningCredentials = signingCredentials,
                Subject = new ClaimsIdentity(claims),
            };

            var handler = new JwtSecurityTokenHandler();
            var securedToken = handler.CreateToken(description);

            return handler.WriteToken(securedToken);
        }
    }
}