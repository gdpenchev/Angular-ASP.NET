namespace CuriousReaders.Test.Data.Queries;

using CuriousReadersData;
using CuriousReadersData.Entities;
using CuriousReadersData.Queries;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class BookQueriesTest
{
    private LibraryDbContext fakeDbContext = A.Fake<LibraryDbContext>();

    private void SetupFakeDbSet(IQueryable<Book> fakeIQueryable)
    {
        var fakeDbSet = A.Fake<DbSet<Book>>((d =>
                 d.Implements(typeof(IQueryable<Book>))));

        A.CallTo(() => ((IQueryable<Book>)fakeDbSet).GetEnumerator())
            .Returns(fakeIQueryable.GetEnumerator());

        A.CallTo(() => ((IQueryable<Book>)fakeDbSet).Provider)
            .Returns(fakeIQueryable.Provider);

        A.CallTo(() => ((IQueryable<Book>)fakeDbSet).Expression)
            .Returns(fakeIQueryable.Expression);

        A.CallTo(() => ((IQueryable<Book>)fakeDbSet).ElementType)
           .Returns(fakeIQueryable.ElementType);

        A.CallTo(() => fakeDbContext.Books).Returns(fakeDbSet);
    }


    [Fact]
    public void GetAllBooks_Should_ReturnAllBooks_ThatAreNot_Deleted_WhenUserIsAdmin()
    {
        //Arrange
        var fakeIQueryable = new List<Book>()
        {
            new Book() { Id = 1, Status = new Status{ Name = Enumerators.BookStatus.Enabled.ToString() } },
            new Book() { Id = 2, Status = new Status{ Name = Enumerators.BookStatus.Enabled.ToString() } },
            new Book() { Id = 3, Status = new Status{ Name = Enumerators.BookStatus.Enabled.ToString() } },
            new Book() { Id = 4 , Status = new Status{ Name = Enumerators.BookStatus.Disabled.ToString() }},
            new Book() { Id = 5 , Status = new Status{ Name = Enumerators.BookStatus.Deleted.ToString() }},
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var bookQueries = new BookQueries(fakeDbContext);

        var expectedCount = fakeIQueryable.Where(x => x.Status.Name != Enumerators.BookStatus.Deleted.ToString()).Count();

        //Act
        var result = bookQueries.GetAllBooks(true, 1, 12, "");

        var actualCount = result.Count();

        //Assert
        Assert.Equal(expectedCount, actualCount);
    }



    [Fact]
    public void GetAllBooks_Should_ReturnAllBooks_ThatAreNot_DisabledDeleted_WhenUserIsNotAdmin()
    {
        //Arrange
        var fakeIQueryable = new List<Book>()
        {
            new Book() { Id = 1, Status = new Status{ Name = Enumerators.BookStatus.Enabled.ToString() } },
            new Book() { Id = 2, Status = new Status{ Name = Enumerators.BookStatus.Enabled.ToString() } },
            new Book() { Id = 3, Status = new Status{ Name = Enumerators.BookStatus.Enabled.ToString() } },
            new Book() { Id = 4 , Status = new Status{ Name = Enumerators.BookStatus.Disabled.ToString() }},
            new Book() { Id = 5 , Status = new Status{ Name = Enumerators.BookStatus.Deleted.ToString() }},
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var bookQueries = new BookQueries(fakeDbContext);

        var expectedCount = fakeIQueryable.Where(x => x.Status.Name != Enumerators.BookStatus.Disabled.ToString() && x.Status.Name != Enumerators.BookStatus.Deleted.ToString()).Count();

        //Act
        var result = bookQueries.GetAllBooks(false, 1, 12, "");

        var actualCount = result.Count();

        //Assert
        Assert.Equal(expectedCount, actualCount);
    }


    [Fact]
    public void GetBooksTotalCount_ShouldReturn_AllBooksCount()
    {
        //Arrange
        var fakeIQueryable = new List<Book>()
        {
            new Book() { Id = 1, Status = new Status{ Name = Enumerators.BookStatus.Enabled.ToString() } },
            new Book() { Id = 2, Status = new Status{ Name = Enumerators.BookStatus.Enabled.ToString() } },
            new Book() { Id = 3, Status = new Status{ Name = Enumerators.BookStatus.Enabled.ToString() } },
            new Book() { Id = 4 , Status = new Status{ Name = Enumerators.BookStatus.Enabled.ToString() }},
            new Book() { Id = 5 , Status = new Status{ Name = Enumerators.BookStatus.Enabled.ToString() }},
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var bookQueries = new BookQueries(fakeDbContext);

        //Act
        var result = bookQueries.GetBooksTotalCount(true, "");

        //Assert
        Assert.Equal(result, fakeIQueryable.Count());
    }

    [Fact]
    public void GetBookById_ShouldReturn_BookWithThatId()
    {
        //Arrange
        var book = new Book() { Id = 1 };

        var fakeIQueryable = new List<Book>() { book }.AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var bookQueries = new BookQueries(fakeDbContext);

        //Act
        var result = bookQueries.GetBookById(1);


        //Assert
        Assert.Equal(result.Id, book.Id);
    }

    [Fact]
    public void GetBookByIdExclude_ShoudReturn_BookToExclude()
    {
        //Arrange
        var book = new Book() { Id = 1 };

        var fakeIQueryable = new List<Book>() { book }.AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var bookQueries = new BookQueries(fakeDbContext);

        //Act
        var result = bookQueries.GetBookByIdExclude(1);


        //Assert
        Assert.Equal(result.Id, book.Id);
    }


    [Fact]
    public void GetReadBookModel_ShouldReturn_IqueriableOfBook()
    {
        //Arrange
        var book = new Book() { Id = 1 };

        var fakeIQueryable = new List<Book>() { book }.AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var bookQueries = new BookQueries(fakeDbContext);


        //Act
        var result = bookQueries.GetReadBookModel(1);


        //Assert
        Assert.Equal(result, fakeIQueryable);
    }


    [Fact]
    public void CheckBookExistanceByISBN_ReinstateAvailabe_IsFalse_WhenBookWithISBN_IsNotPresent()
    {
        //Arrange
        var expected = false;

        var book = new Book() { Id = 1, ISBN = "978-9-3897-4501-3" };

        var otherBook = new Book() { Id = 1, ISBN = "978-9-3897-4501-1" };

        var fakeIQueryable = new List<Book>() { book }.AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var bookQueries = new BookQueries(fakeDbContext);


        //Act
        var result = bookQueries.CheckBookExistanceByISBN(otherBook);

        //Assert
        Assert.Equal(expected, result.restoreAvailable);
    }


    [Fact]
    public void CheckBookExistanceByISBN_ReturnsFalse_WhenBookWithISBN_IsPresent()
    {
        //Arrange
        var expected = false;

        var book = new Book() { Id = 1, ISBN = "978-9-3897-4501-3" };

        var fakeIQueryable = new List<Book>() { book }.AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var bookQueries = new BookQueries(fakeDbContext);


        //Act
        var result = bookQueries.CheckBookExistanceByISBN(book);

        //Assert
        Assert.Equal(expected, result.restoreAvailable);
    }


    [Fact]
    public void BookExists_ShouldReturnTrue_IfBookWithIdExists()
    {
        //Arrange
        var expected = true;
        
        var book = new Book() { Id = 1 };

        var fakeIQueryable = new List<Book>() { book }.AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var bookQueries = new BookQueries(fakeDbContext);


        //Act
        var result = bookQueries.BookExists(1);

        //Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void BookExists_ShouldReturnFalse_IfBookWithIdExistsNot()
    {
        //Arrange
        var expected = false;

        var book = new Book() { Id = 1 };

        var fakeIQueryable = new List<Book>() { book }.AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var bookQueries = new BookQueries(fakeDbContext);


        //Act
        var result = bookQueries.BookExists(5);

        //Assert
        Assert.Equal(expected, result);
    }
}
