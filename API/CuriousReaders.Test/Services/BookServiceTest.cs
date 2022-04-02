namespace CuriousReaders.Test.Services;

using AutoMapper;
using CuriousReadersData.Commands;
using CuriousReadersData.Dto.Books;
using CuriousReadersData.Entities;
using CuriousReadersData.Profiles;
using CuriousReadersData.Queries;
using CuriousReadersService.Services.Book;
using CuriousReadersService.Services.Image;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Linq;
using Xunit;

public class BookServiceTest
{
    private readonly IMapper mapper;
    private readonly IBookQueries bookQueryMock = A.Fake<IBookQueries>();
    private readonly IBookCommands bookCommandMock = A.Fake<IBookCommands>();
    private readonly IAuthorQueries authorQueryMock = A.Fake<IAuthorQueries>();
    private readonly CreateBookModel createBookModelMock = A.Fake<CreateBookModel>();
    private readonly UpdateBookModel updateBookModelMock = A.Fake<UpdateBookModel>();
    private readonly IGenreQueries genreQueryMock = A.Fake<IGenreQueries>();
    private readonly ImageService imageServiceMock = A.Fake<ImageService>();
    private readonly IQueryable<ReadBookModel> readBookModelListMock = A.Fake<IQueryable<ReadBookModel>>();
    private readonly IQueryable<Book> booksMockQueryable = A.Fake<IQueryable<Book>>();
    private readonly Book bookMock = A.Fake<Book>();
    private readonly int booksTotalCount = 0;
    private BookService bookService;
    private UserManager<User> userManager = A.Fake<UserManager<User>>();
    private string imageUrl = "test";


    public BookServiceTest()
    {

        var config = new MapperConfiguration(config =>
        {
            config.AddProfile(new BooksProfile());
            config.AddProfile(new AuthorsProfile());
        });

        this.mapper = new Mapper(config);
    }

    private void SetupService()
    {
        bookService = new BookService(bookQueryMock, bookCommandMock, mapper, authorQueryMock, genreQueryMock, userManager, imageServiceMock);
    }

    [Fact]
    public void CreateBook_Creates_NewBook()
    {
        //Arrange
        A.CallTo(() => bookCommandMock.CreateBook(A<Book>.Ignored, A<string>.Ignored))
            .Returns((new Book()));

        A.CallTo(() => bookQueryMock.CheckBookExistanceByISBN(A<Book>.Ignored))
            .Returns((null, true));

        SetupService();

        //Act
        var bookRequest = new CreateBookModel()
        {
            Image = A.Fake<IFormFile>()
        };

        var result = bookService.CreateBook(bookRequest);

        //Assert
        A.CallTo(() => bookCommandMock.CreateBook(A<Book>.Ignored, A<string>.Ignored))
            .MustHaveHappenedOnceExactly();

        Assert.NotNull(result);
        Assert.Equal(JsonConvert.SerializeObject(new Book()), JsonConvert.SerializeObject(result.Result.Item1));
    }


    [Fact]
    public void CreateBook_ShouldNot_CreateBook_With_ExistingISBN()
    {
        //Arrange
        A.CallTo(() => bookCommandMock.CreateBook(bookMock, imageUrl))
            .Returns(new Book());

        A.CallTo(() => bookQueryMock.CheckBookExistanceByISBN(bookMock))
            .Returns((new Book(), false));

        SetupService();

        //Act
        bookService.CreateBook(createBookModelMock);

        //Assert
        A.CallTo(() => bookCommandMock.CreateBook(bookMock, imageUrl))
            .MustNotHaveHappened();
    }

    [Fact]
    public void GetBookById_Returns_Book_If_IdExists()
    {
        //Arrange
        var bookId = 5;

        SetupService();

        A.CallTo(() => bookQueryMock.GetBookById(A<int>.Ignored))
            .Returns(new Book());

        //Act
        var result = bookService.GetBookById(bookId);


        //Assert
        A.CallTo(() => bookQueryMock.GetBookById(bookId))
            .MustHaveHappenedOnceExactly();
    }


    [Fact]
    public void GetBookById_Returns_Null_If_IdDoesNotExist()
    {
        //Arrange
        var bookId = 5;

        SetupService();

        A.CallTo(() => bookQueryMock.GetBookById(A<int>.Ignored))
            .Returns(null);

        //Act
        bookService.GetBookById(bookId);


        //Assert
        A.CallTo(() => bookQueryMock.GetReadBookModel(bookId))
            .MustNotHaveHappened();
    }

    [Fact]
    public async void GetAllBooks_Returns_IQueryable_Of_ReadModelBook()
    {
        //Arrange
        string userEmail = "email@gmail.com";
        bool isUserAdmin = true;
        int page = 1;
        int pageSize = 20;
        string searchText = "";

        SetupService();

        A.CallTo(() => bookQueryMock.GetAllBooks(isUserAdmin, page, pageSize, searchText))
            .Returns(booksMockQueryable);

        //Act
        var result = await bookService.GetPaginatedBooks(userEmail, page, pageSize, searchText);


        //Assert
        Assert.NotNull(result);
        Assert.Equal(readBookModelListMock, result);

    }

    [Fact]
    public async void GetAllBooks_Returns_Empty_IQueryable_If_Books_Are_Not_Exisitng()
    {
        //Arrange
        string userEmail = "email@gmail.com";
        bool isUserAdmin = true;
        int page = 1;
        int pageSize = 20;
        string searchText = "";

        SetupService();

        A.CallTo(() => bookQueryMock.GetAllBooks(isUserAdmin, page, pageSize, searchText))
            .Returns(Enumerable.Empty<Book>().AsQueryable());

        //Act
        var result = await bookService.GetPaginatedBooks(userEmail, page, pageSize, searchText);

        //Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async void GetBooksTotalCount_Returns_Books_Total_Count()
    {
        //Arrange
        string userEmail = "email@gmail.com";
        bool isUserAdmin = true;
        string searchText = "";
        SetupService();

        A.CallTo(() => bookQueryMock.GetBooksTotalCount(isUserAdmin, searchText))
            .Returns(booksTotalCount);

        //Act
        var result = await bookService.GetBooksTotalCount(userEmail, searchText);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(booksTotalCount, result);
    }

    [Fact]
    public async void UpdateBook_Updates_ExistingBook()
    {
        //Arrange
        var bookId = 1;

        A.CallTo(() => bookCommandMock.UpdateBook(A<Book>.Ignored, A<string>.Ignored, A<string>.Ignored))
            .Returns(new Book());

        SetupService();

        var bookRequest = new UpdateBookModel()
        {
            Image = A.Fake<IFormFile>()
        };

        //Act
        var result = bookService.UpdateBook(bookId, bookRequest);

        //Assert
        A.CallTo(() => bookCommandMock.UpdateBook(A<Book>.Ignored, A<string>.Ignored, A<string>.Ignored))
            .MustHaveHappenedOnceExactly();

        Assert.NotNull(result);
        Assert.Equal(JsonConvert.SerializeObject(new Book()), JsonConvert.SerializeObject(result.Result));
    }

    [Fact]
    public void UpdateBookPartially_Returns_The_Book_If_Updated_Successfully()
    {
        //Arrange
        var bookId = 1;
        SetupService();

        A.CallTo(() => bookCommandMock.UpdateBookPartially(A<Book>.Ignored, A<string>.Ignored))
            .DoesNothing();

        A.CallTo(() => bookQueryMock.BookExists(bookId))
            .Returns(true);

        //Act
        bookService.UpdateBookPartially(bookId, updateBookModelMock);

        //Assert
        A.CallTo(() => bookCommandMock.UpdateBookPartially(A<Book>.Ignored, A<string>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
}
