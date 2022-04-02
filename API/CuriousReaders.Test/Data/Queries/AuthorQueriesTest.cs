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

public class AuthorQueriesTest
{
    private LibraryDbContext fakeDbContext = A.Fake<LibraryDbContext>();

    private void SetupFakeDbSet(IQueryable<Author> fakeIQueryable)
    {
        var fakeDbSet = A.Fake<DbSet<Author>>((d =>
                 d.Implements(typeof(IQueryable<Author>))));

        A.CallTo(() => ((IQueryable<Author>)fakeDbSet).GetEnumerator())
            .Returns(fakeIQueryable.GetEnumerator());

        A.CallTo(() => ((IQueryable<Author>)fakeDbSet).Provider)
            .Returns(fakeIQueryable.Provider);

        A.CallTo(() => ((IQueryable<Author>)fakeDbSet).Expression)
            .Returns(fakeIQueryable.Expression);

        A.CallTo(() => ((IQueryable<Author>)fakeDbSet).ElementType)
           .Returns(fakeIQueryable.ElementType);

        A.CallTo(() => fakeDbContext.Authors).Returns(fakeDbSet);
    }


    [Fact]
    public void GetAllAuthors_Should_ReturnAllAuthors_FromDb()
    {
        //Arrange
        var fakeIQueryable = new List<Author>()
        {
            new Author() { Id = 1, Name = "Stephen King"},
            new Author() { Id = 2, Name = "George R.R. Martin"},
            new Author() { Id = 3, Name = "Agatha Christie"}
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var authorQueries = new AuthorQueries(fakeDbContext);

        var expectedResult = fakeIQueryable;

        //Act
        var result = authorQueries.GetAllAuthors();

        //Assert
        Assert.Equal(expectedResult, result.AsQueryable());
    }

    [Fact]
    public void GetExistingAuthors_ShouldReturn_AllCurrentlyExistingAuthors_FromDb()
    {
        //Arrange
        var fakeIQueryable = new List<Author>()
         {
            new Author() { Id = 1, Name = "Stephen King"},
            new Author() { Id = 2, Name = "George R.R. Martin"},
            new Author() { Id = 3, Name = "Agatha Christie"}
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var authorQueries = new AuthorQueries(fakeDbContext);
        IEnumerable<string> authors = new string[] { "Stephen King", "George R.R. Martin", "Agatha Christie" };

        var expectedResult = fakeIQueryable.Where(a => authors.Contains(a.Name)).Select(a => a);

        //Act
        var result = authorQueries.GetExistingAuthors(authors);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetNewAuthors_ShouldReturn_AllAuthorsWhichAreNotAdded_InDb()
    {
        //Arrange

        var existingAuthors = new List<Author>()
         {
            new Author() { Id = 1, Name = "Stephen King"}
        };

        var AuthorQueries = new AuthorQueries(fakeDbContext);
        IEnumerable<string> authors = new string[] { "Stephen King", "George R.R. Martin", "Agatha Christie" };

        //Act
        var result = AuthorQueries.GetNewAuthors(authors, existingAuthors);

        //Assert
        Assert.NotEqual(result.Count(), existingAuthors.Count());
    }
}
