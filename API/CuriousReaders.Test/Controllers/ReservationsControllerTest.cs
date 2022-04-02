namespace CuriousReaders.Test.Controllers;

using System.Collections.Generic;
using System.Threading.Tasks;

using CuriousReadersAPI.Controllers;
using CuriousReadersData.Dto.Reservations;
using CuriousReadersData.Entities;
using CuriousReadersService.Services.Reservation;
using CuriousReadersService.Request;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Xunit;

public class ReservationsControllerTest
{
    private IReservationService reservationServiceMock = A.Fake<IReservationService>();

    [Fact]
    public async void RequestReservation_Returns_CreatedResult()
    {
        //Arrange
        A.CallTo(() => this.reservationServiceMock.RequestReservation(A<ReservationRequest>.Ignored))
            .Returns(new Reservation());


        var reservationController = new ReservationsController(this.reservationServiceMock);
        var createReservationRequest = A.Fake<ReservationRequest>();

        //Act
        var result = await reservationController.RequestReservation(createReservationRequest);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<CreatedResult>(result.Result);
        Assert.IsType<ActionResult<ReservationRequest>>(result);
    }

    [Fact]
    public async void RequestReservation_Returns_BadRequest_IfUserHasRequestedTheBook()
    {
        //Arrange
        var serviceResponse = new Reservation();
        A.CallTo(() => this.reservationServiceMock.RequestReservation(A<ReservationRequest>.Ignored))
            .Returns(serviceResponse = null);

        var reservationController = new ReservationsController(this.reservationServiceMock);

        ReservationRequest reservationRequest = null;

        //Act
        var result = await reservationController.RequestReservation(reservationRequest);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.IsType<ActionResult<ReservationRequest>>(result);
    }


    [Fact]
    public void GetAllReservations_Returns_OkObjectResult()
    {
        //Arrange
        int page = 1;
        int limit = 20;
        string userEmail = "email@gmail.com";
        string reservationStatusFilter = "";

        A.CallTo(() => this.reservationServiceMock.GetAllReservations(page, limit, userEmail, reservationStatusFilter))
              .Returns(new List<ReadReservationModel>());

        var reservationController = new ReservationsController(this.reservationServiceMock);

        //Act
        var result = reservationController.GetAllReservations(page, limit, userEmail, reservationStatusFilter);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<Task<ActionResult<IEnumerable<ReadReservationModel>>>>(result);
    }

    [Fact]
    public void GetReservationsTotalCount_Returns_TotalReservationsForTheUserCount()
    {
        //Arrange
        string userEmail = "email@gmail.com";
        string reservationStatusFilter = "";

        A.CallTo(() => this.reservationServiceMock.GetReservationsTotalCount(userEmail, reservationStatusFilter))
              .Returns(new int());

        var reservationController = new ReservationsController(this.reservationServiceMock);

        //Act
        var result = reservationController.GetReservationsTotalCount(userEmail, reservationStatusFilter);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<Task<ActionResult<int>>>(result);
    }

    [Fact]
    public void GetAllPendingReturnReservations_Returns_OkObjectResult()
    {
        //Arrange
        int page = 1;
        int limit = 20;

        A.CallTo(() => this.reservationServiceMock.GetPendingReturns(page, limit))
              .Returns(new List<ReadReservationModel>());

        var reservationController = new ReservationsController(this.reservationServiceMock);

        //Act
        var result = reservationController.PendingReturn(page, limit);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<ActionResult<IEnumerable<ReadReservationModel>>>(result);
    }

    [Fact]
    public void RequestProlongation_Returns_OkResult()
    {
        //Arrange
        int reservationId = 1;

        A.CallTo(() => this.reservationServiceMock.RequestProlongation(reservationId))
              .Returns(new Reservation());

        var reservationController = new ReservationsController(this.reservationServiceMock);

        //Act
        var result = reservationController.RequestProlongation(reservationId);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public void RequestProlongation_Returns_BadRequest_IfResultIsNull()
    {
        //Arrange
        int reservationId = 1;

        A.CallTo(() => this.reservationServiceMock.RequestProlongation(reservationId))
              .Returns(null);

        var reservationController = new ReservationsController(this.reservationServiceMock);

        //Act
        var result = reservationController.RequestProlongation(reservationId);

        //Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void RejectProlongation_Returns_OkResult()
    {
        //Arrange
        int reservationId = 1;

        A.CallTo(() => this.reservationServiceMock.RejectProlongation(reservationId))
              .Returns(new Reservation());

        var reservationController = new ReservationsController(this.reservationServiceMock);

        //Act
        var result = reservationController.RejectProlongation(reservationId);

        //Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public void GetPendingReturnsCount_Returns_CorrentNumber()
    {

        A.CallTo(() => this.reservationServiceMock.GetPendingReturnsCount())
              .Returns(new int());

        var reservationController = new ReservationsController(this.reservationServiceMock);

        //Act
        var result = reservationController.GetPendingReturnsCount();

        //Assert
        Assert.IsType<ActionResult<int>>(result);
    }

    [Fact]
    public void CheckStatus_ShouldReturnOkObject_IfBookIsNotBorrowed()
    {
        //Arrange
        A.CallTo(() => this.reservationServiceMock.CheckIfBorrowed(A<int>.Ignored, A<string>.Ignored))
              .Returns(false);

        var reservationController = new ReservationsController(this.reservationServiceMock);

        //Act
        var bookId = 1;
        var userId = "testUserId";

        var result = reservationController.CheckStatus(bookId, userId);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void CheckStatus_ShouldReturnOkObject_IfBookIsBorrowed()
    {
        //Arrange
        A.CallTo(() => this.reservationServiceMock.CheckIfBorrowed(A<int>.Ignored, A<string>.Ignored))
              .Returns(true);

        var reservationController = new ReservationsController(this.reservationServiceMock);

        //Act
        var bookId = 1;
        var userId = "testUserId";

        var result = reservationController.CheckStatus(bookId, userId);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async void UpdateReservation_Returns_CreatedResult_IfResultIsNotNull()
    {
        //Arrange
        A.CallTo(() => this.reservationServiceMock.UpdateReservation(A<int>.Ignored, A<bool>.Ignored))
            .Returns(new Reservation());


        var reservationController = new ReservationsController(this.reservationServiceMock);
        var bookId = 1;
        var isRejected = false;

        //Act
        var result = await reservationController.UpdateReservation(bookId, isRejected);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<CreatedResult>(result);
    }

    [Fact]
    public async void UpdateReservation_Returns_Returns_BadRequest_IfResultIsNull()
    {
        //Arrange
        var serviceResponse = new Reservation();
        A.CallTo(() => this.reservationServiceMock.UpdateReservation(A<int>.Ignored, A<bool>.Ignored))
            .Returns(serviceResponse = null);

        var reservationController = new ReservationsController(this.reservationServiceMock);

        ReservationRequest reservationRequest = null;
        var bookId = 1;
        var isRejected = false;

        //Act
        var result = await reservationController.UpdateReservation(bookId, isRejected);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }
}
