namespace CuriousReadersData.Queries;
using CuriousReadersData.Entities;
using Microsoft.EntityFrameworkCore;


public class NotificationQueries : INotificationQueries
{
    private readonly LibraryDbContext libraryDbContext;

    public NotificationQueries(LibraryDbContext libraryDbContext)
    {
        this.libraryDbContext = libraryDbContext;
    }

    public IEnumerable<Notification> All(string userId, int skip, int notificationsPerPage)
    {

        return this.libraryDbContext.Notifications
            .Where(n => n.UserId == userId)
            .Include(n => n.Book)
            .OrderByDescending(n => n.CreatedOn)
            .Skip(skip * notificationsPerPage)
            .Take(notificationsPerPage)
            .ToList();
    }

    public int CountUnread(string userId)
    {
        return this.libraryDbContext.Notifications
            .Where(n => n.UserId == userId && n.IsRead == false)
            .Count();
    }

    public void ReadAllNotification(string userId)
    {
        var notification = this.libraryDbContext.Notifications
            .Where(n => n.UserId == userId).ToList();

        notification.ForEach(n => n.IsRead = true);

        this.libraryDbContext.UpdateRange(notification);
        this.libraryDbContext.SaveChanges();

    }
}
