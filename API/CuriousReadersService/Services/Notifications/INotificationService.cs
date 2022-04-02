namespace CuriousReadersService.Services.Notifications;
using CuriousReadersData.Entities;
using CuriousReadersService.Dto.Notifications;

public interface INotificationService
{
    Task<Notification> Create(CreateNotificationModel model);

    int CountUnread(string userId);

    IEnumerable<ReadNotificationModel> All(string userId, int skip, int notificationsPerPage);

    IEnumerable<ReadLibrarianNotificationModel> BooksNotOnTimeNotification(int skip, int notificationsPerPage);
}
