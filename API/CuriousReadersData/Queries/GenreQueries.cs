namespace CuriousReadersData.Queries;

using System.Collections.Generic;
using CuriousReadersData.Entities;
using Microsoft.EntityFrameworkCore;

public class GenreQueries : IGenreQueries
{
    private readonly LibraryDbContext libraryDbContext;

    public GenreQueries(LibraryDbContext libraryDbContext)
    {
        this.libraryDbContext = libraryDbContext;
    }


    public int GetAssignedBookGenresCount()
    {
        var genreTotalCount = this.libraryDbContext.BooksGenres
            .Where(b => b.Book.Status.Name == Enumerators.BookStatus.Enabled.ToString())
            .Select(b => b.GenreId)
            .Distinct()
            .Count();

        return genreTotalCount;
    }

    public IEnumerable<Genre> GetAllGenres()
    {
        var allGenres = this.libraryDbContext.Genres.ToArray();

        return allGenres;
    }

    public List<Genre> GetExistingGenres(IEnumerable<string> genres)
    {
        var existingGenres = libraryDbContext.Genres
           .Where(g => genres.Contains(g.Name))
           .Select(g => g)
           .ToList();

        return existingGenres;
    }

    public List<string> GetNewGenres(IEnumerable<string> genres, List<Genre> existingGenres)
    {
        return genres
        .Select(x => x)
        .Except(existingGenres.Select(a => a.Name))
        .ToList();
    }
}
