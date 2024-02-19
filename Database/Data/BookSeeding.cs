using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Models;

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
                NumberOfPages = 253
            },
            new Book
            {
                Isbn = "392809555-2",
                Title = "Lipstick",
                Description="a description of the book",
                Year = 1996,
                Rating = 1.2,
                Genre = "Drama",
                NumberOfPages = 435
            },
            new Book
            {
                Isbn = "389395282-9",
                Title = "Ryan''s Daughter",
                Description="a description of the book",
                Year = 2002,
                Rating = 2.7,
                Genre = "Drama|Romance",
                NumberOfPages = 441
            },
            new Book
            {
                Isbn = "154710961-0",
                Title = "Quinceañera",
                Description="a description of the book",
                Year = 1990,
                Rating = 0.1,
                Genre = "Drama",
                NumberOfPages = 156
            },
            new Book
            {
                Isbn = "524628287-2",
                Title = "Girls on Top",
                Description="a description of the book",
                Year = 2012,
                Rating = 0.4,
                Genre = "Comedy",
                NumberOfPages = 295
            },
            new Book
            {
                Isbn = "576963128-7",
                Title = "Two Lives (Zwei Leben)",
                Description="a description of the book",
                Year = 1985,
                Rating = 4.5,
                Genre = "Drama|Thriller",
                NumberOfPages = 383
            },
            new Book
            {
                Isbn = "623183730-0",
                Title = "C.R.A.Z.Y.",
                Description="a description of the book",
                Year = 1993,
                Rating = 3.3,
                Genre = "Drama",
                NumberOfPages = 251
            },
            new Book
            {
                Isbn = "229997349-4",
                Title = "Return of Martin Guerre, The (Retour de Martin Guerre, Le)",
                Description="a description of the book",
                Year = 2003,
                Rating = 0.2,
                Genre = "Drama",
                NumberOfPages = 343
            },
            new Book
            {
                Isbn = "926488395-9",
                Title = "Cheerleader Massacre",
                Description="a description of the book",
                Year = 2006,
                Rating = 4.7,
                Genre = "Horror|Thriller",
                NumberOfPages = 297
            },
            new Book
            {
                Isbn = "849443241-9",
                Title = "Green Butchers, The (Grønne slagtere, De)",
                Description="a description of the book",
                Year = 1994,
                Rating = 2.4,
                Genre = "Comedy|Crime|Drama|Romance",
                NumberOfPages = 414
            },
            new Book
            {
                Isbn = "592757595-1",
                Title = "Last Valley, The",
                Description="a description of the book",
                Year = 1992,
                Rating = 4.1,
                Genre = "Adventure",
                NumberOfPages = 426
            },
            new Book
            {
                Isbn = "874719639-4",
                Title = "From Paris with Love",
                Description="a description of the book",
                Year = 1995,
                Rating = 2.3,
                Genre = "Action|Crime",
                NumberOfPages = 413
            },
            new Book
            {
                Isbn = "887096793-X",
                Title = "Voyage to Cythera (Taxidi sta Kythira)",
                Description="a description of the book",
                Year = 2012,
                Rating = 4.8,
                Genre = "Drama",
                NumberOfPages = 231
            },
            new Book
            {
                Isbn = "867037154-5",
                Title = "Botany of Desire, The",
                Description="a description of the book",
                Year = 1992,
                Rating = 1.7,
                Genre = "Documentary",
                NumberOfPages = 490
            },
            new Book
            {
                Isbn = "823651516-8",
                Title = "Kazaam",
                Description="a description of the book",
                Year = 1991,
                Rating = 0.8,
                Genre = "Children|Comedy|Fantasy",
                NumberOfPages = 363
            },
            new Book
            {
                Isbn = "868997465-2",
                Title = "Death Racers",
                Description="a description of the book",
                Year = 2000,
                Rating = 2.5,
                Genre = "Action|Adventure|Comedy|Sci-Fi|Thriller",
                NumberOfPages = 140
            },
            new Book
            {
                Isbn = "611366177-6",
                Title = "Life After People",
                Description="a description of the book",
                Year = 2004,
                Rating = 4.9,
                Genre = "Documentary",
                NumberOfPages = 121
            },
            new Book
            {
                Isbn = "588794710-1",
                Title = "C.S.A.: The Confederate States of America",
                Description="a description of the book",
                Year = 2000,
                Rating = 0.5,
                Genre = "Comedy|Drama",
                NumberOfPages = 476
            },
            new Book
            {
                Isbn = "188119986-X",
                Title = "Model",
                Description="a description of the book",
                Year = 2012,
                Rating = 3.8,
                Genre = "Documentary",
                NumberOfPages = 468
            },
            };


            if (!context.Books.Any())
            {
                await context.Books.AddRangeAsync(books);
                await context.SaveChangesAsync();
            }
        }
    }
}