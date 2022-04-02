namespace CuriousReadersData.Queries;

using System.Collections.Generic;
using System.Linq;

using CuriousReadersData.Entities;

public class AuthorQueries : IAuthorQueries
{
    private readonly LibraryDbContext libraryDbContext;

    public AuthorQueries(LibraryDbContext libraryDbContext)
    {
        this.libraryDbContext = libraryDbContext;
    }

    public IEnumerable<Author> GetAllAuthors()
    {
        var allAuthors = libraryDbContext.Authors.ToArray();

        return allAuthors;
    }

    public List<Author> GetExistingAuthors(IEnumerable<string> authors)
    {
        var existingAuthors = libraryDbContext.Authors
            .Where(a => authors.Contains(a.Name))
            .Select(a => a)
            .ToList();

        return existingAuthors;
    }

    public List<string> GetNewAuthors(IEnumerable<string> authors, List<Author> existingAuthors)
    {
        return authors
         .Select(x => x)
         .Except(existingAuthors.Select(a => a.Name))
         .ToList();
    }
}

