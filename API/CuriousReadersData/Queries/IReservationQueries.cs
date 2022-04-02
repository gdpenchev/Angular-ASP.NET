using CuriousReadersData.Entities;

namespace CuriousReadersData.Queries;

public interface IReservationQueries
{
    IQueryable<Reservation> GetAllReservations(int page, int itemsPerPage, string? userId, string? reservationStatus);
    
    bool UserHasReservationTheBook(int bookId, string userId);

    int GetReservationsTotalCount(string? userId, string? reservationStatus);

    IQueryable<Reservation> GetPendingReturns(int page, int itemsPerPage);

    bool CheckIfBorrowed(int bookId, string userId);

    int GetPendingReturnsCount();

    IEnumerable<Reservation> BooksNotOnTimeNotification(int skip, int notificationsPerPage);
}
