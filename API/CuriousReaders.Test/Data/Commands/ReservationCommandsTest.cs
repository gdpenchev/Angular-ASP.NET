using CuriousReadersData;
using CuriousReadersData.Commands;
using CuriousReadersData.Entities;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CuriousReaders.Test.Data.Commands;

public class ReservationCommandsTest
{
    private LibraryDbContext fakeDbContext = A.Fake<LibraryDbContext>();
    private Reservation fakeReservation = A.Fake<Reservation>();
    private Book fakeBook = A.Fake<Book>();
    private User fakeUser = A.Fake<User>();

    private void SetupFakeCommentsDbSet(IQueryable<Reservation> fakeIQueryable)
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
    private void SetupFakeStatusDbSet(IQueryable<Status> fakeIQueryable)
    {
        var fakeDbSet = A.Fake<DbSet<Status>>((d =>
                 d.Implements(typeof(IQueryable<Status>))));

        A.CallTo(() => ((IQueryable<Status>)fakeDbSet).GetEnumerator())
            .Returns(fakeIQueryable.GetEnumerator());

        A.CallTo(() => ((IQueryable<Status>)fakeDbSet).Provider)
            .Returns(fakeIQueryable.Provider);

        A.CallTo(() => ((IQueryable<Status>)fakeDbSet).Expression)
            .Returns(fakeIQueryable.Expression);

        A.CallTo(() => ((IQueryable<Status>)fakeDbSet).ElementType)
           .Returns(fakeIQueryable.ElementType);

        A.CallTo(() => fakeDbContext.Statuses).Returns(fakeDbSet);
    }

    [Fact]
    public void RequestReservation_ShouldCreate_NewReservation()
    {
        //Arrange
        var fakeIQueryableComments = new List<Reservation>()
        {
            new Reservation() { Id = 1, BookId = 1, UserId = "test", Status = new Status{ Name = Enumerators.ReservationStatus.Reserved.ToString() }, RequestDate = DateTime.Now  },
        }
        .AsQueryable();

        var fakeIqueryStatus = new List<Status>()
        {
            new Status{ Name = Enumerators.ReservationStatus.Reserved.ToString() }

        }.AsQueryable();

        SetupFakeCommentsDbSet(fakeIQueryableComments);

        SetupFakeStatusDbSet(fakeIqueryStatus);

        var reservationCommand = new ReservationCommands(fakeDbContext);

        //Act
        var result = reservationCommand.RequestReservation(fakeUser,fakeBook);

        //Assert
        A.CallTo(() => fakeDbContext.Reservations.Add(result))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeDbContext.SaveChanges())
           .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void RequestProlongation_ShouldUpdate_Reservation()
    {
        //Arrange
        var fakeIQueryableComments = new List<Reservation>()
        {
            new Reservation() { Id = 1, BookId = 1, UserId = "test", Status = new Status{ Name = Enumerators.ReservationStatus.Reserved.ToString() }, RequestDate = DateTime.Now  },
        }
        .AsQueryable();

        var fakeIqueryStatus = new List<Status>()
        {
            new Status{ Name = Enumerators.ReservationStatus.Reserved.ToString() }

        }.AsQueryable();

        SetupFakeCommentsDbSet(fakeIQueryableComments);

        SetupFakeStatusDbSet(fakeIqueryStatus);

        var reservationId = 1;

        var reservationCommand = new ReservationCommands(fakeDbContext);

        //Act
        var result = reservationCommand.RequestProlongation(reservationId);

        //Assert
        A.CallTo(() => fakeDbContext.Reservations.Update(result))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeDbContext.SaveChanges())
           .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void RejectProlongation_ShouldUpdate_Reservation()
    {
        //Arrange
        var fakeIQueryableComments = new List<Reservation>()
        {
            new Reservation() { Id = 1, BookId = 1, UserId = "test", Status = new Status{ Name = Enumerators.ReservationStatus.Reserved.ToString() }, RequestDate = DateTime.Now  },
        }
        .AsQueryable();

        var fakeIqueryStatus = new List<Status>()
        {
            new Status{ Name = Enumerators.ReservationStatus.Reserved.ToString() }

        }.AsQueryable();

        SetupFakeCommentsDbSet(fakeIQueryableComments);

        SetupFakeStatusDbSet(fakeIqueryStatus);

        var reservationId = 1;

        var reservationCommand = new ReservationCommands(fakeDbContext);

        //Act
        var result = reservationCommand.RejectProlongation(reservationId);

        //Assert
        A.CallTo(() => fakeDbContext.Reservations.Update(result))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeDbContext.SaveChanges())
           .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void UpdateReservation_ShouldUpdate_Reservation()
    {
        //Arrange
        var fakeIQueryableComments = new List<Reservation>()
        {
            new Reservation() { Id = 1, BookId = 1, UserId = "test", Status = new Status{ Name = Enumerators.ReservationStatus.Reserved.ToString() }, RequestDate = DateTime.Now  },
        }
        .AsQueryable();

        var fakeIqueryStatus = new List<Status>()
        {
            new Status{ Name = Enumerators.ReservationStatus.Reserved.ToString() }

        }.AsQueryable();

        SetupFakeCommentsDbSet(fakeIQueryableComments);

        SetupFakeStatusDbSet(fakeIqueryStatus);

        var reservationId = 1;
        var isRejected = false;

        var reservationCommand = new ReservationCommands(fakeDbContext);

        //Act
        var result = reservationCommand.UpdateReservation(reservationId, isRejected);

        //Assert
        A.CallTo(() => fakeDbContext.Reservations.Update(result))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeDbContext.SaveChanges())
           .MustHaveHappenedOnceExactly();
    }
}
