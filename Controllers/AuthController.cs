using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Dtos.User;
using goodreads.Mappers;
using goodreads.Repos;
using Microsoft.AspNetCore.Mvc;

namespace goodreads.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepo authRepo;
        public AuthController(IAuthRepo authRepo)
        {
            this.authRepo = authRepo;
        }

        [HttpPost]
        [Route("/register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    new
                    {
                        success = false,
                        statusCode = 400,
                        message = ModelState.ValidationState
                    }
                );
            }

            var isDuplicate = await authRepo.IsDuplicate(registerDto.Email, registerDto.Username);
            if (isDuplicate)
            {
                return BadRequest(
                    new
                    {
                        success = false,
                        statusCode = 400,
                        message = "duplicate email or username"
                    }
                );
            }

            var user = registerDto.ToUser();
            var isCreated = await authRepo.CreateUser(user, registerDto.Password);
            if (isCreated)
            {
                return Created(
                    "User created successfully",
                    new
                    {
                        success = true,
                        statusCode = 201,
                        message = "User created successfully."
                    }
                );
            }

            return BadRequest(
                new
                {
                    success = false,
                    statusCode = 400,
                    message = "password must contain lower, upper letters, numbers, and symbols."
                }
            );
        }


        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    new
                    {
                        success = false,
                        statusCode = 400,
                        message = ModelState.ValidationState
                    }
                );
            }

            var generatedToken = await authRepo.LoginUser(loginDto.Username, loginDto.Password);
            if (generatedToken == null)
            {
                return Unauthorized(
                    new
                    {
                        success = false,
                        statusCode = 401,
                        message = "username or password is not correct."
                    }
                );
            }

            return
                Ok(
                new
                {
                    success = true,
                    statusCode = 200,
                    token = generatedToken
                }
            );
        }
    }
}