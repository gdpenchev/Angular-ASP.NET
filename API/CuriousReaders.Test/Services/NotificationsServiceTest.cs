namespace CuriousReaders.Test.Services
{
    using AutoMapper;
    using CuriousReadersData.Commands;
    using CuriousReadersData.Entities;
    using CuriousReadersData.Queries;
    using CuriousReadersService.Dto.Notifications;
    using CuriousReadersService.Profiles;
    using CuriousReadersService.Services.Mail;
    using CuriousReadersService.Services.Notifications;
    using FakeItEasy;
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using Xunit;
    public class NotificationsServiceTest
    {
        private readonly INotificationQueries notificationQueryMock = A.Fake<INotificationQueries>();
        private readonly INotificationCommands notificationCommandsMock = A.Fake<INotificationCommands>();
        private readonly IReservationQueries reservationQueriesMock = A.Fake<IReservationQueries>();
        private readonly CreateNotificationModel createNotificationModelMock = A.Fake<CreateNotificationModel>();
        private UserManager<User> userManager = A.Fake<UserManager<User>>();
        private IMailService mailServiceMock = A.Fake<IMailService>();
        private readonly int notificationsUnreadCount = 0;
        private NotificationService notificationService;
        private readonly IMapper mapper;

        public NotificationsServiceTest()
        {
            var config = new MapperConfiguration(config =>
            {
                config.AddProfile(new NotificationProfile());
            });

            this.mapper = new Mapper(config);
        }

        private void SetupService()
        {
            notificationService = new NotificationService(notificationCommandsMock, notificationQueryMock, reservationQueriesMock, mailServiceMock, userManager, mapper);
        }

        [Fact]
        public void CreateNotification_Creates_NewNotification()
        {
            //Arrange
            A.CallTo(() => notificationCommandsMock.Create(A<Notification>.Ignored))
                .Returns(new Notification());

            SetupService();

            //Act
             var result = notificationService.Create(createNotificationModelMock);

            //Assert
            A.CallTo(() => notificationCommandsMock.Create(A<Notification>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]

        public void AllNotification_Return_All()
        {
            //Arrange
            string userId = "testid";
            int skip = 1;
            int notificationsPerPage = 7;

            A.CallTo(() => notificationQueryMock.All(A<string>.Ignored, A<int>.Ignored, A<int>.Ignored))
                .Returns(new List<Notification>());
            SetupService();

            //Act
            notificationService.All(userId,skip,notificationsPerPage);

            //Assert
            A.CallTo(() => notificationQueryMock.All(userId, skip, notificationsPerPage))
            .MustHaveHappenedOnceExactly();
        }

        [Fact]

        public void CountNotification_Return_CountUnread()
        {
            //Arrange
            string userId = "testid";

            A.CallTo(() => notificationQueryMock.CountUnread(A<string>.Ignored))
                .Returns(notificationsUnreadCount);
            SetupService();

            //Act
            var result = notificationService.CountUnread(userId);

            //Assert
            Assert.Equal(notificationsUnreadCount, result);
            A.CallTo(() => notificationQueryMock.CountUnread(userId))
           .MustHaveHappenedOnceExactly();
        }
        [Fact]

        public void GetNotReturnedOnTime_Return_ReservationNotReturnOnTimeAsNotifications()
        {
            //Arrange
            int skip = 1;
            int notificationPerPage = 7;

            A.CallTo(() => reservationQueriesMock.BooksNotOnTimeNotification(A<int>.Ignored,A<int>.Ignored))
                .Returns(new List<Reservation>());
            SetupService();

            //Act
            var result = notificationService.BooksNotOnTimeNotification(skip,notificationPerPage);

            //Assert
            A.CallTo(() => reservationQueriesMock.BooksNotOnTimeNotification(skip,notificationPerPage))
           .MustHaveHappenedOnceExactly();
        }
    }
}
