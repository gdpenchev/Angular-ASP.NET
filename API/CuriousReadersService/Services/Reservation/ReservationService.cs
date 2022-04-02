namespace CuriousReadersService.Services.Reservation;

using AutoMapper;
using CuriousReadersData;
using CuriousReadersData.Commands;
using CuriousReadersData.Dto.Books;
using CuriousReadersData.Dto.Reservations;
using CuriousReadersData.Entities;
using CuriousReadersData.Queries;
using CuriousReadersService.Request;
using CuriousReadersService.Services.Mail;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using static CuriousReadersData.DataConstants;

public class ReservationService : IReservationService
{
    private readonly IReservationQueries reservationQueries;
    private readonly IReservationCommands reservationCommands;
    private readonly IBookQueries bookQueries;
    private readonly IBookCommands bookCommands;
    private readonly IMapper mapper;
    private readonly IMailService mailService;
    private readonly UserManager<User> userManager;

    public ReservationService(
        IReservationQueries reservationQueries,
        IReservationCommands reservationCommands,
        IBookQueries bookQueries,
        IBookCommands bookCommands,
        IMapper mapper,
        UserManager<User> userManager,
        IMailService mailService)
    {

        this.reservationQueries = reservationQueries;
        this.reservationCommands = reservationCommands;
        this.bookQueries = bookQueries;
        this.bookCommands = bookCommands;
        this.mapper = mapper;
        this.userManager = userManager;
        this.mailService = mailService;
    }

    public async Task<Reservation> RequestReservation(ReservationRequest createReservationModelRequest)
    {
        var user = await userManager.FindByEmailAsync(createReservationModelRequest.UserEmail);

        var userHasReservationOfThisBook = reservationQueries.UserHasReservationTheBook(createReservationModelRequest.BookId, user.Id);

        if (userHasReservationOfThisBook)
        {
            return null;
        }

        var book = this.bookQueries.GetBookById(createReservationModelRequest.BookId);

        return reservationCommands.RequestReservation(user, book);
    }

    public async Task<Reservation> UpdateReservation(int reservationId, bool isRejected)
    {
        var reservationUpdated = reservationCommands.UpdateReservation(reservationId, isRejected);

        if (reservationUpdated is null)
        {
            return null;
        }

        var statusReserved = Enumerators.ReservationStatus.Reserved.ToString();
        var statusReturned = Enumerators.ReservationStatus.Returned.ToString();
        var bookFromDb = this.bookQueries.GetBookById(reservationUpdated.BookId);

        if (reservationUpdated.Status.Name == statusReserved)
        {
            bookFromDb.Quantity -= 1;
            bookCommands.UpdateBookPartially(bookFromDb, "");
            await this.mailService.SendReservationApprovalEmail(reservationUpdated.User.Email, reservationUpdated.Book.Title, reservationUpdated.Book.ISBN, reservationUpdated.User.FirstName);
        }

        if (reservationUpdated.Status.Name == statusReturned)
        {
            var reservationBook = reservationUpdated.Book;

            var bookReservations = reservationBook.Reservations.ToList();

            bookReservations.Remove(reservationUpdated);

            reservationBook.Reservations = bookReservations;

            bookFromDb.Quantity += 1;
            bookCommands.UpdateBookPartially(bookFromDb, "");
        }

        if (isRejected)
        {
            await this.mailService.SendReservationRejectionMail(reservationUpdated.User.Email, reservationUpdated.User.FirstName);
        }

        return reservationUpdated;
    }


    public async Task<IEnumerable<ReadReservationModel>> GetAllReservations(int page, int itemsPerPage, string? userEmail, string? reservationStatus)
    {
        string userId = await GetUserIdIfNotAdmin(userEmail);

        var reservationsQuery = reservationQueries.GetAllReservations(page, itemsPerPage, userId, reservationStatus);
        reservationsQuery.ToList().ForEach(r =>
        {
            mapper.Map<ReadBookModel>(r.Book);
        });

        return mapper.Map<IQueryable<Reservation>, List<ReadReservationModel>>(reservationsQuery).ToList();
    }

    public async Task<int> GetReservationsTotalCount(string? userEmail, string? reservationStatus)
    {
        string userId = await GetUserIdIfNotAdmin(userEmail);

        return reservationQueries.GetReservationsTotalCount(userId, reservationStatus);
    }

    private async Task<string> GetUserIdIfNotAdmin(string? userEmail)
    {
        string userId = "";

        if (userEmail is not null)
        {
            var user = await userManager.FindByEmailAsync(userEmail);

            var isUserAdmin = await userManager.IsInRoleAsync(user, Librarian);

            if (!isUserAdmin)
            {
                userId = user.Id;
            }
        }

        return userId;
    }

    public IEnumerable<ReadReservationModel> GetPendingReturns(int page, int itemsPerPage)
    {
        var pendingReturnsQuery = reservationQueries.GetPendingReturns(page, itemsPerPage);

        pendingReturnsQuery.ToList().ForEach(r =>
        {
            mapper.Map<ReadBookModel>(r.Book);
        });

        return mapper.Map<IQueryable<Reservation>, List<ReadReservationModel>>(pendingReturnsQuery).ToList();
    }

    public bool CheckIfBorrowed(int bookId, string userId)
    {
        return this.reservationQueries.CheckIfBorrowed(bookId, userId);
    }

    public Reservation RequestProlongation(int reservationId)
    {
        return this.reservationCommands.RequestProlongation(reservationId);
    }

    public async Task<Reservation> RejectProlongation(int reservationId)
    {
        var result = this.reservationCommands.RejectProlongation(reservationId);
        
        await this.mailService.SendRejectProlongationEmail(result.User.Email, result.User.FirstName, result.Book.Title);
        
        return result;
    }

    public int GetPendingReturnsCount()
    {
        return this.reservationQueries.GetPendingReturnsCount();
    }
}
