namespace CuriousReaders.Test.Services;

using AutoMapper;
using CuriousReadersData.Commands;
using CuriousReadersData.Dto.Reservations;
using CuriousReadersData.Entities;
using CuriousReadersData.Profiles;
using CuriousReadersData.Queries;
using CuriousReadersService.Services.Reservation;
using CuriousReadersService.Request;
using CuriousReadersService.Services.Mail;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Xunit;
using System.Threading.Tasks;
using CuriousReadersData;

public class ReservationsServiceTest
{
    private readonly IMapper mapper;
    private readonly IReservationQueries reservationQueryMock = A.Fake<IReservationQueries>();
    private readonly IReservationCommands reservationCommandMock = A.Fake<IReservationCommands>();
    private readonly IBookQueries bookQueryMock = A.Fake<IBookQueries>();
    private readonly IBookCommands bookCommandMock = A.Fake<IBookCommands>();
    private readonly IQueryable<Reservation> reservationsMockQueryable = A.Fake<IQueryable<Reservation>>();
    private readonly IQueryable<ReadReservationModel> readReservationModelListMock = A.Fake<IQueryable<ReadReservationModel>>();
    private readonly IMailService mailService = A.Fake<IMailService>();
    private readonly int reservationsTotalCount = 0;
    private readonly Reservation reservationMock = A.Fake<Reservation>();
    private ReservationService reservationService;
    private UserManager<User> userManager = A.Fake<UserManager<User>>();

    public ReservationsServiceTest()
    {
        var config = new MapperConfiguration(config =>
        {
            config.AddProfile(new BooksProfile());
            config.AddProfile(new AuthorsProfile());
            config.AddProfile(new ReservationProfile());
        });

        this.mapper = new Mapper(config);
    }

    private void SetupService()
    {
        reservationService = new ReservationService(reservationQueryMock, reservationCommandMock, bookQueryMock, bookCommandMock, mapper, userManager, mailService);
    }

    [Fact]
    public async void RequestReservation_Creates_NewReservationRequest()
    {
        //Arrange
        A.CallTo(() => reservationQueryMock.UserHasReservationTheBook(A<int>.Ignored, A<string>.Ignored))
            .Returns(false);

        A.CallTo(() => bookQueryMock.GetBookById(A<int>.Ignored))
            .Returns(new Book());

        SetupService();

        //Act
        var result = await reservationService.RequestReservation(new ReservationRequest());

        //Assert
        A.CallTo(() => reservationCommandMock.RequestReservation(A<User>.Ignored, A<Book>.Ignored))
            .MustHaveHappenedOnceExactly();

        Assert.NotNull(result);
        //Assert.Equal(reservationMock, result);
    }

    [Fact]
    public async void CreateBook_ShouldNot_CreateBook_With_ExistingISBN()
    {
        //Arrange
        A.CallTo(() => reservationQueryMock.UserHasReservationTheBook(A<int>.Ignored, A<string>.Ignored))
            .Returns(true);

        SetupService();

        //Act
        var result = await reservationService.RequestReservation(new ReservationRequest());

        //Assert
        A.CallTo(() => bookQueryMock.GetBookById(A<int>.Ignored))
            .MustNotHaveHappened();

        A.CallTo(() => reservationCommandMock.RequestReservation(A<User>.Ignored, A<Book>.Ignored))
            .MustNotHaveHappened();

        Assert.Null(result);
    }

    [Fact]
    public async void GetAllReservationsByUserId_Returns_AllUserReservations()
    {
        //Arrange
        int page = 1;
        int pageSize = 20;
        string userId = "testId";
        string reservationStatus = "";

        SetupService();

        A.CallTo(() => reservationQueryMock.GetAllReservations(A<int>.Ignored, A<int>.Ignored, A<string>.Ignored, A<string>.Ignored))
            .Returns(reservationsMockQueryable);

        //Act
        var result = await reservationService.GetAllReservations(page, pageSize, userId, reservationStatus);


        //Assert
        Assert.NotNull(result);
        Assert.Equal(readReservationModelListMock, result);
    }

    [Fact]
    public async void GetUserReservationsTotalCount_Returns_Reservations_Total_Count()
    {
        //Arrange
        string userEmail = "email@gmail.com";
        string reservationStatus = "";

        SetupService();

        A.CallTo(() => reservationQueryMock.GetReservationsTotalCount(A<string>.Ignored, A<string>.Ignored))
            .Returns(reservationsTotalCount);

        //Act
        var result = await reservationService.GetReservationsTotalCount(userEmail, reservationStatus);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(reservationsTotalCount, result);
    }

    [Fact]
    public void GetPendingReturn_Returns_AllUserPendingReturns()
    {
        //Arrange
        int page = 1;
        int pageSize = 20;

        SetupService();

        A.CallTo(() => reservationQueryMock.GetPendingReturns(A<int>.Ignored, A<int>.Ignored))
            .Returns(reservationsMockQueryable);

        //Act
        var result = reservationService.GetPendingReturns(page, pageSize);


        //Assert
        Assert.NotNull(result);
        Assert.Equal(readReservationModelListMock, result);
    }

    [Fact]
    public void RequestProlongation_Returns_ReservationWithProlongedStatus()
    {
        //Arrange
        int reservationId = 1;

        SetupService();

        A.CallTo(() => reservationCommandMock.RequestProlongation(A<int>.Ignored))
            .Returns(new Reservation());

        //Act
        var result = reservationService.RequestProlongation(reservationId);


        //Assert
        Assert.NotNull(result);
        A.CallTo(() => reservationCommandMock.RequestProlongation(A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void RejectProlongation_Returns_RejectedReservation()
    {
        //Arrange
        int reservationId = 1;

        SetupService();

        A.CallTo(() => reservationCommandMock.RejectProlongation(A<int>.Ignored))
            .Returns(new Reservation());

        //Act
        var result = reservationService.RejectProlongation(reservationId);


        //Assert
        Assert.NotNull(result);
        A.CallTo(() => reservationCommandMock.RejectProlongation(A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void GetPendingReturnsCount_Returns_CorrentNumber()
    {

        A.CallTo(() => this.reservationQueryMock.GetPendingReturnsCount())
              .Returns(1);

        SetupService();


        //Act
        var result = reservationService.GetPendingReturnsCount();

        //Assert
        Assert.Equal(1, result);
    }


    [Fact]
    public void CheckIfBorrowed_ReturnsTrue_IfTheUserBorrowedTheBook()
    {
        var bookId = 1;
        var userId = "testUserId";
        A.CallTo(() => this.reservationQueryMock.CheckIfBorrowed(A<int>.Ignored, A<string>.Ignored))
              .Returns(true);

        SetupService();


        //Act
        var result = reservationService.CheckIfBorrowed(bookId, userId);

        //Assert
        Assert.Equal(true, result);
    }

    [Fact]
    public void CheckIfBorrowed_ReturnsFalse_IfTheUserNotBorrowedTheBook()
    {
        var bookId = 1;
        var userId = "testUserId";
        A.CallTo(() => this.reservationQueryMock.CheckIfBorrowed(A<int>.Ignored, A<string>.Ignored))
              .Returns(false);

        SetupService();


        //Act
        var result = reservationService.CheckIfBorrowed(bookId, userId);

        //Assert
        Assert.Equal(false, result);
    }

    [Fact]
    public void UpdateReservation_ReturnsIsFaultedTask_IfReservationNotUpdated()
    {
        var bookId = 1;
        var isRejected = false;

        A.CallTo(() => this.reservationCommandMock.UpdateReservation(A<int>.Ignored, A<bool>.Ignored))
              .Returns(new Reservation());

        SetupService();


        //Act
        var result = reservationService.UpdateReservation(bookId, isRejected);

        //Assert
        Assert.IsType<Task<Reservation>>(result);
        Assert.True(result.IsFaulted);
    }

    [Fact]
    public void UpdateReservation_ReturnsReservationWithStatusReserved_IfReservationStatusIsReserved()
    {
        var bookId = 1;
        var isRejected = false;
        var returnedReservation = new Reservation()
        {
            Status = new Status
            {
                Name = Enumerators.ReservationStatus.Reserved.ToString()
            },
            Book = new Book(),
            User = new User()
        };

        A.CallTo(() => this.reservationCommandMock.UpdateReservation(A<int>.Ignored, A<bool>.Ignored))
              .Returns(returnedReservation);

        SetupService();


        //Act
        var result = reservationService.UpdateReservation(bookId, isRejected);

        //Assert
        Assert.IsType<Task<Reservation>>(result);
        Assert.Equal(Enumerators.ReservationStatus.Reserved.ToString(), result.Result.Status.Name);
    }

    [Fact]
    public void UpdateReservation_ReturnsReservationWithStatusReturned_IfReservationStatusIsReturned()
    {
        var bookId = 1;
        var isRejected = false;
        var returnedReservation = new Reservation()
        {
            Status = new Status
            {
                Name = Enumerators.ReservationStatus.Returned.ToString()
            },
            Book = new Book(),
            User = new User()
        };

        A.CallTo(() => this.reservationCommandMock.UpdateReservation(A<int>.Ignored, A<bool>.Ignored))
              .Returns(returnedReservation);

        SetupService();


        //Act
        var result = reservationService.UpdateReservation(bookId, isRejected);

        //Assert
        Assert.IsType<Task<Reservation>>(result);
        Assert.Equal(Enumerators.ReservationStatus.Returned.ToString(), result.Result.Status.Name);
    }

    [Fact]
    public void UpdateReservation_ReturnsReservationWithStatuRejected_IfReservationStatusIsRejected()
    {
        var bookId = 1;
        var isRejected = false;
        var returnedReservation = new Reservation()
        {
            Status = new Status
            {
                Name = Enumerators.ReservationStatus.Rejected.ToString()
            },
            Book = new Book(),
            User = new User()
        };

        A.CallTo(() => this.reservationCommandMock.UpdateReservation(A<int>.Ignored, A<bool>.Ignored))
              .Returns(returnedReservation);

        SetupService();


        //Act
        var result = reservationService.UpdateReservation(bookId, isRejected);

        //Assert
        Assert.IsType<Task<Reservation>>(result);
        Assert.Equal(Enumerators.ReservationStatus.Rejected.ToString(), result.Result.Status.Name);
    }
}
