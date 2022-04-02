namespace CuriousReadersData.Queries;

using System.Collections.Generic;

using CuriousReadersData.Entities;

public interface IAuthorQueries
{
    IEnumerable<Author> GetAllAuthors();

    List<Author> GetExistingAuthors(IEnumerable<string> authors);

    List<string> GetNewAuthors(IEnumerable<string> authors, List<Author> existingAuthors);
}