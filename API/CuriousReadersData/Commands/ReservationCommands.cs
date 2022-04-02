namespace CuriousReadersData.Commands;

using CuriousReadersData.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

public class ReservationCommands : IReservationCommands
{
    private readonly LibraryDbContext libraryDbContext;
    public ReservationCommands(LibraryDbContext libraryDbContext)
    {
        this.libraryDbContext = libraryDbContext;
    }

    public Reservation RejectProlongation(int reservationId)
    {
        var reservation = this.libraryDbContext.Reservations
            .Include(r => r.Book)
              .ThenInclude(b => b.Status)
            .Include(r => r.Status)
            .Include(r=>r.User)
              .FirstOrDefault(r => r.Id == reservationId);

        string newStatus = Enumerators.ReservationStatus.Borrowed.ToString();

        JsonPatchDocument<Reservation> reservationPatchDocument = new JsonPatchDocument<Reservation>();
        reservationPatchDocument.Replace(r => r.Status, this.libraryDbContext.Statuses.FirstOrDefault(s => s.Name == newStatus));
        reservationPatchDocument.ApplyTo(reservation);

        this.libraryDbContext.Reservations.Update(reservation);
        this.libraryDbContext.SaveChanges();

        return reservation;
    }

    public Reservation RequestProlongation(int reservationId)
    {
        var reservation = this.libraryDbContext.Reservations
            .Include(r => r.Book)
              .ThenInclude(b => b.Status)
            .Include(r => r.Status)
              .FirstOrDefault(r => r.Id == reservationId);

        JsonPatchDocument<Reservation> reservationPatchDocument = new JsonPatchDocument<Reservation>();
        string newStatus = Enumerators.ReservationStatus.PendingProlongationApproval.ToString();

        reservationPatchDocument.Replace(e => e.Status, this.libraryDbContext.Statuses.FirstOrDefault(s => s.Name == newStatus));

        reservationPatchDocument.ApplyTo(reservation);

        this.libraryDbContext.Reservations.Update(reservation);
        this.libraryDbContext.SaveChanges();

        return reservation;
    }

    public Reservation RequestReservation(User user, Book book)
    {
        var reservation = new Reservation()
        {
            Status = this.libraryDbContext.Statuses.FirstOrDefault(s => s.Name == Enumerators.ReservationStatus.PendingReservationApproval.ToString()),
            User = user,
            Book = book,
            RequestDate = DateTime.Now
        };

        this.libraryDbContext.Reservations.Add(reservation);
        this.libraryDbContext.SaveChanges();

        return reservation;
    }

    public Reservation UpdateReservation(int reservationId, bool isRejected)
    {
        var reservation = this.libraryDbContext.Reservations
            .Include(r => r.Book)
              .ThenInclude(b => b.Status)
            .Include(r => r.Status)
              .FirstOrDefault(r => r.Id == reservationId);

        if (reservation is null || (reservation.Status.Name == Enumerators.ReservationStatus.PendingReservationApproval.ToString() && reservation.Book.Status.Name != Enumerators.BookStatus.Enabled.ToString()))
        {
            return null;
        }

        JsonPatchDocument<Reservation> reservationPatchDocument = new JsonPatchDocument<Reservation>();

        string newStatus = reservation.Status.Name;

        if (reservation.Status.Name == Enumerators.ReservationStatus.PendingProlongationApproval.ToString())
        {
            reservationPatchDocument.Replace(e => e.ReturnDate, reservation.ReturnDate.Value.AddDays(30));
        }

        if (reservation.Status.Name == Enumerators.ReservationStatus.Reserved.ToString() && !isRejected)
        {
            reservationPatchDocument.Replace(e => e.ReturnDate, DateTime.Now.AddDays(30));
        }

        newStatus = GetNewStatus(isRejected, reservation, newStatus);

        reservationPatchDocument.Replace(e => e.Status, this.libraryDbContext.Statuses.FirstOrDefault(s => s.Name == newStatus));

        reservationPatchDocument.ApplyTo(reservation);

        this.libraryDbContext.Reservations.Update(reservation);
        this.libraryDbContext.SaveChanges();

        return reservation;
    }

    private string GetNewStatus(bool isRejected, Reservation? reservation, string newStatus)
    {
        return (isRejected, reservation.Status.Name, newStatus) switch
        {
            (_, "Borrowed", _) => newStatus = Enumerators.ReservationStatus.Returned.ToString(),
            (_, "PendingProlongationApproval", _) => newStatus = Enumerators.ReservationStatus.Borrowed.ToString(),
            (false, "Reserved", _) => newStatus = Enumerators.ReservationStatus.Borrowed.ToString(),
            (false, "PendingReservationApproval", _) => newStatus = Enumerators.ReservationStatus.Reserved.ToString(),
            (true, _, _) => Enumerators.ReservationStatus.Rejected.ToString(),
        };
    }
}
