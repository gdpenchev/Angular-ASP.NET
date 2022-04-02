namespace CuriousReadersData.Commands;

using CuriousReadersData.Entities;

public interface IBookCommands
{
    Book CreateBook(Book createBookModelRequest, string imageUrl);

    Book UpdateBook(Book updateBookModelRequest, string bookRequestStatus, string imageUrl);

    void UpdateBookPartially(Book updateBookModelRequest, string? status);
}
