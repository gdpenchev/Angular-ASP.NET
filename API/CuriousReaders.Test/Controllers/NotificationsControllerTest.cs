using CuriousReadersAPI.Controllers;
using CuriousReadersData.Entities;
using CuriousReadersService.Dto.Notifications;
using CuriousReadersService.Services.Notifications;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Xunit;

namespace CuriousReaders.Test.Controllers
{
    public class NotificationsControllerTest
    {
        private INotificationService notificationServiceMock = A.Fake<INotificationService>();
        private readonly int notificationsUnreadCount = 0;

        [Fact]
        public void CreateNotification_Returns_OkResult()
        {
            //Arrange

            A.CallTo(() => this.notificationServiceMock.Create(A<CreateNotificationModel>.Ignored))
                .Returns(new Notification());

            var notificationController = new NotificationsController(this.notificationServiceMock);

            var createNotificationModel = A.Fake<CreateNotificationModel>();

            //Act
            var result = notificationController.Create(createNotificationModel);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
        }
        [Fact]
        public void CountNotification_Returns_OkResult()
        {
            //Arrange
            string userId = "testId";

            A.CallTo(() => this.notificationServiceMock.CountUnread(A<string>.Ignored))
                .Returns(notificationsUnreadCount);

            var notificationController = new NotificationsController(this.notificationServiceMock);


            //Act
            var result = notificationController.Count(userId);

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void AllNotification_Returns_OkResult()
        {
            //Arrange
            string userId = "testId";
            int skip = 0;
            int notificationPerPage = 7;

            A.CallTo(() => this.notificationServiceMock.All(A<string>.Ignored,A<int>.Ignored,A<int>.Ignored))
                .Returns(new List<ReadNotificationModel>());

            var notificationController = new NotificationsController(this.notificationServiceMock);


            //Act
            var result = notificationController.All(userId,skip,notificationPerPage);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void LibrarianNotification_Returns_OkResult()
        {
            //Arrange
            int skip = 0;
            int notificationPerPage = 7;

            A.CallTo(() => this.notificationServiceMock.BooksNotOnTimeNotification(A<int>.Ignored, A<int>.Ignored))
                .Returns(new List<ReadLibrarianNotificationModel>());

            var notificationController = new NotificationsController(this.notificationServiceMock);


            //Act
            var result = notificationController.BooksNotOnTimeNotification(skip, notificationPerPage);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
