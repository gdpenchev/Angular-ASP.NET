namespace CuriousReadersData.Queries;

using CuriousReadersData.Entities;
using Microsoft.EntityFrameworkCore;
public class BookQueries : IBookQueries
{
    private readonly LibraryDbContext libraryDbContext;
    private static string deletedBookStatus = Enumerators.BookStatus.Deleted.ToString();
    private static string disabledBookStatus = Enumerators.BookStatus.Disabled.ToString();
    public BookQueries(LibraryDbContext libraryDbContext)
    {
        this.libraryDbContext = libraryDbContext;
    }

    public IQueryable<Book> GetAllBooks(bool isUserAdmin, int page, int pageSize, string? searchText)
    {
        return this.libraryDbContext.Books
          .Where(b => (isUserAdmin ? b.Status.Name != deletedBookStatus : b.Status.Name != deletedBookStatus && b.Status.Name != disabledBookStatus) &&
                (string.IsNullOrEmpty(searchText) ||
                b.Title.Contains(searchText) ||
                b.Genres.Select(g => g.Genre.Name).Any(e => e.Contains(searchText)) ||
                b.Description.Contains(searchText) ||
                b.Comments.Select(c => c.Content).Any(c => c.Contains(searchText)) ||
                b.Authors.Select(a => a.Author.Name).Any(e => e.Contains(searchText))))
          .OrderByDescending(b => b.CreatedOn)
          .Skip(pageSize * (page - 1))
          .Take(pageSize)
            .Include(x => x.Genres)
                .ThenInclude(x => x.Genre)
            .Include(x => x.Authors)
                .ThenInclude(x => x.Author)
            .Include(x => x.Comments)
            .Include(x => x.Status)
            .Include(x => x.Reservations)
                .ThenInclude(x => x.User);
    }

    public int GetBooksTotalCount(bool isUserAdmin, string? searchText)
    {
        var totalBooksCount = this.libraryDbContext.Books
            .Where(b => (isUserAdmin ? b.Status.Name != deletedBookStatus : b.Status.Name != deletedBookStatus && b.Status.Name != disabledBookStatus) &&
                (string.IsNullOrEmpty(searchText) ||
                b.Title.Contains(searchText) ||
                b.Genres.Select(g => g.Genre.Name).Any(e => e.Contains(searchText)) ||
                b.Description.Contains(searchText) ||
                b.Comments.Select(c => c.Content).Any(c => c.Contains(searchText)) ||
                b.Authors.Select(a => a.Author.Name).Any(e => e.Contains(searchText))))
                    .Count();

        return totalBooksCount;
    }

    public Book GetBookById(int bookId)
    {
        return libraryDbContext.Books
            .Where(x => x.Id == bookId)
                .Include(x => x.Genres)
                    .ThenInclude(x => x.Genre)
                .Include(x => x.Authors)
                    .ThenInclude(x => x.Author)
                .Include(x => x.Comments)
                .Include(x => x.Status)
                .Include(x => x.Reservations)
                    .ThenInclude(x => x.User)
            .FirstOrDefault();
    }

    public Book GetBookByIdExclude(int bookId)
    {
        return libraryDbContext.Books.FirstOrDefault(x => x.Id == bookId);
    }

    public IQueryable<Book> GetReadBookModel(int bookId)
    {
        var bookQuery = this.libraryDbContext.Books
       .Where(b => b.Id == bookId)
       .AsQueryable();

        return bookQuery;
    }

    public (Book? availableBookFromDb, bool restoreAvailable) CheckBookExistanceByISBN(Book book)
    {
        var bookFromDb = libraryDbContext.Books.Include(b => b.Status).FirstOrDefault(b => b.ISBN == book.ISBN);

        return (bookFromDb, bookFromDb is not null && bookFromDb.Status?.Name == Enumerators.BookStatus.Deleted.ToString());
    }

    public bool BookExists(int bookId)
    {
        return libraryDbContext.Books.Any(b => b.Id == bookId);
    }
}
