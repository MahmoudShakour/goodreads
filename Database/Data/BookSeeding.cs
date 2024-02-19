using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Models;
using Microsoft.EntityFrameworkCore;

namespace goodreads.Database.Data
{
    public static class BookSeeding
    {
        public static async Task Seed(IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            
            
            Book[] books = new Book[]{
            new Book
            {
                Isbn = "539278809-2",
                Title = "Delta Farce",
                Description="a description of the book",
                Year = 1992,
                Rating = 2.6,
                Genre = "Action|Adventure|Comedy",
                NumberOfPages = 253,
                Author=new Author
                {
                    Name="Jackson",
                    BornAt= DateTime.Parse("2/9/1922"),
                    DiedAt= DateTime.Parse("9/16/1991"),
                    Summary= "Integer ac leo. Pellentesque ultrices mattis odio. Donec vitae nisi."
                },
            },
            new Book
            {
                Isbn = "392809555-2",
                Title = "Lipstick",
                Description="a description of the book",
                Year = 1996,
                Rating = 1.2,
                Genre = "Drama",
                NumberOfPages = 435,
                Author=new Author
                {
                    Name="sallo",
                    BornAt= DateTime.Parse("2/9/1930"),
                    DiedAt= DateTime.Parse("9/16/1959"),
                    Summary= "Integer ac leo. Pellentesque ultrices mattis odio. Donec vitae nisi."
                },
            },
            new Book
            {
                Isbn = "389395282-9",
                Title = "Ryan''s Daughter",
                Description="a description of the book",
                Year = 2002,
                Rating = 2.7,
                Genre = "Drama|Romance",
                NumberOfPages = 441,
                Author=new Author
                {
                    Name="hallom",
                    BornAt= DateTime.Parse("1/1/1922"),
                    DiedAt= DateTime.Parse("1/16/2005"),
                    Summary= "Integer ac leo. Pellentesque ultrices mattis odio. Donec vitae nisi."
                },
            }
            };


            if (!context.Books.Any())
            {
                await context.Books.AddRangeAsync(books);
                await context.SaveChangesAsync();
            }
        }
    }
}