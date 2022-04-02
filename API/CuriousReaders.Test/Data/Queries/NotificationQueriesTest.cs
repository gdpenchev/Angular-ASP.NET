namespace CuriousReaders.Test.Data.Queries;

using CuriousReadersData;
using CuriousReadersData.Entities;
using CuriousReadersData.Queries;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

public class NotificationQueriesTest
{
    private LibraryDbContext fakeDbContext = A.Fake<LibraryDbContext>();

    private void SetupFakeDbSet(IQueryable<Notification> fakeIQueryable)
    {
        var fakeDbSet = A.Fake<DbSet<Notification>>((d =>
                 d.Implements(typeof(IQueryable<Notification>))));

        A.CallTo(() => ((IQueryable<Notification>)fakeDbSet).GetEnumerator())
            .Returns(fakeIQueryable.GetEnumerator());

        A.CallTo(() => ((IQueryable<Notification>)fakeDbSet).Provider)
            .Returns(fakeIQueryable.Provider);

        A.CallTo(() => ((IQueryable<Notification>)fakeDbSet).Expression)
            .Returns(fakeIQueryable.Expression);

        A.CallTo(() => ((IQueryable<Notification>)fakeDbSet).ElementType)
           .Returns(fakeIQueryable.ElementType);

        A.CallTo(() => fakeDbContext.Notifications).Returns(fakeDbSet);
    }

    [Fact]
    public void GetAllNotification_Should_ReturnAllNotificationsForThisUser_FromDb()
    {
        //Arrange
        var fakeIQueryable = new List<Notification>()
        {
            new Notification()
            {
                Book = new Book(),
                BookId = 1,
                CreatedOn = DateTime.Now,
                Id = 1,
                IsRead = false,
                User = new User(),
                UserId = "testUserId1"
            },
            new Notification()
            {
                Book = new Book(),
                BookId = 2,
                CreatedOn = DateTime.Now,
                Id = 2,
                IsRead = false,
                User = new User(),
                UserId = "testUserId2"
            }
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var notificationQueries = new NotificationQueries(fakeDbContext);

        var userId = "testUserId1";
        var expectedResult = fakeIQueryable.Where(n => n.UserId == userId);

        //Act
        var result = notificationQueries.All(userId, 0, 12).AsQueryable();

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CountUnread_Should_Return_AllNotificationsCountForThisUser_FromDb()
    {
        //Arrange
        var fakeIQueryable = new List<Notification>()
        {
            new Notification()
            {
                Book = new Book(),
                BookId = 1,
                CreatedOn = DateTime.Now,
                Id = 1,
                IsRead = false,
                User = new User(),
                UserId = "testUserId1"
            },
            new Notification()
            {
                Book = new Book(),
                BookId = 2,
                CreatedOn = DateTime.Now,
                Id = 2,
                IsRead = false,
                User = new User(),
                UserId = "testUserId2"
            }
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var notificationQueries = new NotificationQueries(fakeDbContext);

        var userId = "testUserId1";
        var expectedResult = fakeIQueryable.Where(n => n.UserId == userId).Count();

        //Act
        var result = notificationQueries.CountUnread(userId);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ReadAllNotification_Should_Make_AllNotificationsForThisUserRead()
    {
        //Arrange
        var fakeIQueryable = new List<Notification>()
        {
            new Notification()
            {
                Book = new Book(),
                BookId = 1,
                CreatedOn = DateTime.Now,
                Id = 1,
                IsRead = false,
                User = new User(),
                UserId = "testUserId1"
            },
            new Notification()
            {
                Book = new Book(),
                BookId = 2,
                CreatedOn = DateTime.Now,
                Id = 2,
                IsRead = false,
                User = new User(),
                UserId = "testUserId2"
            }
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var notificationQueries = new NotificationQueries(fakeDbContext);

        var userId = "testUserId1";
        var expectedResult = fakeIQueryable.Where(n => n.UserId == userId).Select(x => x.IsRead == true);

        //Act
        notificationQueries.ReadAllNotification(userId);
        var result = notificationQueries.All(userId, 0, 12).Select(x => x.IsRead == true);

        //Assert
        Assert.Equal(expectedResult.Count(), result.Count());
    }
}
