namespace CuriousReaders.Test.Data.Commands;

using CuriousReadersData;
using CuriousReadersData.Commands;
using CuriousReadersData.Entities;
using CuriousReadersData.Queries;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class BookCommandsTest
{
    private LibraryDbContext fakeDbContext = A.Fake<LibraryDbContext>();
    private Book fakeBook = A.Fake<Book>();

    private void SetupFakeBooksDbSet(IQueryable<Book> fakeIQueryable)
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

    private void SetupFakeStatusDbSet(IQueryable<Status> fakeIQueryable)
    {
        var fakeDbSet = A.Fake<DbSet<Status>>((d =>
                 d.Implements(typeof(IQueryable<Status>))));

        A.CallTo(() => ((IQueryable<Status>)fakeDbSet).GetEnumerator())
            .Returns(fakeIQueryable.GetEnumerator());

        A.CallTo(() => ((IQueryable<Status>)fakeDbSet).Provider)
            .Returns(fakeIQueryable.Provider);

        A.CallTo(() => ((IQueryable<Status>)fakeDbSet).Expression)
            .Returns(fakeIQueryable.Expression);

        A.CallTo(() => ((IQueryable<Status>)fakeDbSet).ElementType)
           .Returns(fakeIQueryable.ElementType);

        A.CallTo(() => fakeDbContext.Statuses).Returns(fakeDbSet);
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

    private void SetupFakeAuthorBookDbSet(IQueryable<AuthorBook> fakeIQueryable)
    {
        var fakeDbSet = A.Fake<DbSet<AuthorBook>>((d =>
                 d.Implements(typeof(IQueryable<AuthorBook>))));

        A.CallTo(() => ((IQueryable<AuthorBook>)fakeDbSet).GetEnumerator())
            .Returns(fakeIQueryable.GetEnumerator());

        A.CallTo(() => ((IQueryable<AuthorBook>)fakeDbSet).Provider)
            .Returns(fakeIQueryable.Provider);

        A.CallTo(() => ((IQueryable<AuthorBook>)fakeDbSet).Expression)
            .Returns(fakeIQueryable.Expression);

        A.CallTo(() => ((IQueryable<AuthorBook>)fakeDbSet).ElementType)
           .Returns(fakeIQueryable.ElementType);

        A.CallTo(() => fakeDbContext.AuthorsBooks).Returns(fakeDbSet);
    }

    [Fact]
    public void CreateBook_ShouldCreate_NewBook()
    {
        //Arrange
        var fakeIQueryableBooks = new List<Book>()
        {
            new Book() { Id = 1, Status = new Status{ Name = Enumerators.BookStatus.Enabled.ToString() } },
            new Book() { Id = 2, Status = new Status{ Name = Enumerators.BookStatus.Enabled.ToString() } },
            new Book() { Id = 3, Status = new Status{ Name = Enumerators.BookStatus.Enabled.ToString() } },
            new Book() { Id = 4 , Status = new Status{ Name = Enumerators.BookStatus.Disabled.ToString() }},
            new Book() { Id = 5 , Status = new Status{ Name = Enumerators.BookStatus.Deleted.ToString() }},
        }
        .AsQueryable();

        var fakeIqueryStatus = new List<Status>()
        {
            new Status{ Name = Enumerators.BookStatus.Enabled.ToString() },
            new Status{ Name = Enumerators.BookStatus.Disabled.ToString() },
            new Status{ Name = Enumerators.BookStatus.Deleted.ToString() }

        }.AsQueryable();

        SetupFakeBooksDbSet(fakeIQueryableBooks);

        SetupFakeStatusDbSet(fakeIqueryStatus);
        
        var bookCommand = new BookCommands(fakeDbContext);

        var image = "someImage";

        //Act
        var result = bookCommand.CreateBook(fakeBook, image);

        //Assert
        A.CallTo(() => fakeDbContext.Books.Add(fakeBook))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeDbContext.SaveChanges())
           .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void UpdateBook_ShouldUpdate_CurrentBook()
    {
        //Arrange
        var fakeIQueryableBooks = new List<Book>()
        {
            new Book() { Id = 1, Status = new Status{ Name = Enumerators.BookStatus.Enabled.ToString() } },
        }
        .AsQueryable();

        var fakeIqueryStatus = new List<Status>()
        {
            new Status{ Name = Enumerators.BookStatus.Enabled.ToString() },

        }.AsQueryable();

        var fakeIqueryGenres = new List<BookGenre>()
        {
            new BookGenre{ BookId = 1, GenreId = 1 },

        }.AsQueryable();

        var fakeIqueryAuthor = new List<AuthorBook>()
        {
            new AuthorBook{ BookId = 1, AuthorId = 1},

        }.AsQueryable();

        SetupFakeBooksDbSet(fakeIQueryableBooks);

        SetupFakeStatusDbSet(fakeIqueryStatus);

        SetupFakeBookGenreDbSet(fakeIqueryGenres);

        SetupFakeAuthorBookDbSet(fakeIqueryAuthor);

        var bookCommand = new BookCommands(fakeDbContext);

        var image = "someImage";
        var requestStatus = "Enable";
        var book = fakeIQueryableBooks.Where(b => b.Id == 1).FirstOrDefault();

        //Act
        var result = bookCommand.UpdateBook(fakeBook, requestStatus, image);

        //Assert
        A.CallTo(() => fakeDbContext.Books.Update(result))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeDbContext.SaveChanges())
           .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void UpdateBookPartialy_ShouldUpdate_CurrentBook()
    {
        //Arrange
        var fakeIQueryable = new List<Book>()
        {
            new Book() { Id = 1,Quantity = 2, Status = new Status{ Name = Enumerators.BookStatus.Enabled.ToString() } },
        }
        .AsQueryable();

        var fakeIqueryStatus = new List<Status>()
        {
            new Status{ Name = Enumerators.BookStatus.Enabled.ToString() },

        }.AsQueryable();

        SetupFakeBooksDbSet(fakeIQueryable);

        SetupFakeStatusDbSet(fakeIqueryStatus);

        var bookCommand = new BookCommands(fakeDbContext);

        var book = fakeIQueryable.Where(b => b.Id == 1).FirstOrDefault();

        var requestStatus = "Enable";

        //Act
        bookCommand.UpdateBookPartially(book, requestStatus);

        //Assert
        A.CallTo(() => fakeDbContext.Books.Update(book))
           .MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeDbContext.SaveChanges())
           .MustHaveHappenedOnceExactly();
    }
}
