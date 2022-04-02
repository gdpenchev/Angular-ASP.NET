namespace CuriousReaders.Test.Controllers;

using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Xunit;

using CuriousReadersAPI.Controllers;
using CuriousReadersData.Dto.Books;
using CuriousReadersData.Entities;
using FakeItEasy;
using CuriousReadersService.Services.Book;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class BooksControllerTest
{
    private IBookService bookServiceMock = A.Fake<IBookService>();

    [Fact]
    public void CreateBook_Returns_CreatedResultIfRestoreNotAvailable()
    {
        //Arrange
        A.CallTo(() => this.bookServiceMock.CreateBook(A<CreateBookModel>.Ignored))
            .Returns((new Book(), false));

        var booksController = new BooksController(bookServiceMock);

        var createBookModel = new CreateBookModel
        {
            Title = "Book",
            ISBN = "978-9-3897-4501-3",
            Genres = A.Fake<List<string>>(),
            Image = A.Fake<IFormFile>(),
            Authors = A.Fake<List<string>>(),
        };

        //Act
        var result = booksController.CreateBook(createBookModel);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<Task<ActionResult<CreateBookModel>>>(result);
    }

    [Fact]
    public void CreateBook_Returns_OkResultIfRestoreAvailable()
    {
        //Arrange
        A.CallTo(() => this.bookServiceMock.CreateBook(A<CreateBookModel>.Ignored))
            .Returns((new Book(), true));

        var booksController = new BooksController(bookServiceMock);

        var createBookModel = new CreateBookModel
        {
            Title = "Book",
            ISBN = "978-9-3897-4501-3",
            Genres = A.Fake<List<string>>(),
            Image = A.Fake<IFormFile>(),
            Authors = A.Fake<List<string>>(),
        };

        //Act
        var result = booksController.CreateBook(createBookModel);

        //Assert
        //Assert
        Assert.NotNull(result);
        Assert.IsType<Task<ActionResult<CreateBookModel>>>(result);
    }


    [Fact]
    public void CreateBook_Returns_BadRequest_IfModel_IsNull()
    {
        //Arrange
        var serviceResponse = new Book();
        A.CallTo(() => this.bookServiceMock.CreateBook(A<CreateBookModel>.Ignored))
            .Returns((serviceResponse = null, false));

        var booksController = new BooksController(bookServiceMock);

        CreateBookModel createBookModel = new CreateBookModel
        {
            Image = A.Fake<IFormFile>(),
        };

        //Act
        var result = booksController.CreateBook(createBookModel);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<Task<ActionResult<CreateBookModel>>>(result);
    }

    [Fact]
    public void GetAllBooks_Returns_OkObjectResult()
    {
        //Arrange
        string userEmail = "email@gmail.com";
        int page = 1;
        int limit = 20;
        string search = "";

        A.CallTo(() => this.bookServiceMock.GetPaginatedBooks(userEmail, page, limit, search))
              .Returns(new List<ReadBookModel>());

        var booksController = new BooksController(bookServiceMock);

        ////Act
        var result = booksController.GetAllBooks(userEmail, page, limit, search);

        ////Assert
        Assert.NotNull(result);
        Assert.IsType<Task<ActionResult<IEnumerable<ReadBookModel>>>>(result);
    }

    [Fact]
    public void GetBookById_Returns_OkObjectResult_If_BookIdExists()
    {
        //Arrange
        var bookId = 3;

        A.CallTo(() => this.bookServiceMock.GetBookById(bookId))
            .Returns(new ReadBookModel() { Id = bookId });

        var booksController = new BooksController(bookServiceMock);

        //Act
        var result = booksController.GetBookById(bookId);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<ActionResult<ReadBookModel>>(result);
    }


    [Fact]
    public void GetBookById_Returns_NotFound_When_IdDoesNotExist()
    {
        //Arrange
        var bookId = 3;

        A.CallTo(() => this.bookServiceMock.GetBookById(bookId))
            .Returns(null);

        var booksController = new BooksController(bookServiceMock);

        //Act
        var result = booksController.GetBookById(bookId);

        //Assert
        Assert.IsType<NotFoundResult>(result.Result);
        Assert.IsType<ActionResult<ReadBookModel>>(result);
    }


    [Fact]
    public void GetBooksTotalCount_Returns_TotalBooksCount()
    {
        //Arrange
        string userEmail = "email@gmail.com";
        string searchText = "";

        A.CallTo(() => this.bookServiceMock.GetBooksTotalCount(userEmail, searchText))
              .Returns(new int());

        var booksController = new BooksController(bookServiceMock);

        ////Act
        var result = booksController.GetBooksTotalCount(userEmail, searchText);

        ////Assert
        Assert.NotNull(result);
        Assert.IsType<Task<ActionResult<int>>>(result);
    }


    [Fact]
    public void UpdateBook_Returns_OkResult()
    {
        //Arrange
        var bookId = 3;

        A.CallTo(() => this.bookServiceMock.UpdateBook(bookId, A<UpdateBookModel>.Ignored))
            .Returns(new Book());

        var booksController = new BooksController(bookServiceMock);

        var updateBookRequestModel = new UpdateBookModel
        {
            Title = "Book",
            ISBN = "978-9-3897-4501-3",
            Genres = A.Fake<List<string>>(),
            Image = A.Fake<IFormFile>(),
            Authors = A.Fake<List<string>>(),
        };

        //Act
        var result = booksController.UpdateBook(bookId, updateBookRequestModel);


        //Assert
        Assert.IsType<ActionResult<ReadBookModel>>(result.Result);

        Assert.NotNull(result);
        Assert.IsType<Task<ActionResult<ReadBookModel>>>(result);
    }

    [Fact]
    public void UpdateBookPartially_Returns_OkObjectResult()
    {
        //Arrange
        var bookId = 3;

        A.CallTo(() => this.bookServiceMock.UpdateBookPartially(bookId, A<UpdateBookModel>.Ignored))
            .DoesNothing();

        var booksController = new BooksController(bookServiceMock);

        var updateBookModelMock = A.Fake<UpdateBookModel>();

        //Act
        var result = booksController.UpdateBookPartially(bookId, updateBookModelMock);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<OkResult>(result);
    }
}
