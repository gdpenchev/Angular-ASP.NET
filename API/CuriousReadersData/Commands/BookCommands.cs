namespace CuriousReadersData.Commands;

using CuriousReadersData.Entities;
using CuriousReadersData;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

public class BookCommands : IBookCommands
{
    private readonly LibraryDbContext libraryDbContext;
    public BookCommands(LibraryDbContext libraryDbContext)
    {
        this.libraryDbContext = libraryDbContext;
    }

    public Book CreateBook(Book book, string imageUrl)
    {
        book.Status = this.libraryDbContext.Statuses.FirstOrDefault(s => s.Name == Enumerators.BookStatus.Enabled.ToString());
        book.Description = book.Description is null ? "" : book.Description;

        book.Image = imageUrl;

        this.libraryDbContext.Books.Add(book);
        this.libraryDbContext.SaveChanges();
        return book;
    }

    public Book UpdateBook(Book book, string bookRequestStatus, string imageUrl)
    {
        var updatedBook = this.libraryDbContext.Books.Find(book.Id);

        updatedBook.ISBN = book.ISBN;
        updatedBook.Title = book.Title;
        updatedBook.Quantity = book.Quantity;
        updatedBook.Status = this.libraryDbContext.Statuses.FirstOrDefault(s => s.Name == bookRequestStatus);
        updatedBook.Description = book.Description is null ? "" : book.Description;
        updatedBook.Image = imageUrl;

        this.libraryDbContext.BooksGenres.RemoveRange(this.libraryDbContext.BooksGenres.Where(g => g.BookId == book.Id));
        this.libraryDbContext.AuthorsBooks.RemoveRange(this.libraryDbContext.AuthorsBooks.Where(g => g.BookId == book.Id));

        updatedBook.Genres = book.Genres;
        updatedBook.Authors = book.Authors;

        this.libraryDbContext.Books.Update(updatedBook);
        this.libraryDbContext.SaveChanges();

        return updatedBook;
    }

    public void UpdateBookPartially(Book book, string? status = "")
    {
        var updatedBook = this.libraryDbContext.Books
            .Include(b => b.Status)
            .FirstOrDefault(b => b.Id == book.Id);

        var updateToStatus = updatedBook.Quantity > 0 ? updatedBook.Status.Name : Enumerators.BookStatus.Disabled.ToString();

        if (!string.IsNullOrEmpty(status) && status == Enumerators.BookStatus.Deleted.ToString())
        {
            updateToStatus = status;
        }

        JsonPatchDocument<Book> bookPatchDocument = new JsonPatchDocument<Book>();
        bookPatchDocument.Replace(e => e.Status, this.libraryDbContext.Statuses.FirstOrDefault(s => s.Name == updateToStatus));
        bookPatchDocument.Replace(e => e.Quantity, book.Quantity);
        bookPatchDocument.Replace(e => e.ModifiedOn, DateTime.Now);
        bookPatchDocument.ApplyTo(updatedBook);

        this.libraryDbContext.Books.Update(updatedBook);
        this.libraryDbContext.SaveChanges();
    }
}
