namespace CuriousReadersData.Commands;
 
using CuriousReadersData.Entities;
public class NotificationCommands : INotificationCommands
{
    private readonly LibraryDbContext libraryDbContext;

    public NotificationCommands(LibraryDbContext libraryDbContext)
    {
        this.libraryDbContext = libraryDbContext;
    }
    public Notification Create(Notification notification)
    {
        this.libraryDbContext.Notifications.Add(notification);
        this.libraryDbContext.SaveChanges();

        return notification;
    }
}
