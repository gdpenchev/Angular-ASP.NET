namespace CuriousReaders.Test.Data.Queries;

using CuriousReadersData;
using CuriousReadersData.Entities;
using CuriousReadersData.Queries;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

public class CommentQueriesTest
{
    private LibraryDbContext fakeDbContext = A.Fake<LibraryDbContext>();

    private void SetupFakeDbSet(IQueryable<Comment> fakeIQueryable)
    {
        var fakeDbSet = A.Fake<DbSet<Comment>>((d =>
                 d.Implements(typeof(IQueryable<Comment>))));

        A.CallTo(() => ((IQueryable<Comment>)fakeDbSet).GetEnumerator())
            .Returns(fakeIQueryable.GetEnumerator());

        A.CallTo(() => ((IQueryable<Comment>)fakeDbSet).Provider)
            .Returns(fakeIQueryable.Provider);

        A.CallTo(() => ((IQueryable<Comment>)fakeDbSet).Expression)
            .Returns(fakeIQueryable.Expression);

        A.CallTo(() => ((IQueryable<Comment>)fakeDbSet).ElementType)
           .Returns(fakeIQueryable.ElementType);

        A.CallTo(() => fakeDbContext.Comments).Returns(fakeDbSet);
    }

    [Fact]
    public void CountUnapproved_Should_ReturnAllUnapproved_Comments_Count_FromDb()
    {
        //Arrange
        var fakeIQueryable = new List<Comment>()
        {
            new Comment() { IsAproved = false },
            new Comment() { IsAproved = false },
            new Comment() { IsAproved = true },
            new Comment() { IsAproved = false },
            new Comment() { IsAproved = true },
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var bookQueries = new CommentQueries(fakeDbContext);

        var expectedResult = fakeIQueryable.Where(c => !c.IsAproved).Count();

        //Act
        var result = bookQueries.CountUnapproved();

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetComments_Should_ReturnAllUnapproved_Comments_FromDb()
    {
        //Arrange
        var fakeIQueryable = new List<Comment>()
        {
            new Comment() { IsAproved = false },
            new Comment() { IsAproved = false },
            new Comment()
            {
                IsAproved = true,
                Book = new Book()
                {
                    Id = 1,
                }
            },
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var bookQueries = new CommentQueries(fakeDbContext);

        var bookId = 1;
        var expectedResult = fakeIQueryable.Where(c => c.BookId == bookId && c.IsAproved);

        //Act
        var result = bookQueries.GetComments(bookId, 1, 12);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetUnapprovedComments_Should_ReturnAllUnapproved_Comments_FromDb()
    {
        //Arrange
        var fakeIQueryable = new List<Comment>()
        {
            new Comment() { IsAproved = false },
            new Comment() { IsAproved = false },
            new Comment() { IsAproved = true },
            new Comment() { IsAproved = false },
            new Comment() { IsAproved = true },
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var bookQueries = new CommentQueries(fakeDbContext);

        var expectedResult = fakeIQueryable.Where(c => !c.IsAproved);

        //Act
        var result = bookQueries.GetUnapprovedComments(1, 12);

        //Assert
        Assert.Equal(expectedResult, result);
    }
}
