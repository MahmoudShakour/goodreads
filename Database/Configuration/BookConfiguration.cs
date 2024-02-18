using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace goodreads.Database.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder
                .Property(b => b.Isbn)
                .IsRequired()
                .HasMaxLength(11);
            
            builder
                .HasIndex(b=>b.Isbn)
                .IsUnique();
            
            builder
                .Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(b => b.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder
                .Property(b => b.Year)
                .IsRequired();

            builder
                .Property(b => b.Genre)
                .IsRequired()
                .HasMaxLength(100);

            builder
               .Property(b => b.Rating)
               .IsRequired();

        }
    }
}