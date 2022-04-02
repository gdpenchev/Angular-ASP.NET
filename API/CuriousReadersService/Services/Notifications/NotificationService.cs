namespace CuriousReadersService.Services.Notifications;

using AutoMapper;
using CuriousReadersData.Commands;
using CuriousReadersData.Entities;
using CuriousReadersData.Queries;
using CuriousReadersService.Dto.Notifications;
using CuriousReadersService.Services.Mail;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Globalization;

public class NotificationService : INotificationService
{
    private readonly INotificationCommands notificationCommands;
    private readonly INotificationQueries notificationQuery;
    private readonly IReservationQueries reservationQueries;
    private readonly IMailService mailService;
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;

    public NotificationService(INotificationCommands notificationCommands,
        INotificationQueries notificationQuery,
        IReservationQueries reservationQueries,
        IMailService mailService,
        UserManager<User> userManager,
        IMapper mapper)
    {
        this.notificationCommands = notificationCommands;
        this.reservationQueries = reservationQueries;
        this.notificationQuery = notificationQuery;
        this.mailService = mailService;
        this.userManager = userManager;
        this.mapper = mapper;
    }

    public IEnumerable<ReadNotificationModel> All(string userId, int skip, int notificationsPerPage)
    {
        var allNotifications = this.notificationQuery.All(userId,skip,notificationsPerPage);

        var nofiticationModel = mapper.Map<IEnumerable<Notification>, List<ReadNotificationModel>>(allNotifications);

        ReadAllNotification(userId);

        return nofiticationModel;
    }

    public int CountUnread(string userId)
    {
        return this.notificationQuery.CountUnread(userId);
    }

    public async Task<Notification> Create(CreateNotificationModel model)
    {

        var user = await this.userManager.FindByEmailAsync(model.Email);

        var notification = mapper.Map<CreateNotificationModel, Notification>(model);

        notification.UserId = user.Id;

        await this.mailService.SendBookReminderEmail(model.Email);

        return this.notificationCommands.Create(notification);
    }

    public IEnumerable<ReadLibrarianNotificationModel> BooksNotOnTimeNotification(int skip, int notificationsPerPage)
    {
        var reservation = this.reservationQueries.BooksNotOnTimeNotification(skip, notificationsPerPage);

        var mappedNotification = mapper.Map<IEnumerable<Reservation>, List<ReadLibrarianNotificationModel>>(reservation);

        mappedNotification.ForEach(x => x.Date = x.ReturnDate.ToString("d", CultureInfo.GetCultureInfo("es-ES")));

        return mappedNotification;
    }

    private void ReadAllNotification(string userId)
    {
        this.notificationQuery.ReadAllNotification(userId);
    }

}
