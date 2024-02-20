using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Models;

namespace goodreads.Database.Data
{
    public class AuthorSeeding
    {
        public static async Task Seed(IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

            var authors = new Author[]
            {
                new Author{ Name="Kasper",BornAt= DateTime.Parse("9/24/1961"),DiedAt= DateTime.Parse("6/13/2001"),Summary= "Sed sagittis. Nam congue, risus semper porta volutpat, quam pede lobortis ligula, sit amet eleifend pede libero quis orci. Nullam molestie nibh in lectus."},
                new Author{ Name="Bunnie",BornAt= DateTime.Parse("3/11/1937"),DiedAt= DateTime.Parse("2/22/1995"),Summary= "In hac habitasse platea dictumst. Morbi vestibulum, velit id pretium iaculis, diam erat fermentum justo, nec condimentum neque sapien placerat ante. Nulla justo."},
                new Author{ Name="Jackson",BornAt= DateTime.Parse("2/9/1922"),DiedAt= DateTime.Parse("9/16/1991"),Summary= "Integer ac leo. Pellentesque ultrices mattis odio. Donec vitae nisi."},
                new Author{ Name="Palm",BornAt= DateTime.Parse("8/27/1966"),DiedAt= DateTime.Parse("8/9/2008"),Summary= "Praesent id massa id nisl venenatis lacinia. Aenean sit amet justo. Morbi ut odio."},
                new Author{ Name="Wilden",BornAt= DateTime.Parse("3/24/1946"),DiedAt= DateTime.Parse("8/5/2009"),Summary= "Nulla ut erat id mauris vulputate elementum. Nullam varius. Nulla facilisi."},
                new Author{ Name="Ransom",BornAt= DateTime.Parse("1/28/1914"),DiedAt= DateTime.Parse("6/10/2005"),Summary= "Fusce posuere felis sed lacus. Morbi sem mauris, laoreet ut, rhoncus aliquet, pulvinar sed, nisl. Nunc rhoncus dui vel sem."},
            };

            System.Console.WriteLine(context.Authors.Any());
            System.Console.WriteLine(context.Authors.Any());
            System.Console.WriteLine(context.Authors.Any());
            System.Console.WriteLine(DateTime.Parse("9/24/1961"));
            if (!context.Authors.Any())
            {
                await context.Authors.AddRangeAsync(authors);
                await context.SaveChangesAsync();
            }
        }
    }
}