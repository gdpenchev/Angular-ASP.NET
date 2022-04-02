namespace CuriousReadersAPI.Controllers;

using System.Collections.Generic;

using CuriousReadersData.Dto.Reservations;
using CuriousReadersService.Services.Reservation;
using CuriousReadersService.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static CuriousReadersData.DataConstants;

[Route("api/[controller]")]
[ApiController]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService reservationService;

    public ReservationsController(IReservationService reservationService)
    {
        this.reservationService = reservationService;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ReservationRequest>> RequestReservation(ReservationRequest reservationRequest)
    {
        var result = await reservationService.RequestReservation(reservationRequest);

        if (result is null)
        {
            return BadRequest("This user has reservation for this book.");
        }

        return Created("api/[controller]", result);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<ReadReservationModel>>> GetAllReservations(int page = 1, int itemsPerPage = 12, string? userEmail = "", string? reservationStatus = "")
    {
        var result = await this.reservationService.GetAllReservations(page, itemsPerPage, userEmail, reservationStatus);

        return Ok(result);
    }

    [HttpGet("Count")]
    [Authorize]
    public async Task<ActionResult<int>> GetReservationsTotalCount(string? userEmail = "", string? reservationStatus = "")
    {
        var userReservationsTotalCount = await this.reservationService.GetReservationsTotalCount(userEmail, reservationStatus);

        return Ok(userReservationsTotalCount);
    }

    [HttpPatch("{reservationId}/{isRejected}")]
    [Authorize(Roles = Librarian)]
    public async Task<ActionResult> UpdateReservation(int reservationId, bool isRejected)
    {
        var result = await reservationService.UpdateReservation(reservationId, isRejected);

        if (result is null)
        {
            return BadRequest("This book cannot be reserved.");
        }

        return Created("api/[controller]", result);
    }

    [HttpGet("PendingReturn")]
    [Authorize(Roles = Librarian)]
    public ActionResult<IEnumerable<ReadReservationModel>> PendingReturn(int page, int itemsPerPage)
    {
        var result = this.reservationService.GetPendingReturns(page, itemsPerPage);
        return Ok(result);
    }

    [HttpGet("CheckStatus")]
    [Authorize]
    public IActionResult CheckStatus(int bookId, string userId)
    {
        var result = this.reservationService.CheckIfBorrowed(bookId, userId);

        return Ok(result);
    }
    [HttpPatch("Prolongation")]
    [Authorize]
    public IActionResult RequestProlongation(int reservationId)
    {
        var result = this.reservationService.RequestProlongation(reservationId);

        if (result is null)
        {
            return BadRequest("This reservation could not be prolonged");
        }
        return Ok();
    }
    [HttpPatch("Reject")]
    [Authorize(Roles = Librarian)]
    public IActionResult RejectProlongation(int reservationId)
    {
        var result = this.reservationService.RejectProlongation(reservationId);

        return Ok();
    }
    [HttpGet("PendingCount")]
    [Authorize(Roles = Librarian)]
    public ActionResult<int> GetPendingReturnsCount()
    {
        var result = this.reservationService.GetPendingReturnsCount();

        return Ok(result);
    }
}
