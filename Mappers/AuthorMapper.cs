using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Dtos.Author;
using goodreads.Models;

namespace goodreads.Mappers
{
    public static class AuthorMapper
    {
        public static Author ToAuthor(this CreateAuthorDto createAuthorDto)
        {
            return new Author
            {
                Name = createAuthorDto.Name,
                BornAt = createAuthorDto.BornAt,
                DiedAt = createAuthorDto.DiedAt,
                Summary = createAuthorDto.Summary,
            };
        }

        public static Author ToAuthor(this UpdateAuthorDto updateAuthorDto)
        {
            return new Author
            {
                Id = updateAuthorDto.Id,
                Name = updateAuthorDto.Name,
                BornAt = updateAuthorDto.BornAt,
                DiedAt = updateAuthorDto.DiedAt,
                Summary = updateAuthorDto.Summary,
            };
        }

        public static AuthorDto ToAuthorDto(this Author author)
        {
            return new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                BornAt = author.BornAt,
                DiedAt = author.DiedAt,
                Summary = author.Summary,
            };
        }
    }
}