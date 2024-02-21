using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace goodreads.Database.Configuration
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder
                .HasKey(r => r.Id);

            builder
                .HasIndex(r => new { r.AppUserId, r.BookId })
                .IsUnique(true);

            builder
                .Property(r => r.RateValue)
                .IsRequired();

            builder
                .Property(r => r.AppUserId)
                .IsRequired();

            builder
                .Property(r => r.BookId)
                .IsRequired();

        }
    }
}