using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Helpers;
using goodreads.Helpers.Contracts;
using goodreads.Repos.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace goodreads.Controllers
{
    [ApiController]
    [Route("/api/book")]
    public class BookController:ControllerBase
    {
        private readonly IBookRepo _bookRepo;
        private readonly TokenInfo tokenInfo;

        public BookController(IBookRepo bookRepo,IHttpContextAccessor httpContextAccessor, IJWTHelper jWTHelper)
        {
            _bookRepo=bookRepo;
            var token = httpContextAccessor?.HttpContext?.Request.Headers.Authorization;
            tokenInfo = jWTHelper.DecodeToken(token);
        }

        
        
    }
}