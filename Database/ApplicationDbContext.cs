using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Database.Configuration;
using goodreads.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace goodreads.Database
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
           

            new BookConfiguration().Configure(builder.Entity<Book>());
            new AuthorConfiguration().Configure(builder.Entity<Author>());
            new ReviewConfiguration().Configure(builder.Entity<Review>());
            new RatingConfiguration().Configure(builder.Entity<Rating>());
            new CommentConfiguration().Configure(builder.Entity<Comment>());
            new UserConfiguration().Configure(builder.Entity<AppUser>());
            new LikeConfiguration().Configure(builder.Entity<Like>());
        }

        

    }
}