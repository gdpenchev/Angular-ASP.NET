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

public class NotificationCommandsTest
{
    private LibraryDbContext fakeDbContext = A.Fake<LibraryDbContext>();
    private Notification fakeNotification = A.Fake<Notification>();

    private void SetupFakeCommentsDbSet(IQueryable<Notification> fakeIQueryable)
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
    public void CreateNotification_ShouldCreate_NewNotification()
    {
        //Arrange
        var fakeIQueryableComments = new List<Notification>()
        {
            new Notification() { Id = 1, BookId = 1, CreatedOn = DateTime.Now, IsRead = true, UserId = "test"  },
        }
        .AsQueryable();

        SetupFakeCommentsDbSet(fakeIQueryableComments);

        var notificationCommand = new NotificationCommands(fakeDbContext);

        //Act
        var result = notificationCommand.Create(fakeNotification);

        //Assert
        A.CallTo(() => fakeDbContext.Notifications.Add(fakeNotification))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeDbContext.SaveChanges())
           .MustHaveHappenedOnceExactly();
    }
}
