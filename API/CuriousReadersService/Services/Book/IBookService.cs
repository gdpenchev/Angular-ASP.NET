namespace CuriousReadersService.Services.Book;

using CuriousReadersData.Dto.Books;
using CuriousReadersData.Entities;

public interface IBookService
{
    Task<(Book book, bool? restoreAvailable)> CreateBook(CreateBookModel createBookModelRequest);

    Task<IEnumerable<ReadBookModel>> GetPaginatedBooks(string userEmail, int page, int pageSize, string? searchText);

    Task<Book> UpdateBook(int bookId, UpdateBookModel updateBookModelRequest);

    void UpdateBookPartially(int bookId, UpdateBookModel updateBookModelRequest);

    Task<int> GetBooksTotalCount(string userEmail, string? searchText);

    ReadBookModel GetBookById(int bookId);
}
