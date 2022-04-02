namespace CuriousReadersData.Queries;

using CuriousReadersData.Entities;

public interface IBookQueries
{
    public IQueryable<Book> GetAllBooks(bool isUserAdmin, int page, int pageSize, string? searchText);

    public int GetBooksTotalCount(bool isUserAdmin, string? searchText);

    public Book GetBookById(int bookId);

    public Book GetBookByIdExclude(int bookId);

    (Book? availableBookFromDb, bool restoreAvailable) CheckBookExistanceByISBN(Book book);

    bool BookExists(int bookId);

    IQueryable<Book> GetReadBookModel(int bookId);
}
