using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Models;
using Microsoft.AspNetCore.Identity;

namespace goodreads.Database
{
    public class AppUser : IdentityUser
    {
        public List<Review> Reviews { get; set; }
        public List<Rating> Ratings { get; set; }
        public List<Comment> Comments { get; set; }
    }
}