using CuriousReadersData.Entities;
using Microsoft.EntityFrameworkCore;

namespace CuriousReadersData.Queries;

public class ReservationQueries : IReservationQueries
{
    private readonly LibraryDbContext libraryDbContext;
    public ReservationQueries(LibraryDbContext libraryDbContext)
    {
        this.libraryDbContext = libraryDbContext;
    }

    public bool CheckIfBorrowed(int bookId, string userId)
    {
        return this.libraryDbContext.Reservations
            .Where(r => r.BookId == bookId && r.UserId == userId && r.Status.Name == Enumerators.ReservationStatus.Borrowed.ToString())
            .Any();
    }

    public IQueryable<Reservation> GetAllReservations(int page, int itemsPerPage, string? userId, string? reservationStatus)
    {
        return this.libraryDbContext.Reservations
            .Where(r => string.IsNullOrEmpty(reservationStatus) ? r.Status.Name != null &&
                (string.IsNullOrEmpty(userId) ? r.UserId != userId : r.UserId == userId &&
                !(string.IsNullOrEmpty(r.UserId))) : r.Status.Name == reservationStatus &&
                (string.IsNullOrEmpty(userId) ? r.UserId != userId : r.UserId == userId &&
                !string.IsNullOrEmpty(r.UserId)))
            .Skip(itemsPerPage * (page - 1))
            .Take(itemsPerPage)
                .Include(x => x.Status)
                .Include(x => x.User)
                .Include(x => x.Book)
                .ThenInclude(x => x.Genres)
                    .ThenInclude(x => x.Genre)
                .Include(x => x.Book)
                .ThenInclude(x => x.Authors)
                    .ThenInclude(x => x.Author)
                .Include(x => x.Book)
                    .ThenInclude(x => x.Comments)
                .Include(x => x.Book)
                    .ThenInclude(x => x.Status)
                .Include(x => x.Book)
                    .ThenInclude(x => x.Reservations)
                        .ThenInclude(x => x.User)
                .OrderBy(r => r.RequestDate)
            .OrderBy(r => r.Status.Name == Enumerators.ReservationStatus.PendingReservationApproval.ToString())
            .OrderBy(r => r.Status.Name == Enumerators.ReservationStatus.PendingProlongationApproval.ToString())
            .OrderBy(r => r.Status.Name == Enumerators.ReservationStatus.Reserved.ToString())
            .OrderBy(r => r.Status.Name == Enumerators.ReservationStatus.Borrowed.ToString());
    }

    public IEnumerable<Reservation> BooksNotOnTimeNotification(int skip, int notificationsPerPage)
    {
        return this.libraryDbContext.Reservations
            .Where(r => r.Status.Name == Enumerators.ReservationStatus.Borrowed.ToString() && r.ReturnDate <= DateTime.Now)
            .Skip(skip * notificationsPerPage)
            .Take(notificationsPerPage)
            .Include(r => r.Book)
            .Include(r => r.User)
            .OrderByDescending(r => r.ReturnDate)
            .ToList();
    }

    public IQueryable<Reservation> GetPendingReturns(int page, int itemsPerPage)
    {
        var currentPage = page <= 0 ? 1 : page;
        return this.libraryDbContext.Reservations
            .Where(r => r.Status.Name == Enumerators.ReservationStatus.Borrowed.ToString() && r.ReturnDate <= DateTime.Now.AddDays(30))
            .Skip(itemsPerPage * (currentPage - 1))
            .Take(itemsPerPage)
            .Include(r => r.User)
            .Include(r => r.Book)
              .ThenInclude(x => x.Genres)
               .ThenInclude(x => x.Genre)
             .Include(x => x.Book)
               .ThenInclude(x => x.Authors)
                .ThenInclude(x => x.Author)
              .Include(x => x.Book)
                .ThenInclude(x => x.Comments)
              .Include(x => x.Book)
                .ThenInclude(x => x.Status)
              .Include(x => x.Book)
                .ThenInclude(x => x.Reservations)
                 .ThenInclude(x => x.User)
               .OrderByDescending(r => r.ReturnDate);
    }

    public int GetPendingReturnsCount()
    {
        return this.libraryDbContext.Reservations
            .Where(r => r.Status.Name == Enumerators.ReservationStatus.Borrowed.ToString() && r.ReturnDate <= DateTime.Now.AddDays(30))
            .Count();
    }

    public int GetReservationsTotalCount(string? userId, string? reservationStatus)
    {
        return this.libraryDbContext.Reservations
            .Where(r => string.IsNullOrEmpty(reservationStatus) ? r.Status.Name != null &&
                (string.IsNullOrEmpty(userId) ? r.UserId != userId : r.UserId == userId &&
                !(string.IsNullOrEmpty(r.UserId))) : r.Status.Name == reservationStatus &&
                (string.IsNullOrEmpty(userId) ? r.UserId != userId : r.UserId == userId &&
                !string.IsNullOrEmpty(r.UserId)))
                .Count();
    }

    public bool UserHasReservationTheBook(int bookId, string userId)
    {
        return this.libraryDbContext.Reservations.Any(r => (r.BookId == bookId && r.UserId == userId));
    }
}
