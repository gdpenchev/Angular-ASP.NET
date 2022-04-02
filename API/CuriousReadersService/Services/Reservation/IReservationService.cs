namespace CuriousReadersService.Services.Reservation;

using CuriousReadersData.Dto.Reservations;
using CuriousReadersData.Entities;
using CuriousReadersService.Request;

public interface IReservationService
{
    Task<IEnumerable<ReadReservationModel>> GetAllReservations(int page, int itemsPerPage, string? userEmail, string? reservationStatus);

    IEnumerable<ReadReservationModel> GetPendingReturns(int page, int itemsPerPage);

    Task<Reservation> RequestReservation(ReservationRequest createReservationModelRequest);

    Task<Reservation> UpdateReservation(int reservationId, bool isRejected);

    Task<int> GetReservationsTotalCount(string? userEmail, string? reservationStatus);

    bool CheckIfBorrowed(int bookId, string userId);

    Reservation RequestProlongation(int reservationId);

    Task<Reservation> RejectProlongation(int reservationId);

    int GetPendingReturnsCount();
}
