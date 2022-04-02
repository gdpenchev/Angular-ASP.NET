namespace CuriousReaders.Test.Data.Queries;

using CuriousReadersData;
using CuriousReadersData.Entities;
using CuriousReadersData.Queries;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class GenreQueriesTest
{
    private LibraryDbContext fakeDbContext = A.Fake<LibraryDbContext>();

    private void SetupFakeGenreDbSet(IQueryable<Genre> fakeIQueryable)
    {
        var fakeDbSet = A.Fake<DbSet<Genre>>((d =>
                 d.Implements(typeof(IQueryable<Genre>))));

        A.CallTo(() => ((IQueryable<Genre>)fakeDbSet).GetEnumerator())
            .Returns(fakeIQueryable.GetEnumerator());

        A.CallTo(() => ((IQueryable<Genre>)fakeDbSet).Provider)
            .Returns(fakeIQueryable.Provider);

        A.CallTo(() => ((IQueryable<Genre>)fakeDbSet).Expression)
            .Returns(fakeIQueryable.Expression);

        A.CallTo(() => ((IQueryable<Genre>)fakeDbSet).ElementType)
           .Returns(fakeIQueryable.ElementType);

        A.CallTo(() => fakeDbContext.Genres).Returns(fakeDbSet);
    }

    private void SetupFakeBookGenreDbSet(IQueryable<BookGenre> fakeIQueryable)
    {
        var fakeDbSet = A.Fake<DbSet<BookGenre>>((d =>
                 d.Implements(typeof(IQueryable<BookGenre>))));

        A.CallTo(() => ((IQueryable<BookGenre>)fakeDbSet).GetEnumerator())
            .Returns(fakeIQueryable.GetEnumerator());

        A.CallTo(() => ((IQueryable<BookGenre>)fakeDbSet).Provider)
            .Returns(fakeIQueryable.Provider);

        A.CallTo(() => ((IQueryable<BookGenre>)fakeDbSet).Expression)
            .Returns(fakeIQueryable.Expression);

        A.CallTo(() => ((IQueryable<BookGenre>)fakeDbSet).ElementType)
           .Returns(fakeIQueryable.ElementType);

        A.CallTo(() => fakeDbContext.BooksGenres).Returns(fakeDbSet);
    }

    [Fact]
    public void GetAllGenres_Should_ReturnAllGenres_FromDb()
    {
        //Arrange
        var fakeIQueryable = new List<Genre>()
        {
            new Genre() { Id = 1, Name = "Thriller"},
            new Genre() { Id = 2, Name = "Horror"},
            new Genre() { Id = 3, Name = "Adventure"},
            new Genre() { Id = 4, Name = "Fantasy"},
            new Genre() { Id = 5, Name = "Comedy"},
        }
        .AsQueryable();

        SetupFakeGenreDbSet(fakeIQueryable);

        var genreQueries = new GenreQueries(fakeDbContext);

        var expectedResult = fakeIQueryable;

        //Act
        var result = genreQueries.GetAllGenres();

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetExistingGenres_ShouldReturn_AllCurrentlyExistingGenres_FromDb()
    {
        //Arrange
        var fakeIQueryable = new List<Genre>()
        {
            new Genre() { Id = 1, Name = "Thriller"},
            new Genre() { Id = 2, Name = "Horror"},
            new Genre() { Id = 3, Name = "Adventure"},
            new Genre() { Id = 4, Name = "Fantasy"},
            new Genre() { Id = 5, Name = "Comedy"},
        }
        .AsQueryable();

        SetupFakeGenreDbSet(fakeIQueryable);

        var genreQueries = new GenreQueries(fakeDbContext);
        IEnumerable<string> genres = new string[] { "Thriller", "Horror", "Adventure", "Fantasy", "Comedy" };

        var expectedResult = fakeIQueryable.Where(g => genres.Contains(g.Name)).Select(g => g);

        //Act
        var result = genreQueries.GetExistingGenres(genres);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetNewGenres_ShouldReturn_AllGenresWhichAreNotAdded_InDb()
    {
        //Arrange

        var existingGenres = new List<Genre>()
        {
            new Genre() { Id = 1, Name = "Thriller"},
            new Genre() { Id = 2, Name = "Horror"},
            new Genre() { Id = 3, Name = "Adventure"},
        };

        var genreQueries = new GenreQueries(fakeDbContext);
        IEnumerable<string> newGenres = new string[] { "Thriller", "Horror", "Adventure", "Fantasy", "Comedy" };

        //Act
        var result = genreQueries.GetNewGenres(newGenres, existingGenres);

        //Assert
        Assert.NotEqual(result.Count(), existingGenres.Count());
    }

    [Fact]
    public void GetAssignedBookGenresCount_ShouldReturn_AllAssignedBookGenresCount()
    {
        //Arrange

        var fakeIQueryable = new List<BookGenre>()
        {
            new BookGenre()
            {
                GenreId = 1,
                Book = new Book()
                {
                    Status = new Status()
                    {
                        Name = Enumerators.BookStatus.Disabled.ToString()
                    }
                }
            },
            new BookGenre()
            {
                GenreId = 1,
                Book = new Book()
                {
                    Status = new Status()
                    {
                        Name = Enumerators.BookStatus.Enabled.ToString()
                    }
                }
            },
            new BookGenre()
            {
                GenreId = 1,
                Book = new Book()
                {
                    Status = new Status()
                    {
                        Name = Enumerators.BookStatus.Enabled.ToString()
                    }
                }
            },
            new BookGenre()
            {
                GenreId = 2,
                Book = new Book()
                {
                    Status = new Status()
                    {
                        Name = Enumerators.BookStatus.Enabled.ToString()
                    }
                }
            }
        }
        .AsQueryable();

        SetupFakeBookGenreDbSet(fakeIQueryable);

        var genreQueries = new GenreQueries(fakeDbContext);
        var expectedResult = fakeIQueryable
                .Where(b => b.Book.Status.Name == Enumerators.BookStatus.Enabled.ToString())
                .Select(b => b.GenreId)
                .Distinct()
                .Count(); ;

        //Act
        var result = genreQueries.GetAssignedBookGenresCount();

        //Assert
        Assert.Equal(result, expectedResult);
    }
}
