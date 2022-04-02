namespace CuriousReaders.Test.Data.Queries;

using CuriousReadersData;
using CuriousReadersData.Entities;
using CuriousReadersData.Queries;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class ReservationQueriesTest
{
    private LibraryDbContext fakeDbContext = A.Fake<LibraryDbContext>();

    private void SetupFakeDbSet(IQueryable<Reservation> fakeIQueryable)
    {
        var fakeDbSet = A.Fake<DbSet<Reservation>>((d =>
                 d.Implements(typeof(IQueryable<Reservation>))));

        A.CallTo(() => ((IQueryable<Reservation>)fakeDbSet).GetEnumerator())
            .Returns(fakeIQueryable.GetEnumerator());

        A.CallTo(() => ((IQueryable<Reservation>)fakeDbSet).Provider)
            .Returns(fakeIQueryable.Provider);

        A.CallTo(() => ((IQueryable<Reservation>)fakeDbSet).Expression)
            .Returns(fakeIQueryable.Expression);

        A.CallTo(() => ((IQueryable<Reservation>)fakeDbSet).ElementType)
           .Returns(fakeIQueryable.ElementType);

        A.CallTo(() => fakeDbContext.Reservations).Returns(fakeDbSet);
    }

    [Fact]
    public void CheckIfBorrowed_Should_Return_True_IfTheUserHasReservationForThisBook()
    {
        //Arrange
        var fakeIQueryable = new List<Reservation>()
        {
            new Reservation()
            {
                BookId = 1,
                UserId = "testUserId",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Borrowed.ToString()
                }
            },
            new Reservation()
            {
                BookId = 1,
                UserId = "testUserId2",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Borrowed.ToString()
                }
            }
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var genreQueries = new ReservationQueries(fakeDbContext);

        var bookId = 1;
        var userId = "testUserId";
        var reservationStatus = Enumerators.ReservationStatus.Borrowed.ToString();

        var expectedResult = fakeIQueryable
            .Where(r => r.BookId == bookId && r.UserId == userId && r.Status.Name == Enumerators.ReservationStatus.Borrowed.ToString())
            .Any();

        //Act
        var result = genreQueries.CheckIfBorrowed(bookId, userId);

        //Assert
        Assert.Equal(true, result);
    }

    [Fact]
    public void CheckIfBorrowed_Should_Return_False_IfTheUserHasNotReservedForThisBook()
    {
        //Arrange
        var fakeIQueryable = new List<Reservation>()
        {
            new Reservation()
            {
                BookId = 1,
                UserId = "testUserId2",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Borrowed.ToString()
                }
            },
            new Reservation()
            {
                BookId = 1,
                UserId = "testUserId2",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Borrowed.ToString()
                }
            }
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var genreQueries = new ReservationQueries(fakeDbContext);

        var bookId = 1;
        var userId = "testUserId";
        var reservationStatus = Enumerators.ReservationStatus.Borrowed.ToString();

        var expectedResult = fakeIQueryable
            .Where(r => r.BookId == bookId && r.UserId == userId && r.Status.Name == Enumerators.ReservationStatus.Borrowed.ToString())
            .Any();

        //Act
        var result = genreQueries.CheckIfBorrowed(bookId, userId);

        //Assert
        Assert.Equal(false, result);
    }

    [Fact]
    public void GetAllReservations_Should_Return_AllReservationsWithThePassedStatus_ForThePassedUserId()
    {
        //Arrange
        var fakeIQueryable = new List<Reservation>()
        {
            new Reservation() 
            { 
                UserId = "testUserId",
                Status = new Status() 
                { 
                    Name = Enumerators.ReservationStatus.Borrowed.ToString() 
                }
            },
            new Reservation() { Status = new Status() { Name = Enumerators.ReservationStatus.Reserved.ToString() }},
            new Reservation()
            {
                UserId = "testUserId",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Borrowed.ToString()
                }
            }
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var genreQueries = new ReservationQueries(fakeDbContext);

        var userId = "testUserId";
        var reservationStatus = Enumerators.ReservationStatus.Borrowed.ToString();

        var expectedResult = fakeIQueryable.Where(r => string.IsNullOrEmpty(reservationStatus) ? r.Status.Name != null &&
                (string.IsNullOrEmpty(userId) ? r.UserId != userId : r.UserId == userId &&
                !(string.IsNullOrEmpty(r.UserId))) : r.Status.Name == reservationStatus &&
                (string.IsNullOrEmpty(userId) ? r.UserId != userId : r.UserId == userId &&
                !string.IsNullOrEmpty(r.UserId)));

        //Act
        var result = genreQueries.GetAllReservations(1, 12, userId, reservationStatus);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetAllReservations_Should_Return_AllReservations_ForThePassedUserId_IfReservationStatusIsNotPassed()
    {
        //Arrange
        var fakeIQueryable = new List<Reservation>()
        {
            new Reservation()
            {
                UserId = "testUserId",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.PendingReservationApproval.ToString()
                }
            },
            new Reservation() { Status = new Status() { Name = Enumerators.ReservationStatus.Reserved.ToString() }},
            new Reservation()
            {
                UserId = "testUserId",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Reserved.ToString()
                }
            },
            new Reservation()
            {
                UserId = "testUserId",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Borrowed.ToString()
                }
            },
            new Reservation()
            {
                UserId = "testUserId2",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Borrowed.ToString()
                }
            },
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var genreQueries = new ReservationQueries(fakeDbContext);

        var userId = "testUserId";
        var reservationStatus = "";

        var expectedResult = fakeIQueryable.Where(r => string.IsNullOrEmpty(reservationStatus) ? r.Status.Name != null &&
                (string.IsNullOrEmpty(userId) ? r.UserId != userId : r.UserId == userId &&
                !(string.IsNullOrEmpty(r.UserId))) : r.Status.Name == reservationStatus &&
                (string.IsNullOrEmpty(userId) ? r.UserId != userId : r.UserId == userId &&
                !string.IsNullOrEmpty(r.UserId)));

        //Act
        var result = genreQueries.GetAllReservations(1, 12, userId, reservationStatus);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetAllReservations_Should_Return_AllReservationsWithThePassedStatus_If_UserIdIsNotPassed()
    {
        //Arrange
        var fakeIQueryable = new List<Reservation>()
        {
            new Reservation()
            {
                UserId = "testUserId",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.PendingReservationApproval.ToString()
                }
            },
            new Reservation() { Status = new Status() { Name = Enumerators.ReservationStatus.Reserved.ToString() }},
            new Reservation()
            {
                UserId = "testUserId",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Reserved.ToString()
                }
            },
            new Reservation()
            {
                UserId = "testUserId2",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Borrowed.ToString()
                }
            }
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var genreQueries = new ReservationQueries(fakeDbContext);

        var userId = "";
        var reservationStatus = Enumerators.ReservationStatus.Borrowed.ToString();

        var expectedResult = fakeIQueryable.Where(r => string.IsNullOrEmpty(reservationStatus) ? r.Status.Name != null &&
                (string.IsNullOrEmpty(userId) ? r.UserId != userId : r.UserId == userId &&
                !(string.IsNullOrEmpty(r.UserId))) : r.Status.Name == reservationStatus &&
                (string.IsNullOrEmpty(userId) ? r.UserId != userId : r.UserId == userId &&
                !string.IsNullOrEmpty(r.UserId)));

        //Act
        var result = genreQueries.GetAllReservations(1, 12, userId, reservationStatus);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetAllReservations_Should_Return_AllReservationsForAllUsers_If_UserIdAndStatusAreNotPassed()
    {
        //Arrange
        var fakeIQueryable = new List<Reservation>()
        {
            new Reservation()
            {
                UserId = "testUserId",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.PendingReservationApproval.ToString()
                }
            },
            new Reservation() { Status = new Status() { Name = Enumerators.ReservationStatus.Reserved.ToString() }},
            new Reservation()
            {
                UserId = "testUserId",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Reserved.ToString()
                }
            },
            new Reservation()
            {
                UserId = "testUserId2",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Borrowed.ToString()
                }
            }
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var genreQueries = new ReservationQueries(fakeDbContext);

        var userId = "";
        var reservationStatus = "";

        var expectedResult = fakeIQueryable.Where(r => string.IsNullOrEmpty(reservationStatus) ? r.Status.Name != null &&
                (string.IsNullOrEmpty(userId) ? r.UserId != userId : r.UserId == userId &&
                !(string.IsNullOrEmpty(r.UserId))) : r.Status.Name == reservationStatus &&
                (string.IsNullOrEmpty(userId) ? r.UserId != userId : r.UserId == userId &&
                !string.IsNullOrEmpty(r.UserId)));

        //Act
        var result = genreQueries.GetAllReservations(1, 12, userId, reservationStatus);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void BooksNotOnTimeNotification_Should_Return_AllReservations_WithStatusBorrowed()
    {
        //Arrange
        var fakeIQueryable = new List<Reservation>()
        {
            new Reservation()
            {
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Borrowed.ToString()
                },
                ReturnDate = DateTime.Now.AddDays(2),
            },
            new Reservation()
            {
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Reserved.ToString()
                }
            }
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var genreQueries = new ReservationQueries(fakeDbContext);


        var expectedResult = fakeIQueryable.Where(r => r.Status.Name == Enumerators.ReservationStatus.Borrowed.ToString() && r.ReturnDate <= DateTime.Now);

        //Act
        var result = genreQueries.BooksNotOnTimeNotification(1, 12);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetReservationsTotalCount_Should_Return_AllReservationsWithThePassedStatus_ForThePassedUserId()
    {
        //Arrange
        var fakeIQueryable = new List<Reservation>()
        {
            new Reservation()
            {
                UserId = "testUserId",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Borrowed.ToString()
                }
            },
            new Reservation() { Status = new Status() { Name = Enumerators.ReservationStatus.Reserved.ToString() }},
            new Reservation()
            {
                UserId = "testUserId",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Borrowed.ToString()
                }
            }
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var genreQueries = new ReservationQueries(fakeDbContext);

        var userId = "testUserId";
        var reservationStatus = Enumerators.ReservationStatus.Borrowed.ToString();

        var expectedResult = fakeIQueryable.Where(r => string.IsNullOrEmpty(reservationStatus) ? r.Status.Name != null &&
                (string.IsNullOrEmpty(userId) ? r.UserId != userId : r.UserId == userId &&
                !(string.IsNullOrEmpty(r.UserId))) : r.Status.Name == reservationStatus &&
                (string.IsNullOrEmpty(userId) ? r.UserId != userId : r.UserId == userId &&
                !string.IsNullOrEmpty(r.UserId)))
                    .Count();

        //Act
        var result = genreQueries.GetReservationsTotalCount(userId, reservationStatus);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetReservationsTotalCount_Should_Return_AllReservations_ForThePassedUserId_IfReservationStatusIsNotPassed()
    {
        //Arrange
        var fakeIQueryable = new List<Reservation>()
        {
            new Reservation()
            {
                UserId = "testUserId",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.PendingReservationApproval.ToString()
                }
            },
            new Reservation() { Status = new Status() { Name = Enumerators.ReservationStatus.Reserved.ToString() }},
            new Reservation()
            {
                UserId = "testUserId",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Reserved.ToString()
                }
            },
            new Reservation()
            {
                UserId = "testUserId",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Borrowed.ToString()
                }
            },
            new Reservation()
            {
                UserId = "testUserId2",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Borrowed.ToString()
                }
            },
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var genreQueries = new ReservationQueries(fakeDbContext);

        var userId = "testUserId";
        var reservationStatus = "";

        var expectedResult = fakeIQueryable.Where(r => string.IsNullOrEmpty(reservationStatus) ? r.Status.Name != null &&
                (string.IsNullOrEmpty(userId) ? r.UserId != userId : r.UserId == userId &&
                !(string.IsNullOrEmpty(r.UserId))) : r.Status.Name == reservationStatus &&
                (string.IsNullOrEmpty(userId) ? r.UserId != userId : r.UserId == userId &&
                !string.IsNullOrEmpty(r.UserId)))
                    .Count();

        //Act
        var result = genreQueries.GetReservationsTotalCount(userId, reservationStatus);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetReservationsTotalCount_Should_Return_AllReservationsWithThePassedStatus_If_UserIdIsNotPassed()
    {
        //Arrange
        var fakeIQueryable = new List<Reservation>()
        {
            new Reservation()
            {
                UserId = "testUserId",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.PendingReservationApproval.ToString()
                }
            },
            new Reservation() { Status = new Status() { Name = Enumerators.ReservationStatus.Reserved.ToString() }},
            new Reservation()
            {
                UserId = "testUserId",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Reserved.ToString()
                }
            },
            new Reservation()
            {
                UserId = "testUserId2",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Borrowed.ToString()
                }
            }
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var genreQueries = new ReservationQueries(fakeDbContext);

        var userId = "";
        var reservationStatus = Enumerators.ReservationStatus.Borrowed.ToString();

        var expectedResult = fakeIQueryable.Where(r => string.IsNullOrEmpty(reservationStatus) ? r.Status.Name != null &&
                (string.IsNullOrEmpty(userId) ? r.UserId != userId : r.UserId == userId &&
                !(string.IsNullOrEmpty(r.UserId))) : r.Status.Name == reservationStatus &&
                (string.IsNullOrEmpty(userId) ? r.UserId != userId : r.UserId == userId &&
                !string.IsNullOrEmpty(r.UserId)))
                    .Count();

        //Act
        var result = genreQueries.GetReservationsTotalCount(userId, reservationStatus);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetReservationsTotalCount_Should_Return_AllReservationsForAllUsers_If_UserIdAndStatusAreNotPassed()
    {
        //Arrange
        var fakeIQueryable = new List<Reservation>()
        {
            new Reservation()
            {
                UserId = "testUserId",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.PendingReservationApproval.ToString()
                }
            },
            new Reservation() { Status = new Status() { Name = Enumerators.ReservationStatus.Reserved.ToString() }},
            new Reservation()
            {
                UserId = "testUserId",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Reserved.ToString()
                }
            },
            new Reservation()
            {
                UserId = "testUserId2",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Borrowed.ToString()
                }
            }
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var genreQueries = new ReservationQueries(fakeDbContext);

        var userId = "";
        var reservationStatus = "";

        var expectedResult = fakeIQueryable.Where(r => string.IsNullOrEmpty(reservationStatus) ? r.Status.Name != null &&
                (string.IsNullOrEmpty(userId) ? r.UserId != userId : r.UserId == userId &&
                !(string.IsNullOrEmpty(r.UserId))) : r.Status.Name == reservationStatus &&
                (string.IsNullOrEmpty(userId) ? r.UserId != userId : r.UserId == userId &&
                !string.IsNullOrEmpty(r.UserId)))
                    .Count();

        //Act
        var result = genreQueries.GetReservationsTotalCount(userId, reservationStatus);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetPendingReturns_Should_Return_AllReservationsWhichAreBorrowed_AndReturnDateExceed30DaysFromTheCurrentDate()
    {
        //Arrange
        var fakeIQueryable = new List<Reservation>()
        {
            new Reservation()
            {
                UserId = "testUserId",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Borrowed.ToString()
                },
                ReturnDate = DateTime.Now.AddDays(31),
            },
            new Reservation()
            {
                UserId = "testUserId",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Borrowed.ToString()
                },
                ReturnDate = DateTime.Now.AddDays(28),
            },
            new Reservation()
            {
                UserId = "testUserId2",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Borrowed.ToString()
                }
            }
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var genreQueries = new ReservationQueries(fakeDbContext);

        var expectedResult = fakeIQueryable.Where(r => r.Status.Name == Enumerators.ReservationStatus.Borrowed.ToString() && r.ReturnDate <= DateTime.Now.AddDays(30));

        //Act
        var result = genreQueries.GetPendingReturns(1, 12);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetPendingReturns_Should_ReturnCountOf_AllReservationsWhichAreBorrowed()
    {
        //Arrange
        var fakeIQueryable = new List<Reservation>()
        {
            new Reservation()
            {
                UserId = "testUserId",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Borrowed.ToString()
                },
                ReturnDate = DateTime.Now.AddDays(31),
            },
            new Reservation()
            {
                UserId = "testUserId",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Borrowed.ToString()
                },
                ReturnDate = DateTime.Now.AddDays(28),
            },
            new Reservation()
            {
                UserId = "testUserId2",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Borrowed.ToString()
                }
            }
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var genreQueries = new ReservationQueries(fakeDbContext);

        var expectedResult = fakeIQueryable
            .Where(r => r.Status.Name == Enumerators.ReservationStatus.Borrowed.ToString() && r.ReturnDate <= DateTime.Now.AddDays(30))
            .Count();

        //Act
        var result = genreQueries.GetPendingReturnsCount();

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void UserHasReservationTheBook_Should_ReturnTrue_IfUserHasReservationOfTheBook()
    {
        //Arrange
        var fakeIQueryable = new List<Reservation>()
        {
            new Reservation()
            {
                BookId = 1,
                UserId = "testUserId",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Reserved.ToString()
                }
            },
            new Reservation()
            {
                BookId = 2,
                UserId = "testUserId2",
                Status = new Status()
                {
                    Name = Enumerators.ReservationStatus.Borrowed.ToString()
                }
            }
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var genreQueries = new ReservationQueries(fakeDbContext);

        var bookId = 1;
        var userId = "testUserId";
        var expectedResult = fakeIQueryable.Any(r => (r.BookId == bookId && r.UserId == userId));

        //Act
        var result = genreQueries.UserHasReservationTheBook(bookId, userId);

        //Assert
        Assert.Equal(expectedResult, result);
    }
}
