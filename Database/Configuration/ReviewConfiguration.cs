using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace goodreads.Database.Configuration
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder
                .HasKey(r => r.Id);

            builder
                .Property(r => r.Content)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .Property(r => r.CreatedAt)
                .IsRequired();

            builder
                .Property(r => r.AppUserId)
                .IsRequired();

            builder
               .Property(r => r.BookId)
               .IsRequired();

            builder
                .HasIndex(r => new { r.AppUserId, r.BookId })
                .IsUnique(true);

            builder
                .HasMany(r=>r.Comments)
                .WithOne()
                .HasForeignKey(c=>c.ReviewId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}