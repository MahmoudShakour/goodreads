using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace goodreads.Database.Configuration
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder
                .HasKey(c => c.Id);

            builder
                .Property(c => c.Content)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .Property(c => c.CreatedAt)
                .IsRequired();
            
            builder
                .Property(c => c.AppUserId)
                .IsRequired();

            builder
                .Property(c => c.ReviewId)
                .IsRequired();
        }
    }
}