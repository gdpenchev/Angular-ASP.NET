namespace CuriousReadersService.Services.Book;

using AutoMapper;
using CuriousReadersData.Commands;
using CuriousReadersData.Dto.Books;
using CuriousReadersData.Entities;
using CuriousReadersData.Queries;
using CuriousReadersService.Services.Image;
using Microsoft.AspNetCore.Identity;

using static CuriousReadersData.DataConstants;

public class BookService : IBookService
{
    private readonly IBookQueries bookQueries;
    private readonly IBookCommands bookCommands;
    private readonly IMapper mapper;
    private readonly IAuthorQueries authorQueries;
    private readonly IGenreQueries genreQueries;
    private readonly UserManager<User> userManager;
    private IImageService imageService;
    public BookService(IBookQueries bookQuery,
        IBookCommands bookCommand,
        IMapper mapper,
        IAuthorQueries authorQuery,
        IGenreQueries genreQueries,
        UserManager<User> userManager,
        IImageService imageService)
    {
        this.bookQueries = bookQuery;
        this.bookCommands = bookCommand;
        this.mapper = mapper;
        this.authorQueries = authorQuery;
        this.genreQueries = genreQueries;
        this.userManager = userManager;
        this.imageService = imageService;
    }

    public async Task<(Book book, bool? restoreAvailable)> CreateBook(CreateBookModel bookRequest)
    {
        var book = mapper.Map<CreateBookModel, Book>(bookRequest);

        var bookExistance = bookQueries.CheckBookExistanceByISBN(book);

        if (bookExistance.availableBookFromDb is not null)
        {
            return (bookExistance.availableBookFromDb, bookExistance.restoreAvailable);
        }

        await using var stream = bookRequest.Image.OpenReadStream();

        var imageFileName = $"{bookRequest.Title} by {string.Join(", ", bookRequest.Authors)} {DateTime.Now.ToString().Replace(':', '-')}";

        var imageUrl = await imageService.UploadFileBlobAsync("book-covers", stream, imageFileName, "");

        book.CreatedOn = DateTime.Now;

        List<BookGenre> bookGenres = new();

        var existingGenres = this.genreQueries.GetExistingGenres(bookRequest.Genres);

        AddBooksToExistingGenre(book, bookGenres, existingGenres);

        var newGenres = this.genreQueries.GetNewGenres(bookRequest.Genres, existingGenres);

        AddBooksToNewGenres(book, bookGenres, newGenres);

        book.Genres = bookGenres;

        List<AuthorBook> authorBooks = new();

        var existingAuthors = this.authorQueries.GetExistingAuthors(bookRequest.Authors);

        AddBooksToExistingAuthor(book, authorBooks, existingAuthors);

        var newAuthors = this.authorQueries.GetNewAuthors(bookRequest.Authors, existingAuthors);

        AddBooksToNewAuthors(book, authorBooks, newAuthors);

        book.Authors = authorBooks;

        return (bookCommands.CreateBook(book, imageUrl), null);
    }


    public async Task<IEnumerable<ReadBookModel>> GetPaginatedBooks(string? userEmail, int page, int pageSize, string? searchText)
    {
        var isUserAdmin = await IsUserAdmin(userEmail);

        var bookQuery = bookQueries.GetAllBooks(isUserAdmin, page, pageSize, searchText);

        var books = mapper.Map<IQueryable<Book>, List<ReadBookModel>>(bookQuery);
        return books.ToList();
    }

    public async Task<int> GetBooksTotalCount(string? userEmail, string? searchText)
    {
        var isUserAdmin = await IsUserAdmin(userEmail);

        return bookQueries.GetBooksTotalCount(isUserAdmin, searchText);
    }

    public ReadBookModel GetBookById(int bookId)
    {
        var bookFromDb = this.bookQueries.GetBookById(bookId);

        if (bookFromDb == null)
        {
            return null;
        }

        return mapper.Map<ReadBookModel>(bookFromDb);
    }


    public async Task<Book> UpdateBook(int bookId, UpdateBookModel bookRequest)
    {
        if (string.IsNullOrEmpty(bookRequest.ImageUrl))
        {
            await using var stream = bookRequest.Image.OpenReadStream();

            var imageFileName = $"{bookRequest.Title} by {string.Join(", ", bookRequest.Authors)} {DateTime.Now.ToString().Replace(':', '-')}";

            bookRequest.ImageUrl = await imageService.UploadFileBlobAsync(
                     "book-covers",
                     stream,
                     imageFileName,
                     bookRequest.OldImageUrl);
        }

        var book = mapper.Map<UpdateBookModel, Book>(bookRequest);

        book.Id = bookId;
        book.ModifiedOn = DateTime.Now;

        List<BookGenre> bookGenres = new();

        var existingGenres = this.genreQueries.GetExistingGenres(bookRequest.Genres);

        AddBooksToExistingGenre(book, bookGenres, existingGenres);

        var newGenres = this.genreQueries.GetNewGenres(bookRequest.Genres, existingGenres);

        AddBooksToNewGenres(book, bookGenres, newGenres);

        book.Genres = bookGenres;

        List<AuthorBook> authorBooks = new();

        var existingAuthors = this.authorQueries.GetExistingAuthors(bookRequest.Authors);

        AddBooksToExistingAuthor(book, authorBooks, existingAuthors);

        var newAuthors = this.authorQueries.GetNewAuthors(bookRequest.Authors, existingAuthors);

        AddBooksToNewAuthors(book, authorBooks, newAuthors);

        book.Authors = authorBooks;

        return bookCommands.UpdateBook(book, bookRequest.Status, bookRequest.ImageUrl);
    }

    public void UpdateBookPartially(int bookId, UpdateBookModel bookRequest)
    {
        var bookExists = bookQueries.BookExists(bookId);

        var book = mapper.Map<UpdateBookModel, Book>(bookRequest);
        book.Id = bookId;
        book.ModifiedOn = DateTime.Now;

        bookCommands.UpdateBookPartially(book, bookRequest.Status);
    }

    private void AddBooksToExistingAuthor(Book book, List<AuthorBook> authorBooks, List<Author> existingAuthors)
    {
        existingAuthors
            .ForEach(author => authorBooks.Add(new AuthorBook()
            {
                Author = author,
                Book = book
            }));
    }

    private void AddBooksToNewAuthors(Book book, List<AuthorBook> authorBooks, List<string> newAuthors)
    {
        newAuthors
            .ForEach(x => authorBooks.Add(new AuthorBook()
            {
                Author = new Author() { Name = x },
                Book = book
            }));
    }

    private void AddBooksToExistingGenre(Book book, List<BookGenre> bookGenres, List<Genre> existingGenres)
    {
        existingGenres
            .ForEach(genre => bookGenres.Add(new BookGenre()
            {
                Book = book,
                Genre = genre,
            }));
    }

    private void AddBooksToNewGenres(Book book, List<BookGenre> bookGenres, List<string> newGenres)
    {
        newGenres
            .ForEach(genreName => bookGenres.Add(new BookGenre()
            {
                Book = book,
                Genre = new Genre() { Name = genreName },
            }));
    }

    private async Task<bool> IsUserAdmin(string? userEmail)
    {
        if (userEmail == null) { return false; }

        var user = await userManager.FindByEmailAsync(userEmail);

        var isUserAdmin = await userManager.IsInRoleAsync(user, Librarian);

        return isUserAdmin;
    }
}
