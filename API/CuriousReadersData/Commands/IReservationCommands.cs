namespace CuriousReadersData.Commands;

using CuriousReadersData.Entities;

public interface IReservationCommands
{
    Reservation RequestReservation(User user, Book book);

    Reservation RequestProlongation(int reservationId);

    Reservation RejectProlongation(int reservationId);

    Reservation UpdateReservation(int reservationId, bool isRejected);
}
